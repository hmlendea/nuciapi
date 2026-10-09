# Troubleshooting

## Common issues

### HMAC validation fails

**Symptoms:** `HmacValidationException` thrown on `ValidateHMAC`

**Causes:**
1. **Secret key mismatch** — Client and server use different keys
2. **Property order changed** — `[HmacOrder]` values differ between client/server
3. **Property added/removed** — DTO structure differs
4. **Token corrupted in transit** — Encoding issue (Base64)
5. **HmacToken not set** — Forgot to assign received token before validation

**Debugging:**
```csharp
// Check token received
logger.LogDebug("Received HMAC token: {Token}", receivedToken);

// Verify secret key
logger.LogDebug("Secret key length: {Length}", secretKey?.Length);

// Compare object graphs
logger.LogDebug("Request: {Request}", JsonSerializer.Serialize(request));
```

### HmacToken is null after SignHMAC

**Symptoms:** `request.HmacToken` remains null

**Causes:**
1. `secretKey` is null or empty — throws `ArgumentException` (check logs)
2. `HmacEncoder.GenerateToken` returns null — unlikely, check NuciSecurity.HMAC

**Fix:** Ensure non-empty secret key passed to `SignHMAC`.

### Serialisation missing properties

**Symptoms:** JSON output doesn't include expected properties

**Causes:**
1. Property not public — must be public instance property
2. `[JsonIgnore]` applied — check attributes
3. Null value with `DefaultIgnoreCondition` — check serializer options

**Fix:** Ensure public get/set, no `[JsonIgnore]`, configure serializer if needed.

### Content property missing in error response

**Symptoms:** Expecting `content` in error JSON but not present

**Cause:** `NuciApiErrorResponse` has no `Content` property by design.

**Fix:** Use `NuciApiSuccessResponse` or `NuciApiContentResponse<T>` for responses with content.

### Deserialisation fails for NuciApiContentResponse<T>

**Symptoms:** `JsonException` or null content after deserialisation

**Causes:**
1. JSON `content` property doesn't match `TContent` structure
2. Missing `[JsonConstructor]` — but it's present on the 3-param constructor
3. `TContent` not inheriting from `NuciApiResponseContent`

**Fix:** Verify JSON structure matches `TContent` exactly; ensure generic constraint satisfied.

### Build fails: NuciSecurity.HMAC not found

**Symptoms:** `NU1101` or `CS0246` errors

**Fix:** Run `dotnet restore` to restore NuGet packages.

### Tests fail after HMAC changes

**Symptoms:** `NuciApiRequestTests` or `NuciApiResponseTests` fail

**Causes:**
1. Changed `[HmacOrder]` values
2. Added/removed properties in test helpers
3. Upgraded `NuciSecurity.HMAC` with breaking changes

**Fix:** Update test expectations or revert HMAC-affecting changes.

## Debugging tips

### Inspect HMAC token generation

```csharp
var request = new CreateOrderRequest { CustomerId = "CUST-001", Total = 149.99m };
request.SignHMAC("secret");
Console.WriteLine($"Token: {request.HmacToken}");
// Decode to verify (Base64)
var bytes = Convert.FromBase64String(request.HmacToken);
Console.WriteLine($"Token bytes: {bytes.Length}"); // Should be 32 for SHA256
```

### Compare object graphs

```csharp
// Two requests with same data should produce same token
var r1 = new CreateOrderRequest { CustomerId = "CUST-001", Total = 149.99m };
var r2 = new CreateOrderRequest { CustomerId = "CUST-001", Total = 149.99m };
r1.SignHMAC("secret");
r2.SignHMAC("secret");
Console.WriteLine(r1.HmacToken == r2.HmacToken); // Should be true
```

### Verify property inclusion order

```csharp
// Add [HmacOrder] explicitly to control order
public class TestRequest : NuciApiRequest
{
    [HmacOrder(1)] public string A { get; set; }
    [HmacOrder(2)] public string B { get; set; }
}
```

## Getting help

1. Check existing tests for expected behaviour
2. Review `NuciSecurity.HMAC` documentation
3. Open GitHub issue with:
   - Minimal reproduction
   - Expected vs actual behaviour
   - NuciAPI version
   - .NET version