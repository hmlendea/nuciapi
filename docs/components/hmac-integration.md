# HMAC Integration

## External dependency

**Package:** `NuciSecurity.HMAC` version 4.1.3
**Namespace:** `NuciSecurity.HMAC`

## Integration points

### Attributes (compile-time)

| Attribute | Applied to | Purpose |
|-----------|------------|---------|
| `[HmacIgnore]` | `NuciApiRequest.HmacToken`, `NuciApiResponse.HmacToken` | Excludes property from HMAC computation and validation |
| `[HmacOrder(int)]` | `NuciApiResponse.IsSuccessful` (9999999), `Message` (9999998), `Code` (9999997) | Controls property inclusion order in HMAC; high values = included last |
| `[HmacOrder(int)]` | Derived content properties (e.g., `DummyResponseContent.DummyProperty` = 1) | Consumer-controlled order for payload properties |

### Runtime calls

| NuciAPI method | NuciSecurity.HMAC call | Direction |
|----------------|------------------------|-----------|
| `NuciApiRequest.SignHMAC` | `HmacEncoder.GenerateToken(this, secretKey)` | Request → HMAC |
| `NuciApiRequest.HasValidHMAC` | `HmacValidator.IsTokenValid(HmacToken, this, secretKey)` | Request → HMAC |
| `NuciApiRequest.ValidateHMAC` | `HmacValidator.Validate(HmacToken, this, secretKey)` | Request → HMAC |
| `NuciApiResponse.SignHMAC` | `HmacEncoder.GenerateToken(this, secretKey)` | Response → HMAC |
| `NuciApiResponse.HasValidHMAC` | `HmacValidator.IsTokenValid(HmacToken, this, secretKey)` | Response → HMAC |
| `NuciApiResponse.ValidateHMAC` | `HmacValidator.Validate(HmacToken, this, secretKey)` | Response → HMAC |

## HMAC computation model

The HMAC is computed over the **entire object graph** including:
- All public instance properties of the request/response object
- All public instance properties of nested objects (recursively)
- Properties are ordered by `[HmacOrder]` value (ascending), then by declaration order
- Properties marked `[HmacIgnore]` are excluded entirely

### Property inclusion order (base classes)

1. Consumer-defined request/response properties (default order = 0, or explicit `[HmacOrder]`)
2. `Code` (order 9999997)
3. `Message` (order 9999998)
4. `IsSuccessful` (order 9999999)
5. `HmacToken` — **excluded** via `[HmacIgnore]`

### Example: Request HMAC

```csharp
public class CreateOrderRequest : NuciApiRequest
{
    [HmacOrder(1)]
    public string CustomerId { get; set; }

    [HmacOrder(2)]
    public decimal Total { get; set; }
}

// HMAC covers: CustomerId → Total → (base: Code → Message → IsSuccessful)
// HmacToken excluded
```

### Example: Response HMAC

```csharp
public class OrderResponseContent : NuciApiResponseContent
{
    [HmacOrder(1)]
    public string OrderId { get; set; }

    [HmacOrder(2)]
    public decimal Total { get; set; }
}

var response = new NuciApiContentResponse<OrderResponseContent>(
    new OrderResponseContent { OrderId = "ORD-123", Total = 99.99m }
);

// HMAC covers: OrderId → Total → Code → Message → IsSuccessful
// HmacToken excluded
```

## Validation flow

### Client-side (signing)

```csharp
var request = new CreateOrderRequest { CustomerId = "CUST-001", Total = 149.99m };
request.SignHMAC("shared-secret");
// request.HmacToken now contains base64-encoded HMAC
// Send request + HmacToken to server
```

### Server-side (validation)

```csharp
// Receive request + HmacToken
request.HmacToken = receivedHmacToken;
request.ValidateHMAC("shared-secret"); // Throws HmacValidationException if invalid
// Request is authentic and unmodified
```

### Response signing (server)

```csharp
var response = NuciApiSuccessResponse.Created;
response.SignHMAC("shared-secret");
// response.HmacToken contains HMAC
// Send response to client
```

### Response validation (client)

```csharp
response.HmacToken = receivedHmacToken;
response.ValidateHMAC("shared-secret"); // Throws if invalid
// Response is authentic and unmodified
```

## Error handling

| Scenario | Exception | Source |
|----------|-----------|--------|
| Invalid HMAC token format | `HmacValidationException` | `HmacValidator.Validate` |
| HMAC mismatch (tampered data) | `HmacValidationException` | `HmacValidator.Validate` |
| Missing `HmacToken` | `HmacValidationException` | `HmacValidator.Validate` |
| `secretKey` null/empty | `ArgumentException` | `HmacEncoder.GenerateToken` / `HmacValidator` |

## Security considerations

1. **Secret key management** — Consumer responsible for secure storage/rotation of HMAC secret keys
2. **Replay protection** — Not provided by library; consumer must implement (timestamps, nonces)
3. **Algorithm** — Determined by `NuciSecurity.HMAC` implementation (HMAC-SHA256 by default)
4. **Token format** — Base64-encoded HMAC; opaque to consumer
5. **Key length** — Should be ≥ 256 bits for HMAC-SHA256

## Testing

- `NuciApiRequestTests` — Verifies token generation and property inclusion
- `NuciApiResponseTests` — Verifies response token generation and property inclusion
- Both use `DummySecretKey = "DummySecretKey123!"` for deterministic tests
- Tests confirm different object graphs produce different tokens