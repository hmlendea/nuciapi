# Response Model

## Class hierarchy

```
NuciApiResponse (abstract)
├── NuciApiSuccessResponse
│   └── NuciApiContentResponse<TContent> (sealed, TContent : NuciApiResponseContent)
└── NuciApiErrorResponse (sealed)
```

## NuciApiResponse (abstract base)

**Location:** `NuciAPI/Responses/NuciApiResponse.cs`
**Namespace:** `NuciAPI.Responses`

### Purpose

Abstract base for all API responses. Defines common properties and HMAC operations.

### Constructor

```csharp
protected NuciApiResponse(string message, string code)
```

### Properties

| Property | Type | JSON name | HMAC order | Description |
|----------|------|-----------|------------|-------------|
| `IsSuccessful` | `bool` (abstract) | `success` | 9999999 | Must be overridden: `true` for success, `false` for error |
| `Message` | `string` | `message` | 9999998 | Human-readable message |
| `Code` | `string` | `code` | 9999997 | Machine-readable code |
| `HmacToken` | `string` | `hmac` | Ignored | HMAC token; `[JsonIgnore]` + `[HmacIgnore]` |

### HMAC methods

| Method | Description |
|--------|-------------|
| `SignHMAC(string secretKey)` | `HmacToken = HmacEncoder.GenerateToken(this, secretKey)` |
| `HasValidHMAC(string secretKey)` | `HmacValidator.IsTokenValid(HmacToken, this, secretKey)` |
| `ValidateHMAC(string secretKey)` | `HmacValidator.Validate(HmacToken, this, secretKey)` |

### Serialisation notes

- `JsonPropertyName` attributes map to lowercase JSON keys
- `HmacToken` excluded from JSON output (`[JsonIgnore]`)
- High `HmacOrder` values ensure metadata properties are included last in HMAC computation

---

## NuciApiSuccessResponse

**Location:** `NuciAPI/Responses/NuciApiSuccessResponse.cs`
**Namespace:** `NuciAPI.Responses`

### Purpose

Concrete successful response without typed payload content. Provides factory properties for common scenarios.

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `IsSuccessful` | `bool` | Always `true` (override) |
| `Content` | `NuciApiResponseContent` | Virtual, defaults to `null`; for optional untyped payload |

### Constructors

```csharp
public NuciApiSuccessResponse() : base(DefaultMessage, DefaultCode) { }
public NuciApiSuccessResponse(string message) : base(message, DefaultCode) { }
public NuciApiSuccessResponse(string message, string code) : base(message, code) { }
```

### Factory properties (static)

| Property | Message | Code |
|----------|---------|------|
| `Default` | "Operation completed successfully." | "SUCCESS" |
| `Created` | "The new resource was successfully created." | "CREATED" |
| `Deleted` | "The resource was successfully deleted." | "DELETED" |
| `Fetched` | "The resource was successfully fetched." | "FETCHED" |
| `NotUpdated` | "The resource was not updated, as it already has the same content." | "NOT_UPDATED" |
| `Updated` | "The resource was successfully updated." | "UPDATED" |

### Factory methods

| Method | Description |
|--------|-------------|
| `FromMessage(string message)` | Creates instance with custom message, default code |

### Serialisation shape

```json
{
  "success": true,
  "message": "Operation completed successfully.",
  "code": "SUCCESS",
  "content": null
}
```

---

## NuciApiErrorResponse (sealed)

**Location:** `NuciAPI/Responses/NuciApiErrorResponse.cs`
**Namespace:** `NuciAPI.Responses`

### Purpose

Concrete error response. Sealed to prevent further inheritance.

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `IsSuccessful` | `bool` | Always `false` (override) |
| `Content` | — | Not present (no `Content` property) |

### Constructors

```csharp
public NuciApiErrorResponse() : base(DefaultMessage, DefaultCode) { }
public NuciApiErrorResponse(string message, string code) : base(message, code) { }
```

### Factory properties (static)

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

### Factory methods

| Method | Description |
|--------|-------------|
| `FromMessage(string message)` | Creates instance with custom message, default error code |

### Serialisation shape

```json
{
  "success": false,
  "message": "The requested resource was not found.",
  "code": "NOT_FOUND"
}
```

Note: No `content` property in JSON output.

---

## NuciApiContentResponse<TContent> (sealed)

**Location:** `NuciAPI/Responses/NuciApiContentResponse.cs`
**Namespace:** `NuciAPI.Responses`
**Constraint:** `where TContent : NuciApiResponseContent`

### Purpose

Generic successful response with strongly-typed payload content. Ensures content participates in HMAC.

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `IsSuccessful` | `bool` | Always `true` (override) |
| `Content` | `TContent` | Required typed payload |

### Constructors

```csharp
public NuciApiContentResponse(TContent content) : base(DefaultMessage, DefaultCode) { }
public NuciApiContentResponse(TContent content, string message) : base(message, DefaultCode) { }
[JsonConstructor]
public NuciApiContentResponse(TContent content, string message, string code) : base(message, code) { }
```

### Serialisation shape

```json
{
  "success": true,
  "content": { "property1": "value1", "property2": "value2" },
  "message": "Operation completed successfully.",
  "code": "SUCCESS"
}
```

### Deserialisation

The `[JsonConstructor]` enables round-trip deserialisation with typed content.

---

## NuciApiResponseContent (abstract)

**Location:** `NuciAPI/Responses/NuciApiResponseContent.cs`
**Namespace:** `NuciAPI.Responses`

### Purpose

Abstract base for response payload content. Enables HMAC participation via `[HmacOrder]` attributes on derived properties.

### Usage

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
```

---

## NuciApiResponseCodes

**Location:** `NuciAPI/Responses/NuciApiResponseCodes.cs`
**Namespace:** `NuciAPI.Responses`

### Structure

```csharp
public static class NuciApiResponseCodes
{
    public static class SuccessCodes { const string Default, Created, Deleted, Fetched, NotUpdated, Updated; }
    public static class ErrorCodes { const string Default, AlreadyExists, AlreadyProcessed, AuthenticationFailure, BadRequest, ClientClosedTheRequest, InternalServerError, InvalidRequest, NotFound, NotImplemented, ServiceUnavailable; }
}
```

All values are `const string` — compile-time constants.

---

## NuciApiResponseMessages

**Location:** `NuciAPI/Responses/NuciApiResponseMessages.cs`
**Namespace:** `NuciAPI.Responses`

### Structure

```csharp
public static class NuciApiResponseMessages
{
    public static class SuccessMessages { const string Default, Created, Deleted, Fetched, NotUpdated, Updated; }
    public static class ErrorMessages { const string Default, AlreadyExists, AlreadyProcessed, AuthenticationFailure, BadRequest, ClientClosedTheRequest, InternalServerError, InvalidRequest, NotFound, NotImplemented, ServiceUnavailable; }
}
```

All values are `const string` — compile-time constants.