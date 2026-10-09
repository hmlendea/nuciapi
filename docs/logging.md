# Logging

## Overview

NuciAPI **does not include any logging framework**. It is a stateless library with no internal logging, no logging dependencies, and no logging configuration.

## Logging framework

| Aspect | Status |
|--------|--------|
| Library used | None |
| Configuration source | N/A |
| Log levels used | N/A |
| Structured fields | N/A |
| Correlation IDs | N/A |
| Sinks/destinations | N/A |
| Redaction/masking | N/A |
| Retention | N/A |
| Sampling | N/A |
| Performance impact | None |
| Error logging | N/A |
| Audit logging | N/A |

## Consumer responsibility

Consumers are responsible for all logging in their applications. NuciAPI provides no hooks, callbacks, or events for logging.

### Recommended logging points (consumer side)

```csharp
// Request received
logger.LogInformation("Request received: {RequestType}", request.GetType().Name);

// HMAC validation
try
{
    request.ValidateHMAC(secretKey);
    logger.LogDebug("HMAC validation succeeded for {RequestType}", request.GetType().Name);
}
catch (HmacValidationException)
{
    logger.LogWarning("HMAC validation failed for {RequestType}", request.GetType().Name);
    return Unauthorized();
}

// Response signing
response.SignHMAC(secretKey);
logger.LogDebug("Response signed: {ResponseType}", response.GetType().Name);

// Errors
logger.LogError(ex, "Error processing {RequestType}", request.GetType().Name);
```

## HMAC token logging (security)

**Never log HMAC tokens or secret keys.**

```csharp
// BAD - exposes secret
logger.LogDebug("Signing with key: {Key}", secretKey);

// BAD - exposes token
logger.LogDebug("HMAC token: {Token}", request.HmacToken);

// GOOD - log only metadata
logger.LogDebug("HMAC validation {Result} for {RequestType}", 
    success ? "succeeded" : "failed", request.GetType().Name);
```

## Structured logging recommendations

If consumers use structured logging (Serilog, NLog, etc.), recommended properties:

| Property | Value |
|----------|-------|
| `RequestType` | `request.GetType().Name` |
| `ResponseType` | `response.GetType().Name` |
| `HmacValid` | `true`/`false` |
| `ResponseCode` | `response.Code` |
| `IsSuccessful` | `response.IsSuccessful` |
| `CorrelationId` | Consumer-generated correlation ID |

## Performance

- Zero logging overhead in NuciAPI itself
- Consumer logging performance depends on their chosen framework
- HMAC computation is the primary CPU cost — consider async logging for high throughput

## Audit logging

For security-relevant events, consumers should log:

| Event | Log level | Fields |
|-------|-----------|--------|
| HMAC validation failure | Warning | RequestType, ClientIP, CorrelationId |
| HMAC validation success | Debug | RequestType, CorrelationId |
| Response signing | Debug | ResponseType, CorrelationId |
| Invalid request deserialisation | Warning | Error, CorrelationId |

## Configuration

No logging configuration in NuciAPI. Consumers configure their logging framework independently.