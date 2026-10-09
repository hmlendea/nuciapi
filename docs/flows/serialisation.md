# Serialisation Flow

## Overview

This flow describes JSON serialisation and deserialisation behaviour for NuciAPI request/response types using `System.Text.Json`.

## Serialisation configuration

- **Library:** `System.Text.Json` (built-in)
- **Attributes used:** `JsonPropertyName`, `JsonIgnore`, `JsonConstructor`
- **No custom `JsonSerializerOptions` required** — default options work correctly
- **ImplicitUsings disabled** — consumers must `using System.Text.Json;`

## Request serialisation

### NuciApiRequest (and derived)

```csharp
public class CreateOrderRequest : NuciApiRequest
{
    [HmacOrder(1)]
    public string CustomerId { get; set; }

    [HmacOrder(2)]
    public decimal Total { get; set; }
}

var request = new CreateOrderRequest
{
    CustomerId = "CUST-001",
    Total = 149.99m
};
request.SignHMAC("secret");
```

**JSON output:**
```json
{
  "CustomerId": "CUST-001",
  "Total": 149.99
}
```

**Notes:**
- `HmacToken` excluded via `[JsonIgnore]`
- Property names use PascalCase (default `JsonNamingPolicy` = null)
- No `success`, `message`, `code` fields — requests don't inherit from `NuciApiResponse`

## Response serialisation

### NuciApiSuccessResponse (no content)

```csharp
var response = NuciApiSuccessResponse.Created;
response.SignHMAC("secret");
```

**JSON output:**
```json
{
  "success": true,
  "message": "The new resource was successfully created.",
  "code": "CREATED",
  "content": null
}
```

### NuciApiSuccessResponse (with content)

```csharp
var response = new NuciApiSuccessResponse
{
    Content = new OrderResponseContent { OrderId = "ORD-123", Total = 99.99m }
};
response.SignHMAC("secret");
```

**JSON output:**
```json
{
  "success": true,
  "message": "Operation completed successfully.",
  "code": "SUCCESS",
  "content": {
    "OrderId": "ORD-123",
    "Total": 99.99
  }
}
```

### NuciApiErrorResponse

```csharp
var response = NuciApiErrorResponse.NotFound;
response.SignHMAC("secret");
```

**JSON output:**
```json
{
  "success": false,
  "message": "The requested resource was not found.",
  "code": "NOT_FOUND"
}
```

**Notes:**
- No `content` property in error responses
- `HmacToken` excluded via `[JsonIgnore]`

### NuciApiContentResponse<TContent>

```csharp
var content = new OrderResponseContent { OrderId = "ORD-123", Total = 99.99m };
var response = new NuciApiContentResponse<OrderResponseContent>(content);
response.SignHMAC("secret");
```

**JSON output:**
```json
{
  "success": true,
  "content": {
    "OrderId": "ORD-123",
    "Total": 99.99
  },
  "message": "Operation completed successfully.",
  "code": "SUCCESS"
}
```

## Deserialisation

### NuciApiContentResponse<TContent> round-trip

The `[JsonConstructor]` enables proper deserialisation:

```csharp
string json = @"{
  ""success"": true,
  ""content"": { ""OrderId"": ""ORD-123"", ""Total"": 99.99 },
  ""message"": ""Operation completed successfully."",
  ""code"": ""SUCCESS""
}";

var response = JsonSerializer.Deserialize<NuciApiContentResponse<OrderResponseContent>>(json);
// response.Content.OrderId == "ORD-123"
// response.Content.Total == 99.99m
// response.IsSuccessful == true
// response.Message == "Operation completed successfully."
// response.Code == "SUCCESS"
```

### NuciApiSuccessResponse deserialisation

```csharp
string json = @"{
  ""success"": true,
  ""message"": ""Created"",
  ""code"": ""CREATED"",
  ""content"": null
}";

var response = JsonSerializer.Deserialize<NuciApiSuccessResponse>(json);
// response.IsSuccessful == true
// response.Message == "Created"
// response.Code == "CREATED"
// response.Content == null
```

### NuciApiErrorResponse deserialisation

```csharp
string json = @"{
  ""success"": false,
  ""message"": ""Not found"",
  ""code"": ""NOT_FOUND""
}";

var response = JsonSerializer.Deserialize<NuciApiErrorResponse>(json);
// response.IsSuccessful == false
// response.Message == "Not found"
// response.Code == "NOT_FOUND"
```

## Property naming

| C# property | JSON property | Attribute |
|-------------|---------------|-----------|
| `IsSuccessful` | `success` | `[JsonPropertyName("success")]` |
| `Message` | `message` | `[JsonPropertyName("message")]` |
| `Code` | `code` | `[JsonPropertyName("code")]` |
| `Content` | `content` | `[JsonPropertyName("content")]` |
| `HmacToken` | (excluded) | `[JsonIgnore]` |
| Derived properties | PascalCase | Default (no attribute) |

## HMAC vs Serialisation order

**HMAC computation order** (via `[HmacOrder]`):
1. Consumer properties (explicit `[HmacOrder]` or default 0)
2. `Code` (9999997)
3. `Message` (9999998)
4. `IsSuccessful` (9999999)

**JSON serialisation order** (reflection order):
1. `success` (`IsSuccessful`)
2. `message` (`Message`)
3. `code` (`Code`)
4. `content` (`Content`)
5. Derived properties (declaration order)

**Key difference:** HMAC includes metadata properties LAST; JSON serialises them FIRST. This is intentional — HMAC order ensures metadata is bound to the payload, while JSON order is human-readable.

## Tests

- `NuciApiSuccessResponseTests.GivenASuccessResponse_WhenSerialising_ThenTheMessageAndCodePropertiesAreAtTheRootLevel`
- `NuciApiErrorResponseTests.GivenAnErrorResponse_WhenSerialising_ThenTheContentPropertyIsAbsent`
- `NuciApiContentResponseTests.GivenContent_WhenSerialisingAResponse_ThenTheContentPropertiesAreSerialised`
- `NuciApiContentResponseTests.GivenContent_WhenDeserialisingAResponse_ThenTheContentPropertiesAreDeserialised`