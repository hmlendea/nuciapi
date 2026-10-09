# NuciApiErrorResponse

**Namespace:** `NuciAPI.Responses`
**Assembly:** `NuciAPI.dll`
**Base class:** `NuciApiResponse`
**Sealed:** Yes

## Purpose

Concrete error response. Sealed to prevent further inheritance.

## Syntax

```csharp
public sealed class NuciApiErrorResponse : NuciApiResponse
{
    public override bool IsSuccessful => false;

    public NuciApiErrorResponse();
    public NuciApiErrorResponse(string message, string code);

    public static NuciApiErrorResponse FromMessage(string message);
    public static NuciApiErrorResponse Default { get; }
    public static NuciApiErrorResponse AlreadyExists { get; }
    public static NuciApiErrorResponse AlreadyProcessed { get; }
    public static NuciApiErrorResponse AuthenticationFailure { get; }
    public static NuciApiErrorResponse BadRequest { get; }
    public static NuciApiErrorResponse ClientClosedTheRequest { get; }
    public static NuciApiErrorResponse InternalServerError { get; }
    public static NuciApiErrorResponse InvalidRequest { get; }
    public static NuciApiErrorResponse NotFound { get; }
}
```

## Properties

### IsSuccessful

```csharp
public override bool IsSuccessful => false;
```

- Always returns `false`.

### Content

- **Not present** — Error responses do not have a `Content` property.

## Constructors

```csharp
public NuciApiErrorResponse()
    : base(NuciApiResponseMessages.ErrorMessages.Default, NuciApiResponseCodes.ErrorCodes.Default) { }

public NuciApiErrorResponse(string message, string code)
    : base(message, code) { }
```

## Factory properties

| Property | Message | Code |
|----------|---------|------|
| `Default` | "An error occurred while processing your request." | "ERROR" |
| `AlreadyExists` | "The requested resource already exists." | "ALREADY_EXISTS" |
| `AlreadyProcessed` | "The request has already been processed." | "ALREADY_PROCESSED" |
| `AuthenticationFailure` | "The authentication has failed." | "AUTHENTICATION_FAILURE" |
| `BadRequest` | "The request had failed due to invalid or missing parameters." | "BAD_REQUEST" |
| `ClientClosedTheRequest` | "The client has closed the request." | "CLIENT_CLOSED_THE_REQUEST" |
| `InternalServerError` | "An internal server error has occurred." | "INTERNAL_SERVER_ERROR" |
| `InvalidRequest` | "The request is invalid." | "INVALID_REQUEST" |
| `NotFound` | "The requested resource was not found." | "NOT_FOUND" |

## Factory methods

```csharp
public static NuciApiErrorResponse FromMessage(string message)
```

- Creates instance with custom message, default error code ("ERROR").

## Serialisation example

```json
{
  "success": false,
  "message": "The requested resource was not found.",
  "code": "NOT_FOUND"
}
```

**Note:** No `content` property in JSON output.

## Usage example

```csharp
using NuciAPI.Responses;

// Predefined errors
var response = NuciApiErrorResponse.NotFound;
response.SignHMAC("secret");

// Custom error
var response = NuciApiErrorResponse.FromMessage("Custom validation failed");
response.SignHMAC("secret");
```

## Tests

- `NuciApiErrorResponseTests.GivenAnErrorResponse_WhenGettingTheIsSuccessfulProperty_ThenFalseIsReturned`
- `NuciApiErrorResponseTests.GivenAnErrorResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedMessageIsUsed`
- `NuciApiErrorResponseTests.GivenAnErrorResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedCodeIsUsed`
- `NuciApiErrorResponseTests.GivenAnErrorResponse_WhenCreatingTheInvalidRequestResponse_ThenTheExpectedMessageIsUsed`
- `NuciApiErrorResponseTests.GivenAnErrorResponse_WhenSerialising_ThenTheContentPropertyIsAbsent`