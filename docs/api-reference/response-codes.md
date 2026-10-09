# NuciApiResponseCodes

**Namespace:** `NuciAPI.Responses`
**Assembly:** `NuciAPI.dll`
**Type:** `static class`

## Purpose

Contains standard response codes used across the API. All values are `const string` — compile-time constants.

## Syntax

```csharp
public static class NuciApiResponseCodes
{
    public static class SuccessCodes { ... }
    public static class ErrorCodes { ... }
}
```

## SuccessCodes

```csharp
public static class SuccessCodes
{
    public const string Default = "SUCCESS";
    public const string Created = "CREATED";
    public const string Deleted = "DELETED";
    public const string Fetched = "FETCHED";
    public const string NotUpdated = "NOT_UPDATED";
    public const string Updated = "UPDATED";
}
```

| Constant | Value | Used by |
|----------|-------|---------|
| `Default` | `"SUCCESS"` | `NuciApiSuccessResponse.Default`, `NuciApiContentResponse` default |
| `Created` | `"CREATED"` | `NuciApiSuccessResponse.Created` |
| `Deleted` | `"DELETED"` | `NuciApiSuccessResponse.Deleted` |
| `Fetched` | `"FETCHED"` | `NuciApiSuccessResponse.Fetched` |
| `NotUpdated` | `"NOT_UPDATED"` | `NuciApiSuccessResponse.NotUpdated` |
| `Updated` | `"UPDATED"` | `NuciApiSuccessResponse.Updated` |

## ErrorCodes

```csharp
public static class ErrorCodes
{
    public const string Default = "ERROR";
    public const string AlreadyExists = "ALREADY_EXISTS";
    public const string AlreadyProcessed = "ALREADY_PROCESSED";
    public const string AuthenticationFailure = "AUTHENTICATION_FAILURE";
    public const string BadRequest = "BAD_REQUEST";
    public const string ClientClosedTheRequest = "CLIENT_CLOSED_THE_REQUEST";
    public const string InternalServerError = "INTERNAL_SERVER_ERROR";
    public const string InvalidRequest = "INVALID_REQUEST";
    public const string NotFound = "NOT_FOUND";
    public const string NotImplemented = "NOT_IMPLEMENTED";
    public const string ServiceUnavailable = "SERVICE_UNAVAILABLE";
}
```

| Constant | Value | Used by |
|----------|-------|---------|
| `Default` | `"ERROR"` | `NuciApiErrorResponse.Default`, `FromMessage` |
| `AlreadyExists` | `"ALREADY_EXISTS"` | `NuciApiErrorResponse.AlreadyExists` |
| `AlreadyProcessed` | `"ALREADY_PROCESSED"` | `NuciApiErrorResponse.AlreadyProcessed` |
| `AuthenticationFailure` | `"AUTHENTICATION_FAILURE"` | `NuciApiErrorResponse.AuthenticationFailure` |
| `BadRequest` | `"BAD_REQUEST"` | `NuciApiErrorResponse.BadRequest` |
| `ClientClosedTheRequest` | `"CLIENT_CLOSED_THE_REQUEST"` | `NuciApiErrorResponse.ClientClosedTheRequest` |
| `InternalServerError` | `"INTERNAL_SERVER_ERROR"` | `NuciApiErrorResponse.InternalServerError` |
| `InvalidRequest` | `"INVALID_REQUEST"` | `NuciApiErrorResponse.InvalidRequest` |
| `NotFound` | `"NOT_FOUND"` | `NuciApiErrorResponse.NotFound` |
| `NotImplemented` | `"NOT_IMPLEMENTED"` | (Available, no factory property) |
| `ServiceUnavailable` | `"SERVICE_UNAVAILABLE"` | (Available, no factory property) |

## Usage example

```csharp
using NuciAPI.Responses;

// In custom response
var response = new NuciApiSuccessResponse("Custom", NuciApiResponseCodes.SuccessCodes.Created);

// In error handling
if (error.Code == NuciApiResponseCodes.ErrorCodes.NotFound) { ... }
```

## Design notes

- All codes are uppercase with underscores (SCREAMING_SNAKE_CASE)
- Codes are stable — consumers can safely switch on them
- New codes can be added without breaking existing consumers
- No HTTP status code mapping — consumer maps as needed