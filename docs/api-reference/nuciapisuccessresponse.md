# NuciApiSuccessResponse

**Namespace:** `NuciAPI.Responses`
**Assembly:** `NuciAPI.dll`
**Base class:** `NuciApiResponse`

## Purpose

Concrete successful response without typed payload content. Provides factory properties for common scenarios.

## Syntax

```csharp
public class NuciApiSuccessResponse : NuciApiResponse
{
    public override bool IsSuccessful => true;

    public virtual NuciApiResponseContent Content { get; set; }

    public NuciApiSuccessResponse();
    public NuciApiSuccessResponse(string message);
    public NuciApiSuccessResponse(string message, string code);

    public static NuciApiSuccessResponse FromMessage(string message);
    public static NuciApiSuccessResponse Default { get; }
    public static NuciApiSuccessResponse Created { get; }
    public static NuciApiSuccessResponse Deleted { get; }
    public static NuciApiSuccessResponse Fetched { get; }
    public static NuciApiSuccessResponse NotUpdated { get; }
    public static NuciApiSuccessResponse Updated { get; }
}
```

## Properties

### IsSuccessful

```csharp
public override bool IsSuccessful => true;
```

- Always returns `true`.

### Content

```csharp
public virtual NuciApiResponseContent Content { get; set; }
```

- Optional untyped payload content.
- Default: `null`.

## Constructors

```csharp
public NuciApiSuccessResponse()
    : base(NuciApiResponseMessages.SuccessMessages.Default, NuciApiResponseCodes.SuccessCodes.Default) { }

public NuciApiSuccessResponse(string message)
    : base(message, NuciApiResponseCodes.SuccessCodes.Default) { }

public NuciApiSuccessResponse(string message, string code)
    : base(message, code) { }
```

## Factory properties

| Property | Message | Code |
|----------|---------|------|
| `Default` | "Operation completed successfully." | "SUCCESS" |
| `Created` | "The new resource was successfully created." | "CREATED" |
| `Deleted` | "The resource was successfully deleted." | "DELETED" |
| `Fetched` | "The resource was successfully fetched." | "FETCHED" |
| `NotUpdated` | "The resource was not updated, as it already has the same content." | "NOT_UPDATED" |
| `Updated` | "The resource was successfully updated." | "UPDATED" |

## Factory methods

```csharp
public static NuciApiSuccessResponse FromMessage(string message)
```

- Creates instance with custom message, default success code ("SUCCESS").

## Serialisation example

```json
{
  "success": true,
  "message": "The new resource was successfully created.",
  "code": "CREATED",
  "content": null
}
```

## Usage example

```csharp
using NuciAPI.Responses;

// Predefined responses
var response = NuciApiSuccessResponse.Created;
response.SignHMAC("secret");

// Custom message
var response = NuciApiSuccessResponse.FromMessage("Order processed");

// With content
var response = new NuciApiSuccessResponse
{
    Content = new OrderResponseContent { OrderId = "ORD-123" }
};
response.SignHMAC("secret");
```

## Tests

- `NuciApiSuccessResponseTests.GivenASuccessResponse_WhenGettingTheIsSuccessfulProperty_ThenTrueIsReturned`
- `NuciApiSuccessResponseTests.GivenASuccessResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedMessageIsUsed`
- `NuciApiSuccessResponseTests.GivenASuccessResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedCodeIsUsed`
- `NuciApiSuccessResponseTests.GivenASuccessResponse_WhenCreatingTheDefaultResponse_ThenTheContentIsNull`
- `NuciApiSuccessResponseTests.GivenASuccessResponse_WhenSerialising_ThenTheMessageAndCodePropertiesAreAtTheRootLevel`