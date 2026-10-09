# Architecture

## High-level decomposition

NuciAPI follows a simple layered architecture with two primary namespaces:

```
┌─────────────────────────────────────────────────────────────┐
│                    Consumer Application                      │
│  (inherits NuciApiRequest, NuciApiSuccessResponse, etc.)    │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                        NuciAPI                               │
│  ┌─────────────────────┐  ┌─────────────────────────────┐   │
│  │ NuciAPI.Requests    │  │ NuciAPI.Responses           │   │
│  │ • NuciApiRequest    │  │ • NuciApiResponse (abstract)│   │
│  │                     │  │ • NuciApiSuccessResponse    │   │
│  │                     │  │ • NuciApiErrorResponse      │   │
│  │                     │  │ • NuciApiContentResponse<T> │   │
│  │                     │  │ • NuciApiResponseContent    │   │
│  │                     │  │ • NuciApiResponseCodes      │   │
│  │                     │  │ • NuciApiResponseMessages   │   │
│  └─────────────────────┘  └─────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                  NuciSecurity.HMAC (external)               │
│  • HmacEncoder.GenerateToken                                │
│  • HmacValidator.IsTokenValid / Validate                    │
│  • [HmacOrder], [HmacIgnore] attributes                     │
└─────────────────────────────────────────────────────────────┘
```

## Component relationships

| Component | Depends on | Consumed by |
|-----------|------------|-------------|
| `NuciApiRequest` | `NuciSecurity.HMAC` | Consumer request DTOs |
| `NuciApiResponse` | `NuciSecurity.HMAC` | All response types |
| `NuciApiSuccessResponse` | `NuciApiResponse` | Consumer success DTOs |
| `NuciApiErrorResponse` | `NuciApiResponse` | Consumer error DTOs |
| `NuciApiContentResponse<T>` | `NuciApiResponse`, `NuciApiResponseContent` | Consumer typed responses |
| `NuciApiResponseCodes` | — | All response constructors |
| `NuciApiResponseMessages` | — | All response constructors |

## Dependency direction

```
Consumer code → NuciAPI → NuciSecurity.HMAC
```

No circular dependencies. NuciAPI has zero dependencies on consumer code.

## Runtime topology

- Single assembly (`NuciAPI.dll`)
- No background threads, timers, or scheduled work
- No internal state — all operations are pure functions of input + secret key
- Thread-safe: HMAC operations use no shared mutable state

## Major state

None. The library is stateless. HMAC tokens are computed on-demand from the object graph + secret key.

## Major integrations

| Integration | Purpose | Coupling |
|-------------|---------|----------|
| `NuciSecurity.HMAC` | Cryptographic signing/validation | Compile-time (attributes), runtime (encoder/validator) |
| `System.Text.Json` | Serialisation | Compile-time (attributes: `JsonPropertyName`, `JsonIgnore`, `JsonConstructor`) |

## System-wide invariants

1. **HMAC covers all non-ignored properties** — `[HmacIgnore]` excludes `HmacToken` itself; `[HmacOrder]` controls property inclusion order
2. **Success responses always have `IsSuccessful == true`** — enforced by override in `NuciApiSuccessResponse` and `NuciApiContentResponse<T>`
3. **Error responses always have `IsSuccessful == false`** — enforced by override in `NuciApiErrorResponse`
4. **Content responses require `TContent : NuciApiResponseContent`** — generic constraint ensures content participates in HMAC
5. **Serialisation shape** — `success`, `message`, `code` at root; `content` only on success responses with payload
6. **Immutability of codes/messages** — `NuciApiResponseCodes` and `NuciApiResponseMessages` are `const` strings