# Security

## Overview

This document complements the root [SECURITY.md](../SECURITY.md) with implementation-specific security details for NuciAPI.

## Threat model

### Assets protected

| Asset | Protection |
|-------|------------|
| Request integrity | HMAC-SHA256 via `NuciSecurity.HMAC` |
| Response integrity | HMAC-SHA256 via `NuciSecurity.HMAC` |
| Secret key confidentiality | Consumer responsibility |
| Payload confidentiality | Not provided (HMAC ≠ encryption) |

### Threats addressed

| Threat | Mitigation |
|--------|------------|
| Request tampering | HMAC validation on server |
| Response tampering | HMAC validation on client |
| Replay attacks | Not mitigated — consumer must implement (timestamps, nonces) |
| Key compromise | Consumer key rotation |
| Timing attacks | Constant-time comparison in `NuciSecurity.HMAC` |

### Threats NOT addressed

| Threat | Reason |
|--------|--------|
| Payload encryption | HMAC provides integrity, not confidentiality |
| Replay protection | Out of scope for library |
| Key distribution | Consumer responsibility |
| DoS via HMAC computation | Negligible cost; consumer may rate-limit |

## HMAC security

### Algorithm

- **Algorithm:** HMAC-SHA256 (via `NuciSecurity.HMAC`)
- **Key length:** Minimum 256 bits (32 bytes) recommended
- **Token format:** Base64-encoded HMAC

### Key management

```csharp
// GOOD: Key from secure configuration
string secretKey = configuration["Hmac:SecretKey"]; // Key Vault, env var, etc.

// BAD: Hardcoded key
string secretKey = "hardcoded-secret"; // NEVER DO THIS

// BAD: Weak key
string secretKey = "short"; // Too short for HMAC-SHA256
```

### Token handling

```csharp
// GOOD: Token transmitted via header
context.Request.Headers["X-HMAC-Token"] = request.HmacToken;

// ACCEPTABLE: Token in body (if header not feasible)
var body = new { Request = request, HmacToken = request.HmacToken };

// NEVER: Log tokens
logger.LogInformation("Token: {Token}", request.HmacToken); // SECURITY VIOLATION
```

## Secure usage patterns

### Server-side validation

```csharp
public async Task<IActionResult> Post([FromBody] CreateOrderRequest request)
{
    // 1. Extract token from header (not body)
    if (!Request.Headers.TryGetValue("X-HMAC-Token", out var token))
        return Unauthorized();

    request.HmacToken = token;

    // 2. Validate with constant-time comparison
    try
    {
        request.ValidateHMAC(secretKey);
    }
    catch (HmacValidationException)
    {
        return Unauthorized();
    }

    // 3. Process validated request
    var result = await ProcessOrder(request);

    // 4. Sign response
    var response = NuciApiSuccessResponse.Created;
    response.SignHMAC(secretKey);
    Response.Headers["X-HMAC-Token"] = response.HmacToken;

    return Ok(response);
}
```

### Client-side validation

```csharp
var response = await httpClient.PostAsJsonAsync("/orders", request);
var responseBody = await response.Content.ReadFromJsonAsync<NuciApiSuccessResponse>();
var token = response.Headers.GetValues("X-HMAC-Token").First();

responseBody.HmacToken = token;

try
{
    responseBody.ValidateHMAC(secretKey);
}
catch (HmacValidationException)
{
    throw new SecurityException("Response HMAC validation failed");
}
```

## Cryptographic dependencies

### NuciSecurity.HMAC

- **Version:** 4.1.3 (pinned)
- **Responsibility:** HMAC generation, validation, constant-time comparison
- **Update policy:** Test thoroughly before upgrading; verify HMAC compatibility

### System.Text.Json

- **Risk:** Deserialisation vulnerabilities (mitigated by .NET runtime updates)
- **Mitigation:** Keep .NET runtime updated

## Vulnerability reporting

See root [SECURITY.md](../SECURITY.md) for reporting process.

### In-scope for NuciAPI

- HMAC bypass or weakness in NuciAPI usage of `NuciSecurity.HMAC`
- Serialisation issues leading to HMAC mismatch
- Property inclusion/exclusion logic errors

### Out-of-scope

- `NuciSecurity.HMAC` internal vulnerabilities (report to that project)
- Consumer misconfiguration (weak keys, logging tokens, etc.)
- Replay attacks (consumer responsibility)

## Security testing

### Current coverage

- Unit tests verify HMAC token generation and property inclusion
- No penetration tests or cryptographic validation tests
- No fuzzing of deserialisation

### Recommended additions

- Test HMAC validation with malformed tokens
- Test constant-time comparison property
- Test property ordering edge cases
- Fuzz deserialisation with malicious JSON

## Compliance

- **GDPR:** No personal data processed by library
- **PCI DSS:** No card data; consumer responsible for their data
- **FIPS:** Depends on `NuciSecurity.HMAC` and .NET runtime
- **SOC 2:** Library stateless; no audit trail generated