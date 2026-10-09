# Response Signing and Validation Flow

## Overview

This flow describes how a server signs an API response with HMAC and how the client validates it.

## Sequence diagram

```
Server                          Client
  │                                  │
  ├─ Create response DTO ──────────►│
  │                                  │
  ├─ response.SignHMAC(secretKey)   │
  │   │                              │
  │   ├─ HmacEncoder.GenerateToken(  │
  │   │   response, secretKey)        │
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
  │   └─ response.HmacToken = token │
  │                                  │
  ├─ Send response + HmacToken ─────►│
  │                                  │
  │                                  ├─ Receive response + HmacToken
  │                                  │
  │                                  ├─ response.HmacToken = received
  │                                  │
  │                                  ├─ response.ValidateHMAC(secretKey)
  │                                  │   │
  │                                  │   ├─ HmacValidator.Validate(
  │                                  │   │   HmacToken, response, secretKey)
  │                                  │   │
  │                                  │   │  1. Recompute HMAC from response
  │                                  │   │     (same algorithm as SignHMAC)
  │                                  │   │  2. Compare with provided token
  │                                  │   │  3. Constant-time comparison
  │                                  │   │
  │                                  │   ├─ Match: return (valid)
  │                                  │   │
  │                                  │   └─ Mismatch: throw HmacValidationException
  │                                  │
  ◄─ Acknowledgement ─────────────────┤
```

## Detailed steps

### 1. Server creates response

```csharp
// Success response without content
var response = NuciApiSuccessResponse.Created;

// Success response with typed content
var content = new OrderResponseContent { OrderId = "ORD-123", Total = 99.99m };
var response = new NuciApiContentResponse<OrderResponseContent>(content);

// Error response
var response = NuciApiErrorResponse.BadRequest;
```

### 2. Server signs response

```csharp
const string secretKey = "shared-secret-key";
response.SignHMAC(secretKey);
// response.HmacToken now populated
```

**Internal: `SignHMAC` implementation**
```csharp
public void SignHMAC(string secretKey)
    => HmacToken = HmacEncoder.GenerateToken(this, secretKey);
```

**Internal: `HmacEncoder.GenerateToken` behaviour**
1. Reflects all public instance properties of `response`
2. Recursively includes nested object properties
3. Orders by `[HmacOrder]` ascending (default 0)
4. Excludes properties with `[HmacIgnore]`
5. Serialises to canonical byte representation
6. Computes HMAC-SHA256(secretKey, bytes)
7. Returns Base64 string

### 3. Server transmits response

HTTP response includes:
- Body: JSON-serialised response (without `HmacToken` due to `[JsonIgnore]`)
- Header or body field: `HmacToken` value

### 4. Client receives and validates

```csharp
// Deserialise response body to appropriate type
var response = JsonSerializer.Deserialize<NuciApiSuccessResponse>(body);
// Set received HMAC token
response.HmacToken = receivedHmacToken;

// Validate
response.ValidateHMAC(secretKey); // Throws if invalid
```

**Internal: `ValidateHMAC` implementation**
```csharp
public void ValidateHMAC(string secretKey)
    => HmacValidator.Validate(HmacToken, this, secretKey);
```

**Internal: `HmacValidator.Validate` behaviour**
1. Recomputes HMAC from `response` object (same as `GenerateToken`)
2. Decodes provided `HmacToken` from Base64
3. Performs constant-time comparison
4. Throws `HmacValidationException` on mismatch

### 5. Client processes validated response

Response is guaranteed authentic and unmodified.

## Error scenarios

| Scenario | Exception | Handling |
|----------|-----------|----------|
| `HmacToken` missing/null | `HmacValidationException` | Return 400 Bad Request |
| `HmacToken` malformed Base64 | `HmacValidationException` | Return 400 Bad Request |
| HMAC mismatch (tampering) | `HmacValidationException` | Return 401 Unauthorized |
| `secretKey` null/empty | `ArgumentException` | Client configuration error (500) |

## Invariants

1. **Same object graph → same HMAC** — Deterministic for identical property values
2. **Different object graph → different HMAC** — Verified by tests
3. **`HmacToken` never included in its own computation** — `[HmacIgnore]` prevents circularity
4. **Order matters** — `[HmacOrder]` ensures deterministic property ordering
5. **Constant-time comparison** — Prevents timing attacks

## Tests

- `NuciApiResponseTests.GivenAResponse_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated`
- `NuciApiResponseTests.GivenAResponse_WhenSigningTheHmac_ThenTheHmacTokenWasBuiltUsingAllProperties`