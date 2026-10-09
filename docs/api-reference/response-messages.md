# NuciApiResponseMessages

**Namespace:** `NuciAPI.Responses`
**Assembly:** `NuciAPI.dll`
**Type:** `static class`

## Purpose

Contains standard response messages used across the API. All values are `const string` — compile-time constants.

## Syntax

```csharp
public static class NuciApiResponseMessages
{
    public static class SuccessMessages { ... }
    public static class ErrorMessages { ... }
}
```

## SuccessMessages

```csharp
public static class SuccessMessages
{
    public const string Default = "Operation completed successfully.";
    public const string Created = "The new resource was successfully created.";
    public const string Deleted = "The resource was successfully deleted.";
    public const string Fetched = "The resource was successfully fetched.";
    public const string NotUpdated = "The resource was not updated, as it already has the same content.";
    public const string Updated = "The resource was successfully updated.";
}
```

| Constant | Value | Used by |
|----------|-------|---------|
| `Default` | "Operation completed successfully." | `NuciApiSuccessResponse.Default`, `NuciApiContentResponse` default |
| `Created` | "The new resource was successfully created." | `NuciApiSuccessResponse.Created` |
| `Deleted` | "The resource was successfully deleted." | `NuciApiSuccessResponse.Deleted` |
| `Fetched` | "The resource was successfully fetched." | `NuciApiSuccessResponse.Fetched` |
| `NotUpdated` | "The resource was not updated, as it already has the same content." | `NuciApiSuccessResponse.NotUpdated` |
| `Updated` | "The resource was successfully updated." | `NuciApiSuccessResponse.Updated` |

## ErrorMessages

```csharp
public static class ErrorMessages
{
    public const string Default = "An error occurred while processing your request.";
    public const string AlreadyExists = "The requested resource already exists.";
    public const string AlreadyProcessed = "The request has already been processed.";
    public const string AuthenticationFailure = "The authentication has failed.";
    public const string BadRequest = "The request had failed due to invalid or missing parameters.";
    public const string ClientClosedTheRequest = "The client has closed the request.";
    public const string InternalServerError = "An internal server error has occurred.";
    public const string InvalidRequest = "The request is invalid.";
    public const string NotFound = "The requested resource was not found.";
    public const string NotImplemented = "The requested functionality is not implemented.";
    public const string ServiceUnavailable = "The service dependency is unavailable.";
}
```

| Constant | Value | Used by |
|----------|-------|---------|
| `Default` | "An error occurred while processing your request." | `NuciApiErrorResponse.Default`, `FromMessage` |
| `AlreadyExists` | "The requested resource already exists." | `NuciApiErrorResponse.AlreadyExists` |
| `AlreadyProcessed` | "The request has already been processed." | `NuciApiErrorResponse.AlreadyProcessed` |
| `AuthenticationFailure` | "The authentication has failed." | `NuciApiErrorResponse.AuthenticationFailure` |
| `BadRequest` | "The request had failed due to invalid or missing parameters." | `NuciApiErrorResponse.BadRequest` |
| `ClientClosedTheRequest` | "The client has closed the request." | `NuciApiErrorResponse.ClientClosedTheRequest` |
| `InternalServerError` | "An internal server error has occurred." | `NuciApiErrorResponse.InternalServerError` |
| `InvalidRequest` | "The request is invalid." | `NuciApiErrorResponse.InvalidRequest` |
| `NotFound` | "The requested resource was not found." | `NuciApiErrorResponse.NotFound` |
| `NotImplemented` | "The requested functionality is not implemented." | (Available, no factory property) |
| `ServiceUnavailable` | "The service dependency is unavailable." | (Available, no factory property) |

## Usage example

```csharp
using NuciAPI.Responses;

// In custom response
var response = new NuciApiSuccessResponse(NuciApiResponseMessages.SuccessMessages.Created);

// In error handling
if (error.Message == NuciApiResponseMessages.ErrorMessages.NotFound) { ... }
```

## Design notes

- Messages are human-readable, sentence-case with trailing period
- Messages are stable — consumers can display them directly
- New messages can be added without breaking existing consumers
- No localisation support — messages are English only
- Codes (`NuciApiResponseCodes`) should be used for programmatic handling