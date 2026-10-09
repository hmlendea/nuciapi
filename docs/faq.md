# Frequently Asked Questions

## General

### What is NuciAPI?

NuciAPI is a .NET library for building consistent API contracts with strongly-typed request/response models and integrated HMAC signing/validation for payload integrity.

### What problem does it solve?

- Standardises API response shapes (success/error/content)
- Provides HMAC-based integrity verification for requests and responses
- Eliminates boilerplate for consistent error handling
- Enforces typed payloads for success responses

### Is it a web framework?

No. It's a library for DTOs and contracts. You use it with ASP.NET Core, Minimal APIs, or any HTTP framework.

## HMAC

### Why HMAC instead of JWT?

HMAC is simpler for payload integrity — no token parsing, no claims, no expiration. It verifies the request/response body hasn't been tampered with.

### Does it provide encryption?

No. HMAC provides integrity and authenticity, not confidentiality. Use TLS for encryption.

### Can I use it for authentication?

It can be part of authentication (verifying request came from holder of secret key), but it's not a complete auth system. Combine with other mechanisms.

### What algorithm is used?

HMAC-SHA256 via `NuciSecurity.HMAC`.

### How do I rotate keys?

Implement key versioning in your application (e.g., `X-HMAC-Key-Version` header). NuciAPI doesn't manage keys.

## Usage

### How do I create a custom request?

```csharp
public class MyRequest : NuciApiRequest
{
    [HmacOrder(1)]
    public string MyProperty { get; set; }
}
```

### How do I create a response with typed content?

```csharp
public class MyContent : NuciApiResponseContent
{
    [HmacOrder(1)]
    public string Data { get; set; }
}

var response = new NuciApiContentResponse<MyContent>(new MyContent { Data = "value" });
```

### Can I use it without HMAC?

Yes — just don't call `SignHMAC`/`ValidateHMAC`. The base classes work as plain DTOs.

### How do I handle validation errors?

Catch `HmacValidationException` and return 401/400:

```csharp
try { request.ValidateHMAC(key); }
catch (HmacValidationException) { return Unauthorized(); }
```

## Serialisation

### Why is `content` null in success responses?

`NuciApiSuccessResponse.Content` defaults to null. Use `NuciApiContentResponse<T>` for required content.

### Why no `content` in error responses?

By design — errors don't carry payload content. Use `message` and `code` for error details.

### Can I customise JSON property names?

Yes, use `[JsonPropertyName("custom_name")]` on your derived properties. Base class properties use fixed names (`success`, `message`, `code`, `content`).

## Testing

### How do I test my DTOs?

Inherit from base classes in tests, use the same HMAC methods. See `NuciAPI.UnitTests.Helpers` for patterns.

### Are there integration tests?

No — only unit tests. Consumers should add integration tests for their HTTP pipeline.

## Versioning

### What's the versioning scheme?

Semantic Versioning (Major.Minor.Patch). Current: 3.6.1.

### Will HMAC tokens work across versions?

Only if `NuciSecurity.HMAC` and property order are compatible. Test before upgrading.

## Performance

### Is it fast?

Yes — HMAC signing/validation takes microseconds. No allocations beyond the token string.

### Any caching?

No. Each call computes HMAC fresh. For high throughput, consider caching signed responses if immutable.

## Compatibility

### What .NET versions?

.NET 10.0 only (net10.0).

### Can I use it with .NET 8/9?

Not officially. You could multi-target by modifying the csproj, but not tested.

### Does it work with AOT/Native AOT?

Should work — no reflection emit, only reflection for HMAC (which `NuciSecurity.HMAC` handles). Not tested.

## Licensing

### What license?

GPL-3.0-or-later. See [LICENSE](../LICENSE).

### Can I use it in commercial projects?

Yes, but GPL requires your application to also be GPL-compatible if distributed. Consult legal counsel.