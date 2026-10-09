# API Reference Index

> **Note:** NuciAPI is a .NET library, not a web API. This index documents the public types and members available for consumption.

## Types by namespace

### NuciAPI.Requests

| Type | Kind | Description |
|------|------|-------------|
| `NuciApiRequest` | `abstract class` | Base class for HMAC-signed API requests |

### NuciAPI.Responses

| Type | Kind | Description |
|------|------|-------------|
| `NuciApiResponse` | `abstract class` | Base class for all API responses |
| `NuciApiSuccessResponse` | `class` | Successful response without typed content |
| `NuciApiErrorResponse` | `sealed class` | Error response |
| `NuciApiContentResponse<TContent>` | `sealed class` | Generic successful response with typed content |
| `NuciApiResponseContent` | `abstract class` | Base class for response payload content |
| `NuciApiResponseCodes` | `static class` | Standardised success/error codes |
| `NuciApiResponseMessages` | `static class` | Standardised success/error messages |

## Quick reference

### Request signing/validation

```csharp
// Sign
request.SignHMAC(secretKey);

// Validate (throws on failure)
request.ValidateHMAC(secretKey);

// Check without throwing
bool valid = request.HasValidHMAC(secretKey);
```

### Response signing/validation

```csharp
// Sign
response.SignHMAC(secretKey);

// Validate (throws on failure)
response.ValidateHMAC(secretKey);

// Check without throwing
bool valid = response.HasValidHMAC(secretKey);
```

### Factory responses

```csharp
// Success
var ok = NuciApiSuccessResponse.Default;
var created = NuciApiSuccessResponse.Created;
var fetched = NuciApiSuccessResponse.Fetched;
var updated = NuciApiSuccessResponse.Updated;
var deleted = NuciApiSuccessResponse.Deleted;
var notUpdated = NuciApiSuccessResponse.NotUpdated;
var custom = NuciApiSuccessResponse.FromMessage("Custom message");

// Error
var err = NuciApiErrorResponse.Default;
var notFound = NuciApiErrorResponse.NotFound;
var badRequest = NuciApiErrorResponse.BadRequest;
var unauthorized = NuciApiErrorResponse.AuthenticationFailure;
var conflict = NuciApiErrorResponse.AlreadyExists;
var internalError = NuciApiErrorResponse.InternalServerError;
var customErr = NuciApiErrorResponse.FromMessage("Custom error");

// Content response
var content = new MyContent { Prop = "value" };
var contentResponse = new NuciApiContentResponse<MyContent>(content);
```

### Codes and messages

```csharp
// Success codes
string code = NuciApiResponseCodes.SuccessCodes.Created; // "CREATED"
string code = NuciApiResponseCodes.SuccessCodes.Updated; // "UPDATED"

// Error codes
string code = NuciApiResponseCodes.ErrorCodes.NotFound; // "NOT_FOUND"
string code = NuciApiResponseCodes.ErrorCodes.BadRequest; // "BAD_REQUEST"

// Success messages
string msg = NuciApiResponseMessages.SuccessMessages.Created; // "The new resource was successfully created."

// Error messages
string msg = NuciApiResponseMessages.ErrorMessages.NotFound; // "The requested resource was not found."
```

## Links to detailed references

- [NuciApiRequest](./nuciapirequest.md)
- [NuciApiResponse](./nuciapiresponse.md)
- [NuciApiSuccessResponse](./nuciapisuccessresponse.md)
- [NuciApiErrorResponse](./nuciapierrorresponse.md)
- [NuciApiContentResponse](./nuciapicontentresponse.md)
- [NuciApiResponseContent](./nuciapiresponsecontent.md)
- [NuciApiResponseCodes](./response-codes.md)
- [NuciApiResponseMessages](./response-messages.md)