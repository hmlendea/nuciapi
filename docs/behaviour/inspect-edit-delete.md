# Inspect, Edit, Delete Patterns

## Inspecting requests and responses

### Request inspection

```csharp
// After deserialization, before validation
var request = JsonSerializer.Deserialize<MyRequest>(json);

// Inspect properties
Console.WriteLine($"Request type: {request.GetType().Name}");
Console.WriteLine($"HMAC token present: {!string.IsNullOrEmpty(request.HmacToken)}");
Console.WriteLine($"HMAC token length: {request.HmacToken?.Length ?? 0}");

// Inspect all properties (including HMAC-excluded)
foreach (var prop in request.GetType().GetProperties())
{
    var value = prop.GetValue(request);
    var ignored = prop.GetCustomAttribute<HmacIgnoreAttribute>() != null;
    var order = prop.GetCustomAttribute<HmacOrderAttribute>()?.Order;
    Console.WriteLine($"  {prop.Name}: {value} (HMAC ignored: {ignored}, order: {order})");
}
```

### Response inspection

```csharp
// After deserialization
var response = JsonSerializer.Deserialize<MyResponse>(json);

// Check success
if (response.IsSuccessful)
{
    Console.WriteLine($"Success: {response.Code} - {response.Message}");
    
    // If typed content response
    if (response is NuciApiContentResponse<NuciApiResponseContent> typed)
    {
        Console.WriteLine($"Content type: {typed.Content?.GetType().Name}");
        Console.WriteLine($"Content: {JsonSerializer.Serialize(typed.Content)}");
    }
}
else
{
    Console.WriteLine($"Error: {response.Code} - {response.Message}");
}

// HMAC token
Console.WriteLine($"HMAC token present: {!string.IsNullOrEmpty(response.HmacToken)}");
```

### HMAC token inspection

```csharp
// Decode and inspect HMAC token (base64)
var tokenBytes = Convert.FromBase64String(response.HmacToken);
Console.WriteLine($"HMAC token bytes: {tokenBytes.Length} (expected 32 for SHA256)");

// Note: Token is the raw HMAC-SHA256 hash, not further encoded
// Cannot be "decoded" to reveal input - it's a one-way hash
```

## Editing patterns

### Modifying a request before signing

```csharp
var request = new MyRequest
{
    Id = 123,
    Data = "original"
};

// Edit properties
request.Data = "modified";
request.Timestamp = DateTime.UtcNow;

// Sign AFTER all edits
request.SignHMAC(secretKey);

// Any edit after signing invalidates HMAC
request.Data = "tampered"; // HasValidHMAC will now return false
```

### Modifying a response before signing

```csharp
var response = new MyContentResponse
{
    Content = new MyContent { Value = 100 }
};

// Edit content
response.Content.Value = 200;
response.Message = "Updated value";

// Sign AFTER all edits
response.SignHMAC(secretKey);
```

### Using factory properties as starting point

```csharp
// Start from factory, then customise
var response = NuciApiSuccessResponse.Created;
response.Message = "Order created with ID 456";
response.Code = "ORDER_CREATED"; // Override code if needed

// Or for error
var error = NuciApiErrorResponse.NotFound();
error.Message = "Product SKU-001 not found in warehouse 3";

// Then sign
response.SignHMAC(secretKey);
error.SignHMAC(secretKey);
```

### Building responses programmatically

```csharp
// Success with content
var response = new MyContentResponse
{
    Content = BuildContent(),
    Message = "Operation completed",
    Code = "CUSTOM_SUCCESS"
};

// Error with context
var error = new NuciApiErrorResponse
{
    Message = "Validation failed",
    Code = "VALIDATION_ERROR"
};

// Helper method
private MyContent BuildContent()
{
    return new MyContent
    {
        Items = _service.GetItems(),
        Total = _service.GetTotal()
    };
}
```

## Deleting/clearing patterns

### Clearing HMAC token (for re-signing)

```csharp
// No direct ClearHMAC method - create new instance or set to null
request.HmacToken = null; // Requires accessible setter

// Better: create fresh instance
var newRequest = new MyRequest
{
    Id = request.Id,
    Data = request.Data
};
newRequest.SignHMAC(secretKey);
```

### Removing content from response

```csharp
// For NuciApiContentResponse<T>, Content is required (no null)
// Use NuciApiSuccessResponse instead for no-content success
var noContentResponse = NuciApiSuccessResponse.Default;
noContentResponse.SignHMAC(secretKey);

// Or create minimal content
public sealed class EmptyContent : NuciApiResponseContent { }

var response = new NuciApiContentResponse<EmptyContent>
{
    Content = new EmptyContent()
};
response.SignHMAC(secretKey);
```

### Discarding invalid requests

```csharp
// In middleware/handler
if (!request.HasValidHMAC(secretKey))
{
    // Log and discard
    _logger.LogWarning("Invalid HMAC from {IP}", context.Connection.RemoteIpAddress);
    return NuciApiErrorResponse.AuthenticationFailure();
}

// If request is valid but business logic fails
if (!await _service.CanProcess(request))
{
    return NuciApiErrorResponse.BadRequest();
}
```

## Validation patterns

### Pre-sign validation

```csharp
public bool TryPrepareRequest(MyRequest request, byte[] key, out string error)
{
    // Business validation before signing
    if (request.Id <= 0)
    {
        error = "Id must be positive";
        return false;
    }
    
    if (string.IsNullOrWhiteSpace(request.Data))
    {
        error = "Data is required";
        return false;
    }
    
    request.SignHMAC(key);
    error = null;
    return true;
}
```

### Post-deserialization validation

```csharp
public ValidationResult ValidateRequest(MyRequest request, byte[] key)
{
    var result = new ValidationResult();
    
    // HMAC validation
    if (!request.HasValidHMAC(key))
    {
        result.AddError("HMAC", "Invalid or missing signature");
        return result;
    }
    
    // Business validation
    if (request.Id <= 0)
        result.AddError("Id", "Must be positive");
    
    if (string.IsNullOrWhiteSpace(request.Data))
        result.AddError("Data", "Required");
    
    return result;
}

public class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<ValidationError> Errors { get; } = new();
    
    public void AddError(string field, string message)
        => Errors.Add(new ValidationError { Field = field, Message = message });
}

public class ValidationError
{
    public string Field { get; set; }
    public string Message { get; set; }
}
```

## Debugging patterns

### Logging request/response (without sensitive data)

```csharp
public void LogRequest(NuciApiRequest request, bool includeHMAC = false)
{
    var log = new
    {
        Type = request.GetType().Name,
        HasHMAC = !string.IsNullOrEmpty(request.HmacToken),
        HMAC = includeHMAC ? request.HmacToken : "[REDACTED]",
        Properties = request.GetType()
            .GetProperties()
            .Where(p => p.Name != nameof(NuciApiRequest.HmacToken))
            .ToDictionary(p => p.Name, p => p.GetValue(request))
    };
    
    _logger.LogInformation("Request: {@Request}", log);
}
```

### Correlation IDs

```csharp
// Add to your base request/response
public abstract class CorrelatedRequest : NuciApiRequest
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
}

public abstract class CorrelatedResponse : NuciApiResponse
{
    public string CorrelationId { get; set; }
}

// Usage
var request = new MyRequest { CorrelationId = "req-123" };
request.SignHMAC(key);

// Server copies correlation ID
var response = new MyResponse { CorrelationId = request.CorrelationId };
response.SignHMAC(key);
```

### Request/response comparison

```csharp
public bool AreEquivalent(NuciApiRequest a, NuciApiRequest b, bool ignoreHMAC = true)
{
    var props = a.GetType().GetProperties()
        .Where(p => !ignoreHMAC || p.Name != nameof(NuciApiRequest.HmacToken));
    
    foreach (var prop in props)
    {
        var va = prop.GetValue(a);
        var vb = prop.GetValue(b);
        if (!Equals(va, vb)) return false;
    }
    return true;
}
```

## Testing patterns

### Test request creation and signing

```csharp
[Test]
public void Request_CanBeCreated_Signed_AndValidated()
{
    var request = new MyRequest { Id = 42, Data = "test" };
    var key = TestKeys.ValidKey;
    
    request.SignHMAC(key);
    
    Assert.That(request.HmacToken, Is.Not.Empty);
    Assert.That(request.HasValidHMAC(key), Is.True);
}

[Test]
public void Request_Tampering_InvalidatesHMAC()
{
    var request = new MyRequest { Id = 42, Data = "test" };
    var key = TestKeys.ValidKey;
    
    request.SignHMAC(key);
    var originalToken = request.HmacToken;
    
    request.Data = "tampered";
    
    Assert.That(request.HasValidHMAC(key), Is.False);
    Assert.That(request.HmacToken, Is.EqualTo(originalToken)); // Token unchanged
}
```

### Test response factories

```csharp
[Test]
public void SuccessResponse_Factories_HaveExpectedValues()
{
    Assert.Multiple(() =>
    {
        Assert.That(NuciApiSuccessResponse.Default.Code, Is.EqualTo("SUCCESS"));
        Assert.That(NuciApiSuccessResponse.Created.Code, Is.EqualTo("CREATED"));
        Assert.That(NuciApiSuccessResponse.Deleted.Code, Is.EqualTo("DELETED"));
        Assert.That(NuciApiSuccessResponse.Fetched.Code, Is.EqualTo("FETCHED"));
        Assert.That(NuciApiSuccessResponse.NotUpdated.Code, Is.EqualTo("NOT_UPDATED"));
        Assert.That(NuciApiSuccessResponse.Updated.Code, Is.EqualTo("UPDATED"));
    });
}

[Test]
public void ErrorResponse_Factories_HaveExpectedValues()
{
    Assert.Multiple(() =>
    {
        Assert.That(NuciApiErrorResponse.NotFound().Code, Is.EqualTo("NOT_FOUND"));
        Assert.That(NuciApiErrorResponse.BadRequest().Code, Is.EqualTo("BAD_REQUEST"));
        Assert.That(NuciApiErrorResponse.AuthenticationFailure().Code, Is.EqualTo("AUTHENTICATION_FAILURE"));
        // ... etc
    });
}
```

### Test content round-trip

```csharp
[Test]
public void ContentResponse_SerialisesAndDeserialisesCorrectly()
{
    var original = new MyContentResponse
    {
        Content = new MyContent { Value = 123, Name = "test" }
    };
    
    var json = JsonSerializer.Serialize(original);
    var deserialized = JsonSerializer.Deserialize<MyContentResponse>(json);
    
    Assert.That(deserialized.Content.Value, Is.EqualTo(123));
    Assert.That(deserialized.Content.Name, Is.EqualTo("test"));
    Assert.That(deserialized.IsSuccessful, Is.True);
}
```