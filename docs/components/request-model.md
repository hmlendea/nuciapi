# Request Model

## NuciApiRequest

**Location:** `NuciAPI/Requests/NuciApiRequest.cs`
**Namespace:** `NuciAPI.Requests`
**Base class:** `object` (abstract)

### Purpose

Abstract base class for all API request DTOs. Provides HMAC signing and validation capabilities.

### Public API

| Member | Type | Description |
|--------|------|-------------|
| `HmacToken` | `string` | HMAC token for request validation. Marked `[JsonIgnore]` and `[HmacIgnore]` — excluded from serialisation and HMAC computation. |
| `SignHMAC(string secretKey)` | `void` | Generates HMAC token using `HmacEncoder.GenerateToken(this, secretKey)` and assigns to `HmacToken`. |
| `HasValidHMAC(string secretKey)` | `bool` | Returns `HmacValidator.IsTokenValid(HmacToken, this, secretKey)`. |
| `ValidateHMAC(string secretKey)` | `void` | Calls `HmacValidator.Validate(HmacToken, this, secretKey)`; throws on invalid token. |

### HMAC integration

- Uses `NuciSecurity.HMAC.HmacEncoder.GenerateToken(object, string)`
- Uses `NuciSecurity.HMAC.HmacValidator.IsTokenValid(string, object, string)` and `Validate(string, object, string)`
- `[HmacIgnore]` on `HmacToken` prevents circular inclusion
- All other public properties (in derived classes) are included in HMAC computation by default
- `[HmacOrder(n)]` can be applied to derived class properties to control inclusion order

### Usage pattern

```csharp
public class CreateOrderRequest : NuciApiRequest
{
    public string CustomerId { get; set; }
    public decimal Total { get; set; }
}

var request = new CreateOrderRequest { CustomerId = "CUST-001", Total = 149.99m };
request.SignHMAC("secret-key");
// Send request...

// Server side:
request.ValidateHMAC("secret-key"); // throws if invalid
```

### Invariants

1. `HmacToken` is `null` until `SignHMAC` is called
2. `SignHMAC` overwrites any existing `HmacToken`
3. `ValidateHMAC` throws `HmacValidationException` (from NuciSecurity.HMAC) on failure
4. Derived classes must not mark `HmacToken` with `[HmacOrder]` — it is ignored

### Tests

- `NuciApiRequestTests.GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated`
- `NuciApiRequestTests.GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenWasBuiltUsingAllProperties`