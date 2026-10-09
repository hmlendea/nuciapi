# Request Signing and Validation Flow

## Overview

This flow describes how a consumer signs an API request with HMAC and how the server validates it.

## Sequence diagram

```
Consumer                          Server
  │                                  │
  ├─ Create request DTO ────────────►│
  │                                  │
  ├─ request.SignHMAC(secretKey)     │
  │   │                              │
  │   ├─ HmacEncoder.GenerateToken(  │
  │   │   request, secretKey)        │
  │   │                              │
  │   │  1. Serialise all public     │
  │   │     properties (recursive)   │
  │   │     ordered by [HmacOrder]   │
  │   │  2. Compute HMAC-SHA256      │
  │   │     over serialised bytes    │
  │   │  3. Base64-encode result     │
  │   │                              │
  │   ◄─ base64 HMAC token ──────────│
  │   │                              │
  │   └─ request.HmacToken = token   │
  │                                  │
  ├─ Send request + HmacToken ──────►│
  │                                  │
  │                                  ├─ Receive request + HmacToken
  │                                  │
  │                                  ├─ request.HmacToken = received
  │                                  │
  │                                  ├─ request.ValidateHMAC(secretKey)
  │                                  │   │
  │                                  │   ├─ HmacValidator.Validate(
  │                                  │   │   HmacToken, request, secretKey)
  │                                  │   │
  │                                  │   │  1. Recompute HMAC from request
  │                                  │   │     (same algorithm as SignHMAC)
  │                                  │   │  2. Compare with provided token
  │                                  │   │  3. Constant-time comparison
  │                                  │   │
  │                                  │   ├─ Match: return (valid)
  │                                  │   │
  │                                  │   └─ Mismatch: throw HmacValidationException
  │                                  │
  │                                  └─ Process request (validated)
  │
  ◄─ Response ───────────────────────┤
```

## Detailed steps

### 1. Consumer creates request

```csharp
public class CreateOrderRequest : NuciApiRequest
{
    [HmacOrder(1)]
    public string CustomerId { get; set; }

    [HmacOrder(2)]
    public decimal Total { get; set; }
}

var request = new CreateOrderRequest
{
    CustomerId = "CUST-001",
    Total = 149.99m
};
```

### 2. Consumer signs request

```csharp
const string secretKey = "shared-secret-key";
request.SignHMAC(secretKey);
// request.HmacToken now populated
```

**Internal: `SignHMAC` implementation**
```csharp
public void SignHMAC(string secretKey)
    => HmacToken = HmacEncoder.GenerateToken(this, secretKey);
```

**Internal: `HmacEncoder.GenerateToken` behaviour**
1. Reflects all public instance properties of `request`
2. Recursively includes nested object properties
3. Orders by `[HmacOrder]` ascending (default 0)
4. Excludes properties with `[HmacIgnore]`
5. Serialises to canonical byte representation
6. Computes HMAC-SHA256(secretKey, bytes)
7. Returns Base64 string

### 3. Consumer transmits request

HTTP request includes:
- Body: JSON-serialised request (without `HmacToken` due to `[JsonIgnore]`)
- Header or body field: `HmacToken` value

### 4. Server receives and validates

```csharp
// Deserialise request body to CreateOrderRequest
var request = JsonSerializer.Deserialize<CreateOrderRequest>(body);
// Set received HMAC token
request.HmacToken = receivedHmacToken;

// Validate
request.ValidateHMAC(secretKey); // Throws if invalid
```

**Internal: `ValidateHMAC` implementation**
```csharp
public void ValidateHMAC(string secretKey)
    => HmacValidator.Validate(HmacToken, this, secretKey);
```

**Internal: `HmacValidator.Validate` behaviour**
1. Recomputes HMAC from `request` object (same as `GenerateToken`)
2. Decodes provided `HmacToken` from Base64
3. Performs constant-time comparison
4. Throws `HmacValidationException` on mismatch

### 5. Server processes validated request

Request is guaranteed authentic and unmodified.

## Error scenarios

| Scenario | Exception | Handling |
|----------|-----------|----------|
| `HmacToken` missing/null | `HmacValidationException` | Return 401 Unauthorized |
| `HmacToken` malformed Base64 | `HmacValidationException` | Return 400 Bad Request |
| HMAC mismatch (tampering) | `HmacValidationException` | Return 401 Unauthorized |
| `secretKey` null/empty | `ArgumentException` | Server configuration error (500) |

## Invariants

1. **Same object graph → same HMAC** — Deterministic for identical property values
2. **Different object graph → different HMAC** — Verified by tests
3. **`HmacToken` never included in its own computation** — `[HmacIgnore]` prevents circularity
4. **Order matters** — `[HmacOrder]` ensures deterministic property ordering
5. **Constant-time comparison** — Prevents timing attacks

## Tests

- `NuciApiRequestTests.GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated`
- `NuciApiRequestTests.GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenWasBuiltUsingAllProperties`