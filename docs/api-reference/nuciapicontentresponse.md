# NuciApiContentResponse<TContent>

**Namespace:** `NuciAPI.Responses`
**Assembly:** `NuciAPI.dll`
**Base class:** `NuciApiResponse`
**Sealed:** Yes
**Constraint:** `where TContent : NuciApiResponseContent`

## Purpose

Generic successful response with strongly-typed payload content. Ensures content participates in HMAC.

## Syntax

```csharp
public sealed class NuciApiContentResponse<TContent> : NuciApiResponse
    where TContent : NuciApiResponseContent
{
    public override bool IsSuccessful => true;

    public TContent Content { get; set; }

    public NuciApiContentResponse(TContent content);
    public NuciApiContentResponse(TContent content, string message);
    [JsonConstructor]
    public NuciApiContentResponse(TContent content, string message, string code);
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
public TContent Content { get; set; }
```

- Required typed payload content.
- Must be non-null at construction (no default).

## Constructors

```csharp
public NuciApiContentResponse(TContent content)
    : base(NuciApiResponseMessages.SuccessMessages.Default, NuciApiResponseCodes.SuccessCodes.Default)
{
    Content = content;
}

public NuciApiContentResponse(TContent content, string message)
    : base(message, NuciApiResponseCodes.SuccessCodes.Default)
{
    Content = content;
}

[JsonConstructor]
public NuciApiContentResponse(TContent content, string message, string code)
    : base(message, code)
{
    Content = content;
}
```

## Serialisation example

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

The `[JsonConstructor]` attribute enables round-trip deserialisation:

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
```

## Usage example

```csharp
using NuciAPI.Responses;

public class OrderResponseContent : NuciApiResponseContent
{
    [HmacOrder(1)]
    public string OrderId { get; set; }

    [HmacOrder(2)]
    public decimal Total { get; set; }
}

var content = new OrderResponseContent { OrderId = "ORD-123", Total = 99.99m };
var response = new NuciApiContentResponse<OrderResponseContent>(content);
response.SignHMAC("secret");
```

## Tests

- `NuciApiContentResponseTests.GivenContent_WhenCreatingAResponse_ThenTheContentIsRetained`
- `NuciApiContentResponseTests.GivenContent_WhenCreatingAResponse_ThenTheDefaultMetadataIsUsed`
- `NuciApiContentResponseTests.GivenContent_WhenSerialisingAResponse_ThenTheContentPropertiesAreSerialised`
- `NuciApiContentResponseTests.GivenContent_WhenDeserialisingAResponse_ThenTheContentPropertiesAreDeserialised`