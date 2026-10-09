# Architecture

## Repository Purpose

NuciAPI is a lightweight .NET library that provides strongly-typed base classes for building consistent API request/response contracts with integrated HMAC signing and validation. It targets .NET 10.0 and has a single external dependency: `NuciSecurity.HMAC` for cryptographic operations.

## Architectural Decomposition

```
┌──────────────────────────────────────────────────────────────────┐
│                      Consumer Application                        │
│  (defines concrete Request/Response DTOs inheriting from NuciAPI)│
└──────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                           NuciAPI                               │
│  ┌─────────────────────────┐  ┌─────────────────────────────┐   │
│  │ NuciAPI.Requests        │  │ NuciAPI.Responses           │   │
│  │ • NuciApiRequest (abs)  │  │ • NuciApiResponse (abs)     │   │
│  │                         │  │ • NuciApiSuccessResponse    │   │
│  │                         │  │ • NuciApiErrorResponse      │   │
│  │                         │  │ • NuciApiContentResponse<T> │   │
│  │                         │  │ • NuciApiResponseContent    │   │
│  │                         │  │ • NuciApiResponseCodes      │   │
│  │                         │  │ • NuciApiResponseMessages   │   │
│  └─────────────────────────┘  └─────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    NuciSecurity.HMAC (external)                 │
│  • HmacEncoder.GenerateToken                                    │
│  • HmacValidator.IsTokenValid / Validate                        │
│  • [HmacOrder], [HmacIgnore] attributes                         │
└─────────────────────────────────────────────────────────────────┘
```

## Principal Capabilities

| Capability | Description |
|------------|-------------|
| **Request contracts** | Abstract `NuciApiRequest` with `SignHMAC`, `HasValidHMAC`, `ValidateHMAC` methods |
| **Response contracts** | Abstract `NuciApiResponse` with `IsSuccessful`, `Message`, `Code`, `HmacToken` |
| **Success responses** | `NuciApiSuccessResponse` with factory properties (Default, Created, Deleted, Fetched, NotUpdated, Updated) |
| **Error responses** | `NuciApiErrorResponse` with 11 factory properties covering standard HTTP error codes |
| **Typed content responses** | Generic `NuciApiContentResponse<TContent>` for strongly-typed payloads |
| **Standardised codes/messages** | Constants in `NuciApiResponseCodes` and `NuciApiResponseMessages` |

## Component Relationships

| Component | Depends On | Consumed By |
|-----------|------------|-------------|
| `NuciApiRequest` | `NuciSecurity.HMAC` | Consumer request DTOs |
| `NuciApiResponse` | `NuciSecurity.HMAC` | All response types |
| `NuciApiSuccessResponse` | `NuciApiResponse` | Consumer success DTOs |
| `NuciApiErrorResponse` | `NuciApiResponse` | Consumer error DTOs |
| `NuciApiContentResponse<T>` | `NuciApiResponse`, `NuciApiResponseContent` | Consumer typed responses |
| `NuciApiResponseCodes` | — | All response constructors |
| `NuciApiResponseMessages` | — | All response constructors |

## Dependency Direction

```
Consumer code → NuciAPI → NuciSecurity.HMAC
```

- No circular dependencies
- NuciAPI has zero dependencies on consumer code
- Single external dependency: `NuciSecurity.HMAC` (v4.1.3)

## Runtime Topology

- **Single assembly**: `NuciAPI.dll` (net10.0)
- **No background threads**, timers, or scheduled work
- **No internal state** — all operations are pure functions of input + secret key
- **Thread-safe**: HMAC operations use no shared mutable state
- **No configuration**, logging, or persistence infrastructure

## Major State

**None.** The library is intentionally stateless. HMAC tokens are computed on-demand from the object graph + secret key. No instance or static state is retained between operations.

## Major Integrations

| Integration | Purpose | Coupling |
|-------------|---------|----------|
| `NuciSecurity.HMAC` | Cryptographic signing/validation | Compile-time (attributes), runtime (encoder/validator) |
| `System.Text.Json` | Serialisation | Compile-time (attributes: `JsonPropertyName`, `JsonIgnore`, `JsonConstructor`) |

## System-Wide Invariants

1. **HMAC covers all non-ignored properties** — `[HmacIgnore]` excludes `HmacToken` itself; `[HmacOrder]` controls property inclusion order
2. **Success responses always have `IsSuccessful == true`** — enforced by override in `NuciApiSuccessResponse` and `NuciApiContentResponse<T>`
3. **Error responses always have `IsSuccessful == false`** — enforced by override in `NuciApiErrorResponse`
4. **Content responses require `TContent : NuciApiResponseContent`** — generic constraint ensures content participates in HMAC
5. **Serialisation shape** — `success`, `message`, `code` at root; `content` only on success responses with payload
6. **Immutability of codes/messages** — `NuciApiResponseCodes` and `NuciApiResponseMessages` are `const` strings

## Documentation

Detailed architectural documentation lives in `docs/`:

- `docs/architecture.md` — detailed component diagrams and relationships
- `docs/components/` — per-component specifications
- `docs/flows/` — request/response and HMAC flow diagrams
- `docs/api-reference/` — public API surface
- `docs/behaviour/` — behavioural contracts and guarantees
- `docs/INDEX.md` — complete documentation catalogue