# Error Handling

## Error taxonomy

| Category | Type | Source | Handling |
|----------|------|--------|----------|
| HMAC validation failure | `HmacValidationException` | `NuciSecurity.HMAC` | Catch and return 401/400 |
| Invalid secret key | `ArgumentException` | `HmacEncoder`/`HmacValidator` | Configuration error (500) |
| Serialisation failure | `JsonException` | `System.Text.Json` | Catch and return 400 |
| Null reference | `NullReferenceException` | Consumer code | Defensive coding |

## HMAC validation errors

### HmacValidationException

Thrown by `HmacValidator.Validate` when:
- `HmacToken` is null or empty
- `HmacToken` is not valid Base64
- Computed HMAC does not match provided token (constant-time comparison)

**Consumer responsibility:** Catch and map to appropriate HTTP response.

```csharp
try
{
    request.ValidateHMAC(secretKey);
}
catch (HmacValidationException)
{
    return Results.Unauthorized(); // or 400
}
```

### ArgumentException

Thrown by `HmacEncoder.GenerateToken` or `HmacValidator` when `secretKey` is null or empty.

**Consumer responsibility:** Validate configuration at startup; treat as 500 if occurs at runtime.

## Response-based errors

NuciAPI uses response objects for application-level errors, not exceptions:

```csharp
// Not found
return NuciApiErrorResponse.NotFound;

// Bad request
return NuciApiErrorResponse.BadRequest;

// Custom error
return NuciApiErrorResponse.FromMessage("Validation failed");
```

**Advantages:**
- No exception overhead for expected errors
- Consistent serialisation shape
- HMAC covers error responses too
- Type-safe error handling

## Serialisation errors

`System.Text.Json.JsonException` can occur during:
- Deserialisation of malformed JSON
- Type mismatch (e.g., string into decimal)

**Handling:**
```csharp
try
{
    var request = JsonSerializer.Deserialize<CreateOrderRequest>(json);
}
catch (JsonException)
{
    return NuciApiErrorResponse.BadRequest;
}
```

## Null safety

- `HmacToken` is nullable (`string?` effectively) — check before validation
- `Content` on `NuciApiSuccessResponse` is virtual and defaults to null
- `Content` on `NuciApiContentResponse<T>` is non-null (constructor requires it)
- Derived request/response properties — consumer responsibility

## Error handling patterns

### Pattern 1: Middleware validation (ASP.NET Core)

```csharp
app.Use(async (context, next) =>
{
    var request = await DeserializeRequest(context);
    request.HmacToken = GetHmacToken(context);
    
    try
    {
        request.ValidateHMAC(secretKey);
    }
    catch (HmacValidationException)
    {
        context.Response.StatusCode = 401;
        return;
    }
    
    await next(context);
});
```

### Pattern 2: Filter/attribute validation

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class ValidateHmacAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.Values.FirstOrDefault() is NuciApiRequest request)
        {
            request.HmacToken = GetToken(context);
            request.ValidateHMAC(secretKey);
        }
    }
}
```

### Pattern 3: Result-based (Minimal APIs)

```csharp
app.MapPost("/orders", (CreateOrderRequest request) =>
{
    if (!request.HasValidHMAC(secretKey))
        return Results.Unauthorized();
    
    // Process...
    return Results.Ok(NuciApiSuccessResponse.Created);
});
```

## Testing error scenarios

- `NuciApiRequestTests` — Validates HMAC token generation
- `NuciApiResponseTests` — Validates response HMAC
- No tests for `HmacValidationException` — depends on `NuciSecurity.HMAC` behaviour
- Consumers should test their error handling paths