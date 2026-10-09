# State and Persistence

## Overview

NuciAPI has **no internal state and no persistence**. It is a stateless library with no databases, caches, files, or stateful components.

## State management

| Aspect | Status |
|--------|--------|
| In-memory state | None |
| Static fields | None |
| Singleton instances | None |
| Caches | None |
| Session state | None |
| Database | None |
| File storage | None |
| Distributed cache | None |

## Object lifecycle

```
Request DTO created → SignHMAC → Transmitted → Received → ValidateHMAC → Processed → Discarded
Response DTO created → SignHMAC → Transmitted → Received → ValidateHMAC → Processed → Discarded
```

- Objects are short-lived, created per request/response
- No pooling, no reuse, no retention
- GC collects normally

## HMAC token lifetime

- `HmacToken` property exists only for the lifetime of the DTO instance
- Generated on-demand by `SignHMAC`
- Validated on-demand by `ValidateHMAC`
- Not persisted, not cached, not reused

## Consumer state (out of scope)

Consumers may have state, but NuciAPI does not manage or interact with it:

| Consumer state | NuciAPI involvement |
|----------------|---------------------|
| Secret key storage | None — passed as parameter |
| Request/response logging | None — consumer responsibility |
| Replay protection (nonces) | None — consumer implements |
| Rate limiting | None — consumer implements |
| Session management | None — consumer implements |

## Serialisation as persistence

The only "persistence" is JSON serialisation for transport:

```csharp
// Transient serialisation for HTTP transport
string json = JsonSerializer.Serialize(request);
// Transient deserialisation on receipt
var request = JsonSerializer.Deserialize<CreateOrderRequest>(json);
```

- No schema versioning in library
- Consumer handles version tolerance
- `System.Text.Json` handles forward/backward compatibility

## Migrations

**Not applicable** — no persistent schema, no database, no stored format versioning.

## Backup and recovery

**Not applicable** — no data to backup.

## Disaster recovery

**Not applicable** — stateless library; redeploy package to recover.