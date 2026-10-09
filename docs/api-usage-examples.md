# API Usage Examples

## Basic request/response cycle

### Client sends signed request

```csharp
using NuciAPI.Requests;
using NuciAPI.Responses;
using System.Text.Json;

public sealed class CreateOrderRequest : NuciApiRequest
{
    public int CustomerId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public sealed class OrderItem : NuciApiResponseContent
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}

public sealed class CreateOrderResponse : NuciApiContentResponse<OrderResult>
{
}

public sealed class OrderResult : NuciApiResponseContent
{
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Total { get; set; }
}

// Client code
var request = new CreateOrderRequest
{
    CustomerId = 123,
    Items = new List<OrderItem>
    {
        new() { ProductId = "SKU-001", Quantity = 2 },
        new() { ProductId = "SKU-002", Quantity = 1 }
    }
};

var secretKey = Convert.FromBase64String(Environment.GetEnvironmentVariable("HMAC_SECRET"));
request.SignHMAC(secretKey);

var json = JsonSerializer.Serialize(request);
var httpResponse = await httpClient.PostAsync("/api/orders", new StringContent(json, Encoding.UTF8, "application/json"));
```

### Server validates and responds

```csharp
[HttpPost("/api/orders")]
public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
{
    var secretKey = Convert.FromBase64String(_config["HmacSecret"]);
    
    if (!request.HasValidHMAC(secretKey))
    {
        return Unauthorized(NuciApiErrorResponse.AuthenticationFailure());
    }
    
    var order = await _orderService.CreateAsync(request.CustomerId, request.Items);
    
    var response = new CreateOrderResponse
    {
        Content = new OrderResult
        {
            OrderId = order.Id,
            CreatedAt = order.CreatedAt,
            Total = order.Total
        }
    };
    
    response.SignHMAC(secretKey);
    return Ok(response);
}
```

### Client validates response

```csharp
var responseJson = await httpResponse.Content.ReadAsStringAsync();
var response = JsonSerializer.Deserialize<CreateOrderResponse>(responseJson);

var secretKey = Convert.FromBase64String(Environment.GetEnvironmentVariable("HMAC_SECRET"));

if (!response.HasValidHMAC(secretKey))
{
    throw new InvalidOperationException("Response signature validation failed");
}

if (!response.IsSuccessful)
{
    throw new ApiException(response.Code, response.Message);
}

Console.WriteLine($"Order {response.Content.OrderId} created, total: {response.Content.Total}");
```

## Using factory properties

### Success scenarios

```csharp
// Simple acknowledgment
return NuciApiSuccessResponse.Default;

// Resource created
return NuciApiSuccessResponse.Created;

// Resource deleted
return NuciApiSuccessResponse.Deleted;

// Data fetched
return NuciApiSuccessResponse.Fetched;

// Update resulted in no change
return NuciApiSuccessResponse.NotUpdated;

// Resource updated
return NuciApiSuccessResponse.Updated;

// Custom success
return NuciApiSuccessResponse.FromMessage("Batch processed", "BATCH_OK");
```

### Error scenarios

```csharp
// Generic error
return NuciApiErrorResponse.Default;

// Resource not found
return NuciApiErrorResponse.NotFound();

// Invalid input
return NuciApiErrorResponse.BadRequest();

// Authentication failed
return NuciApiErrorResponse.AuthenticationFailure();

// Duplicate resource
return NuciApiErrorResponse.AlreadyExists();

// Idempotent request already processed
return NuciApiErrorResponse.AlreadyProcessed();

// Server error
return NuciApiErrorResponse.InternalServerError();

// Malformed request
return NuciApiErrorResponse.InvalidRequest();

// Client disconnected
return NuciApiErrorResponse.ClientClosedTheRequest();

// Custom error
return NuciApiErrorResponse.FromMessage("Rate limit exceeded", "RATE_LIMITED");
```

## Typed content responses

### Generic success with payload

```csharp
public sealed class ProductContent : NuciApiResponseContent
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public sealed class GetProductResponse : NuciApiContentResponse<ProductContent>
{
}

// Usage
var response = new GetProductResponse
{
    Content = new ProductContent
    {
        Id = 42,
        Name = "Widget",
        Price = 19.99m,
        Stock = 100
    }
};

response.SignHMAC(secretKey);
return Ok(response);
```

### Collection response

```csharp
public sealed class ProductListContent : NuciApiResponseContent
{
    public List<ProductContent> Products { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public sealed class ListProductsResponse : NuciApiContentResponse<ProductListContent>
{
}

// Usage
var response = new ListProductsResponse
{
    Content = new ProductListContent
    {
        Products = products,
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize
    }
};
```

## HMAC with custom properties

### Ignoring properties from HMAC

```csharp
using NuciSecurity.HMAC;

public sealed class SensitiveRequest : NuciApiRequest
{
    public string PublicData { get; set; }
    
    [HmacIgnore]
    public string InternalNote { get; set; } // Excluded from signature
    
    [HmacIgnore]
    public DateTime RequestTimestamp { get; set; } // Excluded from signature
}
```

### Controlling property order in HMAC

```csharp
using NuciSecurity.HMAC;

public sealed class OrderedRequest : NuciApiRequest
{
    [HmacOrder(1)]
    public int PrimaryId { get; set; }
    
    [HmacOrder(2)]
    public string SecondaryKey { get; set; }
    
    [HmacOrder(3)]
    public DateTime Timestamp { get; set; }
}
```

## Error handling patterns

### TryValidate pattern

```csharp
public static class HmacExtensions
{
    public static bool TryValidateHMAC(this NuciApiRequest request, byte[] key, out NuciApiErrorResponse error)
    {
        if (!request.HasValidHMAC(key))
        {
            error = NuciApiErrorResponse.AuthenticationFailure();
            return false;
        }
        error = null;
        return true;
    }
    
    public static bool TryValidateHMAC(this NuciApiResponse response, byte[] key, out NuciApiErrorResponse error)
    {
        if (!response.HasValidHMAC(key))
        {
            error = NuciApiErrorResponse.AuthenticationFailure();
            return false;
        }
        error = null;
        return true;
    }
}

// Usage
if (!request.TryValidateHMAC(secretKey, out var error))
{
    return Unauthorized(error);
}
```

### Middleware pattern (ASP.NET Core)

```csharp
public class HmacValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly byte[] _key;
    
    public HmacValidationMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _key = Convert.FromBase64String(config["HmacSecret"]);
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method != HttpMethods.Get && 
            context.Request.ContentLength > 0)
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;
            
            var requestType = GetRequestType(context.Request.Path);
            var request = JsonSerializer.Deserialize(body, requestType) as NuciApiRequest;
            
            if (request != null && !request.HasValidHMAC(_key))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                var error = NuciApiErrorResponse.AuthenticationFailure();
                await context.Response.WriteAsJsonAsync(error);
                return;
            }
        }
        
        await _next(context);
    }
}
```

## Testing examples

### Unit test for request signing

```csharp
[Test]
public void CreateOrderRequest_SignAndValidate_RoundTrip()
{
    var request = new CreateOrderRequest
    {
        CustomerId = 123,
        Items = new List<OrderItem>
        {
            new() { ProductId = "SKU-001", Quantity = 2 }
        }
    };
    
    var key = Convert.FromBase64String("test-secret-key-base64-encoded==");
    
    request.SignHMAC(key);
    
    Assert.That(request.HmacToken, Is.Not.Null.And.Not.Empty);
    Assert.That(request.HasValidHMAC(key), Is.True);
    
    // Tamper
    request.CustomerId = 999;
    Assert.That(request.HasValidHMAC(key), Is.False);
}
```

### Unit test for response factory

```csharp
[Test]
public void NuciApiSuccessResponse_FactoryProperties_HaveCorrectCodes()
{
    Assert.That(NuciApiSuccessResponse.Default.Code, Is.EqualTo("SUCCESS"));
    Assert.That(NuciApiSuccessResponse.Created.Code, Is.EqualTo("CREATED"));
    Assert.That(NuciApiSuccessResponse.Deleted.Code, Is.EqualTo("DELETED"));
    Assert.That(NuciApiSuccessResponse.Fetched.Code, Is.EqualTo("FETCHED"));
    Assert.That(NuciApiSuccessResponse.NotUpdated.Code, Is.EqualTo("NOT_UPDATED"));
    Assert.That(NuciApiSuccessResponse.Updated.Code, Is.EqualTo("UPDATED"));
}
```

## Serialization examples

### JSON output (request)

```json
{
  "customerId": 123,
  "items": [
    { "productId": "SKU-001", "quantity": 2 },
    { "productId": "SKU-002", "quantity": 1 }
  ],
  "hmacToken": "base64-encoded-hmac-signature"
}
```

### JSON output (success response)

```json
{
  "isSuccessful": true,
  "message": "Resource created successfully",
  "code": "CREATED",
  "hmacToken": "base64-encoded-hmac-signature",
  "content": {
    "orderId": 456,
    "createdAt": "2024-01-15T10:30:00Z",
    "total": 59.97
  }
}
```

### JSON output (error response)

```json
{
  "isSuccessful": false,
  "message": "Resource not found",
  "code": "NOT_FOUND",
  "hmacToken": "base64-encoded-hmac-signature"
}
```

## Key management

### Environment variable

```bash
export HMAC_SECRET="base64-encoded-32-byte-key"
```

### Configuration (appsettings.json)

```json
{
  "Hmac": {
    "SecretKey": "base64-encoded-32-byte-key",
    "KeyId": "v1"
  }
}
```

### Key rotation

```csharp
public class HmacKeyProvider
{
    private readonly Dictionary<string, byte[]> _keys = new();
    
    public void AddKey(string keyId, string base64Key)
    {
        _keys[keyId] = Convert.FromBase64String(base64Key);
    }
    
    public byte[] GetKey(string keyId = "current")
    {
        return _keys[keyId];
    }
    
    public bool TryValidateWithAnyKey(NuciApiRequest request, out string keyId)
    {
        foreach (var kvp in _keys)
        {
            if (request.HasValidHMAC(kvp.Value))
            {
                keyId = kvp.Key;
                return true;
            }
        }
        keyId = null;
        return false;
    }
}
```