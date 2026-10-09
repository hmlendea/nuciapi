# Browse and Search Patterns

## Finding the right response type

### By intent

| Intent | Response type | Factory property |
|--------|---------------|------------------|
| Generic success | `NuciApiSuccessResponse` | `.Default` |
| Resource created | `NuciApiSuccessResponse` | `.Created` |
| Resource deleted | `NuciApiSuccessResponse` | `.Deleted` |
| Data retrieved | `NuciApiSuccessResponse` | `.Fetched` |
| No changes made | `NuciApiSuccessResponse` | `.NotUpdated` |
| Resource updated | `NuciApiSuccessResponse` | `.Updated` |
| Custom success | `NuciApiSuccessResponse` | `.FromMessage()` |
| Typed payload success | `NuciApiContentResponse<T>` | Constructor |
| Generic error | `NuciApiErrorResponse` | `.Default` |
| Not found | `NuciApiErrorResponse` | `.NotFound()` |
| Bad input | `NuciApiErrorResponse` | `.BadRequest()` |
| Auth failure | `NuciApiErrorResponse` | `.AuthenticationFailure()` |
| Duplicate | `NuciApiErrorResponse` | `.AlreadyExists()` |
| Idempotent replay | `NuciApiErrorResponse` | `.AlreadyProcessed()` |
| Server error | `NuciApiErrorResponse` | `.InternalServerError()` |
| Malformed request | `NuciApiErrorResponse` | `.InvalidRequest()` |
| Client disconnected | `NuciApiErrorResponse` | `.ClientClosedTheRequest()` |
| Custom error | `NuciApiErrorResponse` | `.FromMessage()` |

### By HTTP status code mapping

| HTTP Status | Recommended response |
|-------------|---------------------|
| 200 OK | `NuciApiSuccessResponse.Default` or `NuciApiContentResponse<T>` |
| 201 Created | `NuciApiSuccessResponse.Created` |
| 204 No Content | `NuciApiSuccessResponse.Deleted` or `NuciApiSuccessResponse.NotUpdated` |
| 400 Bad Request | `NuciApiErrorResponse.BadRequest()` or `NuciApiErrorResponse.InvalidRequest()` |
| 401 Unauthorized | `NuciApiErrorResponse.AuthenticationFailure()` |
| 404 Not Found | `NuciApiErrorResponse.NotFound()` |
| 409 Conflict | `NuciApiErrorResponse.AlreadyExists()` |
| 422 Unprocessable Entity | `NuciApiErrorResponse.InvalidRequest()` |
| 429 Too Many Requests | `NuciApiErrorResponse.FromMessage("Rate limited", "RATE_LIMITED")` |
| 500 Internal Server Error | `NuciApiErrorResponse.InternalServerError()` |
| 499 Client Closed Request | `NuciApiErrorResponse.ClientClosedTheRequest()` |

## Discovering response codes and messages

### Success codes

```csharp
// All success codes
NuciApiResponseCodes.SuccessCodes
// Returns: SUCCESS, CREATED, DELETED, FETCHED, NOT_UPDATED, UPDATED

// All success messages
NuciApiResponseMessages.SuccessMessages
// Returns corresponding messages
```

### Error codes

```csharp
// All error codes
NuciApiResponseCodes.ErrorCodes
// Returns: ERROR, NOT_FOUND, BAD_REQUEST, AUTHENTICATION_FAILURE,
//          ALREADY_EXISTS, ALREADY_PROCESSED, INTERNAL_SERVER_ERROR,
//          INVALID_REQUEST, CLIENT_CLOSED_REQUEST, VALIDATION_ERROR,
//          RATE_LIMITED

// All error messages
NuciApiResponseMessages.ErrorMessages
// Returns corresponding messages
```

## Searching the API surface

### By namespace

```
NuciAPI.Requests
  └── NuciApiRequest (abstract base)

NuciAPI.Responses
  ├── NuciApiResponse (abstract base)
  ├── NuciApiSuccessResponse (concrete)
  ├── NuciApiErrorResponse (sealed)
  ├── NuciApiContentResponse<T> (sealed generic)
  ├── NuciApiResponseContent (abstract base)
  ├── NuciApiResponseCodes (static)
  └── NuciApiResponseMessages (static)
```

### By inheritance hierarchy

```
NuciApiRequest
  └── (your custom requests)

NuciApiResponse
  ├── NuciApiSuccessResponse
  │     └── NuciApiContentResponse<T>
  └── NuciApiErrorResponse

NuciApiResponseContent
  └── (your custom content types)
```

## IDE navigation tips

### Visual Studio / VS Code

- **Go to Definition** (F12) on any type → see source
- **Go to Implementation** (Ctrl+F12) on `NuciApiRequest`/`NuciApiResponse` → see your derived types
- **Find All References** (Shift+F12) on factory properties → see usage patterns
- **Peek Definition** (Alt+F12) → inline view without navigation

### Symbol search

```
NuciApiRequest      → Base request class
NuciApiResponse     → Base response class
SignHMAC            → Signing method
HasValidHMAC        → Validation method
ValidateHMAC        → Validation with exception
HmacToken           → Signature property
IsSuccessful        → Success discriminator
Content             → Payload property (on NuciApiContentResponse<T>)
```

## Filtering by capability

### Types supporting HMAC

All request and response types (via base classes):
- `NuciApiRequest` and derivatives
- `NuciApiResponse` and derivatives

### Types with typed content

- `NuciApiContentResponse<T>` where `T : NuciApiResponseContent`

### Types with factory properties

- `NuciApiSuccessResponse` (6 success factories + `FromMessage`)
- `NuciApiErrorResponse` (9 error factories + `FromMessage`)

### Types for extension

| Extend this | For |
|-------------|-----|
| `NuciApiRequest` | Custom request DTOs |
| `NuciApiResponseContent` | Custom response payloads |
| `NuciApiContentResponse<T>` | Typed success responses |

## Common search queries

### "How do I create a signed request?"

1. Inherit `NuciApiRequest`
2. Add properties
3. Call `SignHMAC(key)` before sending

### "How do I return typed data?"

1. Create `TContent : NuciApiResponseContent`
2. Create `MyResponse : NuciApiContentResponse<TContent>`
3. Set `Content` property
4. Call `SignHMAC(key)`

### "How do I return an error?"

Use `NuciApiErrorResponse` factory properties or `FromMessage()`

### "How do I validate incoming request?"

Call `HasValidHMAC(key)` or `ValidateHMAC(key)` on deserialized request

### "What codes are available?"

Check `NuciApiResponseCodes.SuccessCodes` and `ErrorCodes`

### "Can I customise the message?"

Yes, use `FromMessage(message, code)` on both success and error responses