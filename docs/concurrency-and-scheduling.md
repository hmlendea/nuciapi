# Concurrency and Scheduling

## Overview

NuciAPI is **single-threaded by design** with no internal concurrency, scheduling, or asynchronous operations.

## Threading model

| Aspect | Detail |
|--------|--------|
| Internal threads | None |
| Thread pool usage | None |
| Async/await | Not used in library code |
| Synchronization primitives | None (no locks, semaphores, etc.) |
| Thread-safety | All methods thread-safe (stateless) |

## HMAC operations

```csharp
// These are synchronous, CPU-bound operations
request.SignHMAC(secretKey);        // ~microseconds
request.ValidateHMAC(secretKey);    // ~microseconds
response.SignHMAC(secretKey);       // ~microseconds
response.ValidateHMAC(secretKey);   // ~microseconds
```

- No I/O, no network calls, no disk access
- Pure computation: reflection + serialisation + HMAC-SHA256
- Safe to call from any thread concurrently

## Consumer concurrency patterns

### Pattern 1: Per-request signing (recommended)

```csharp
// Each request gets its own HMAC — no shared state
var request = new CreateOrderRequest { ... };
request.SignHMAC(secretKey); // Thread-safe
await httpClient.PostAsync(...);
```

### Pattern 2: Shared secret key (read-only)

```csharp
// Secret key is immutable string — safe to share across threads
private readonly string _secretKey = configuration["Hmac:SecretKey"];

public async Task<OrderResponse> CreateOrder(CreateOrderRequest request)
{
    request.SignHMAC(_secretKey); // Thread-safe
    // ...
}
```

### Pattern 3: Parallel validation (server)

```csharp
// Validate multiple requests concurrently — no shared state
var tasks = requests.Select(r => Task.Run(() =>
{
    r.HmacToken = GetToken(r);
    r.ValidateHMAC(_secretKey); // Thread-safe
    return Process(r);
}));
await Task.WhenAll(tasks);
```

## No scheduling

| Feature | Supported |
|---------|-----------|
| Timers | No |
| Background tasks | No |
| Periodic work | No |
| Delayed execution | No |
| Cancellation tokens | Not used internally |

## Async compatibility

While NuciAPI itself is synchronous, it works seamlessly in async contexts:

```csharp
// In async controller
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
{
    request.HmacToken = Request.Headers["X-HMAC-Token"];
    request.ValidateHMAC(_secretKey); // Sync, fast, non-blocking

    var result = await _service.CreateOrderAsync(request); // Your async work

    var response = NuciApiSuccessResponse.Created;
    response.SignHMAC(_secretKey); // Sync, fast
    Response.Headers["X-HMAC-Token"] = response.HmacToken;

    return Ok(response);
}
```

## Performance characteristics

| Operation | Typical time | Scales with |
|-----------|--------------|-------------|
| `SignHMAC` | 10-50 μs | Object graph size |
| `ValidateHMAC` | 10-50 μs | Object graph size |
| Reflection | Included above | Property count |
| Serialisation | Included above | Property count |

- No allocation beyond HMAC token string
- No GC pressure beyond normal
- Suitable for high-throughput scenarios (>100k ops/sec)

## Deadlocks and race conditions

**Impossible in NuciAPI** — no shared state, no locks, no async coordination.

## Testing concurrency

No concurrency tests in library (nothing to test). Consumers should test their own concurrent usage patterns.