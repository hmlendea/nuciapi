# NuciApiResponse

**Namespace:** `NuciAPI.Responses`
**Assembly:** `NuciAPI.dll`
**Base class:** `object` (abstract)

## Purpose

Abstract base class for all API responses. Defines common properties and HMAC operations.

## Syntax

```csharp
public abstract class NuciApiResponse
{
    protected NuciApiResponse(string message, string code);

    [JsonPropertyName("success")]
    [HmacOrder(9999999)]
    public abstract bool IsSuccessful { get; }

    [JsonPropertyName("message")]
    [HmacOrder(9999998)]
    public string Message { get; set; }

    [JsonPropertyName("code")]
    [HmacOrder(9999997)]
    public string Code { get; set; }

    [JsonPropertyName("hmac")]
    [JsonIgnore]
    [HmacIgnore]
    public string HmacToken { get; set; }

    public void SignHMAC(string secretKey);
    public bool HasValidHMAC(string secretKey);
    public void ValidateHMAC(string secretKey);
}
```

## Constructor

```csharp
protected NuciApiResponse(string message, string code)
```

- **Parameters:**
  - `message` — Human-readable message
  - `code` — Machine-readable code
- **Description:** Initialises `Message` and `Code` properties.

## Properties

### IsSuccessful (abstract)

```csharp
public abstract bool IsSuccessful { get; }
```

- **JSON:** `success`
- **HMAC order:** 9999999
- **Description:** Must be overridden by derived classes. `true` for success responses, `false` for error responses.

### Message

```csharp
public string Message { get; set; }
```

- **JSON:** `message`
- **HMAC order:** 9999998
- **Description:** Human-readable message describing the result.

### Code

```csharp
public string Code { get; set; }
```

- **JSON:** `code`
- **HMAC order:** 9999997
- **Description:** Machine-readable code for programmatic handling.

### HmacToken

```csharp
public string HmacToken { get; set; }
```

- **JSON:** `hmac` (excluded via `[JsonIgnore]`)
- **HMAC:** Excluded via `[HmacIgnore]`
- **Description:** HMAC token for response validation.

## Methods

### SignHMAC(string secretKey)

```csharp
public void SignHMAC(string secretKey)
```

- **Parameters:** `secretKey` — The secret key used to generate the HMAC token.
- **Description:** Generates HMAC token using `HmacEncoder.GenerateToken(this, secretKey)`.

### HasValidHMAC(string secretKey)

```csharp
public bool HasValidHMAC(string secretKey)
```

- **Parameters:** `secretKey` — The secret key used to validate the HMAC token.
- **Returns:** `true` if valid; otherwise, `false`.

### ValidateHMAC(string secretKey)

```csharp
public void ValidateHMAC(string secretKey)
```

- **Parameters:** `secretKey` — The secret key used to validate the HMAC token.
- **Throws:** `HmacValidationException` if invalid.

## Derived types

| Type | IsSuccessful | Content |
|------|--------------|---------|
| `NuciApiSuccessResponse` | `true` | Optional `NuciApiResponseContent` |
| `NuciApiErrorResponse` | `false` | None |
| `NuciApiContentResponse<T>` | `true` | Required `T : NuciApiResponseContent` |

## Serialisation example

```json
{
  "success": true,
  "message": "Operation completed successfully.",
  "code": "SUCCESS",
  "content": null
}
```

## Tests

- `NuciApiResponseTests.GivenAResponse_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated`
- `NuciApiResponseTests.GivenAResponse_WhenSigningTheHmac_ThenTheHmacTokenWasBuiltUsingAllProperties`