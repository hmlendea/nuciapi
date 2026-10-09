# NuciApiRequest

**Namespace:** `NuciAPI.Requests`
**Assembly:** `NuciAPI.dll`
**Base class:** `object` (abstract)

## Purpose

Abstract base class for all API request DTOs. Provides HMAC signing and validation capabilities.

## Syntax

```csharp
public abstract class NuciApiRequest
{
    [JsonIgnore]
    [HmacIgnore]
    public string HmacToken { get; set; }

    public void SignHMAC(string secretKey);
    public bool HasValidHMAC(string secretKey);
    public void ValidateHMAC(string secretKey);
}
```

## Members

### HmacToken

```csharp
public string HmacToken { get; set; }
```

- **Attributes:** `[JsonIgnore]`, `[HmacIgnore]`
- **Description:** HMAC token for request validation. Excluded from JSON serialisation and HMAC computation.
- **Default:** `null`

### SignHMAC(string secretKey)

```csharp
public void SignHMAC(string secretKey)
```

- **Parameters:** `secretKey` — The secret key used to generate the HMAC token.
- **Description:** Generates an HMAC token using `HmacEncoder.GenerateToken(this, secretKey)` and assigns it to `HmacToken`.
- **Throws:** `ArgumentException` if `secretKey` is null or empty.

### HasValidHMAC(string secretKey)

```csharp
public bool HasValidHMAC(string secretKey)
```

- **Parameters:** `secretKey` — The secret key used to validate the HMAC token.
- **Returns:** `true` if the HMAC token is valid; otherwise, `false`.
- **Description:** Calls `HmacValidator.IsTokenValid(HmacToken, this, secretKey)`.

### ValidateHMAC(string secretKey)

```csharp
public void ValidateHMAC(string secretKey)
```

- **Parameters:** `secretKey` — The secret key used to validate the HMAC token.
- **Description:** Calls `HmacValidator.Validate(HmacToken, this, secretKey)`.
- **Throws:** `HmacValidationException` if the token is invalid, missing, or malformed.

## Usage example

```csharp
using NuciAPI.Requests;
using NuciSecurity.HMAC;

public class CreateOrderRequest : NuciApiRequest
{
    [HmacOrder(1)]
    public string CustomerId { get; set; }

    [HmacOrder(2)]
    public decimal Total { get; set; }
}

// Client side
var request = new CreateOrderRequest
{
    CustomerId = "CUST-001",
    Total = 149.99m
};
request.SignHMAC("shared-secret-key");
// Send request.HmacToken with request

// Server side
request.HmacToken = receivedToken;
request.ValidateHMAC("shared-secret-key"); // Throws if invalid
```

## Inheritance notes

- Derived classes automatically participate in HMAC via reflection
- Apply `[HmacOrder(n)]` to properties to control inclusion order
- Do not override `HmacToken` — it is sealed by base implementation
- All public instance properties are included unless marked `[HmacIgnore]`

## Tests

- `NuciApiRequestTests.GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated`
- `NuciApiRequestTests.GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenWasBuiltUsingAllProperties`