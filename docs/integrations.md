# Integrations

## External system integrations

### NuciSecurity.HMAC

**Type:** NuGet package dependency
**Version:** 4.1.3 (pinned)
**Purpose:** HMAC encoding, validation, attributes
**Integration points:**
- `HmacEncoder.GenerateToken(object, string)` — called by `SignHMAC`
- `HmacValidator.IsTokenValid(string, object, string)` — called by `HasValidHMAC`
- `HmacValidator.Validate(string, object, string)` — called by `ValidateHMAC`
- `[HmacIgnore]` attribute — on `HmacToken` properties
- `[HmacOrder(int)]` attribute — on metadata and consumer properties

**Coupling:** Compile-time (attributes) + runtime (encoder/validator)

**Upgrade process:**
1. Update version in `NuciAPI.csproj`
2. Run full test suite
3. Verify HMAC compatibility (old tokens validate with new version)
4. Test with consumer applications

### System.Text.Json

**Type:** .NET built-in library
**Version:** Part of net10.0
**Purpose:** JSON serialisation/deserialisation
**Integration points:**
- `[JsonPropertyName]` — maps C# properties to JSON names
- `[JsonIgnore]` — excludes `HmacToken` from JSON
- `[JsonConstructor]` — enables `NuciApiContentResponse<T>` deserialisation
- Default `JsonSerializerOptions` — no custom configuration required

**Coupling:** Compile-time (attributes) only

### GitHub Actions

**Type:** CI/CD platform
**Workflows:**
- `.github/workflows/dotnet.yml` — Build + test on push/PR
- `.github/workflows/github-release.yml` — Pack + publish on release

**Integration:** Repository-level, not code-level

## Consumer integrations

### ASP.NET Core

```csharp
// Minimal API
app.MapPost("/orders", (CreateOrderRequest request) =>
{
    request.HmacToken = Request.Headers["X-HMAC-Token"];
    request.ValidateHMAC(secretKey);
    // ...
});

// Controller
[HttpPost]
public IActionResult Create([FromBody] CreateOrderRequest request)
{
    request.HmacToken = Request.Headers["X-HMAC-Token"];
    request.ValidateHMAC(secretKey);
    // ...
}
```

### HttpClient (client-side)

```csharp
var request = new CreateOrderRequest { ... };
request.SignHMAC(secretKey);

var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/orders")
{
    Content = JsonContent.Create(request)
};
httpRequest.Headers.Add("X-HMAC-Token", request.HmacToken);

var response = await httpClient.SendAsync(httpRequest);
var responseBody = await response.Content.ReadFromJsonAsync<NuciApiSuccessResponse>();
responseBody.HmacToken = response.Headers.GetValues("X-HMAC-Token").First();
responseBody.ValidateHMAC(secretKey);
```

### ASP.NET Core middleware

```csharp
public class HmacValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _secretKey;

    public HmacValidationMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _secretKey = config["Hmac:SecretKey"];
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method != "GET" && context.Request.ContentLength > 0)
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            var requestType = GetRequestType(context.Request.Path);
            var request = JsonSerializer.Deserialize(body, requestType) as NuciApiRequest;

            if (request != null && context.Request.Headers.TryGetValue("X-HMAC-Token", out var token))
            {
                request.HmacToken = token;
                request.ValidateHMAC(_secretKey);
                context.Items["ValidatedRequest"] = request;
            }
        }
        await _next(context);
    }
}
```

## No other integrations

NuciAPI does not integrate with:
- Databases (EF Core, Dapper, etc.)
- Message queues (RabbitMQ, Kafka, etc.)
- Service discovery (Consul, etcd, etc.)
- Monitoring (OpenTelemetry, Prometheus, etc.)
- Authentication (IdentityServer, JWT, etc.)
- Authorisation (policy-based, role-based, etc.)
- Caching (Redis, MemoryCache, etc.)
- Configuration (Consul, Vault, etc.)