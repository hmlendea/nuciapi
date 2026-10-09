# Invariants

## System-wide invariants

### 1. HMAC covers all non-ignored properties

**Rule:** Every public instance property of a request/response object (and nested objects) is included in HMAC computation unless marked `[HmacIgnore]`.

**Enforcement:** `NuciSecurity.HMAC.HmacEncoder.GenerateToken` uses reflection with `[HmacIgnore]` filter.

**Violation consequence:** Property changes not detected by HMAC validation.

### 2. HmacToken never included in its own computation

**Rule:** `HmacToken` property on both `NuciApiRequest` and `NuciApiResponse` is marked `[HmacIgnore]`.

**Enforcement:** Attribute on property declaration.

**Violation consequence:** Circular dependency — token would depend on itself.

### 3. Success responses always have IsSuccessful == true

**Rule:** `NuciApiSuccessResponse.IsSuccessful` and `NuciApiContentResponse<T>.IsSuccessful` override to return `true`.

**Enforcement:** `override bool IsSuccessful => true;`

**Violation consequence:** Consumer logic relying on `IsSuccessful` breaks.

### 4. Error responses always have IsSuccessful == false

**Rule:** `NuciApiErrorResponse.IsSuccessful` overrides to return `false`.

**Enforcement:** `override bool IsSuccessful => false;` (sealed class)

**Violation consequence:** Consumer error handling breaks.

### 5. Content responses require typed content

**Rule:** `NuciApiContentResponse<TContent>` constrains `TContent : NuciApiResponseContent`.

**Enforcement:** Generic constraint `where TContent : NuciApiResponseContent`.

**Violation consequence:** Content without HMAC participation possible.

### 6. Serialisation shape consistency

**Rule:**
- Success responses: `success`, `message`, `code`, `content` (nullable)
- Error responses: `success`, `message`, `code` (no `content`)
- Content responses: `success`, `content`, `message`, `code`

**Enforcement:** `JsonPropertyName` attributes, `JsonIgnore` on `HmacToken`, no `Content` property on `NuciApiErrorResponse`.

**Violation consequence:** Consumer deserialisation fails or produces unexpected shape.

### 7. Codes and messages are compile-time constants

**Rule:** All values in `NuciApiResponseCodes` and `NuciApiResponseMessages` are `const string`.

**Enforcement:** `public const string` declarations.

**Violation consequence:** Runtime mutation possible; switch expressions may not optimise.

### 8. No internal mutable state

**Rule:** Library classes have no static mutable fields, no singleton state, no caches.

**Enforcement:** Code review — all methods are pure functions of input + secret key.

**Violation consequence:** Thread-safety issues, test interference, unexpected behaviour.

### 9. Thread-safety

**Rule:** All public methods are thread-safe (no shared state).

**Enforcement:** Stateless design; `HmacEncoder`/`HmacValidator` from `NuciSecurity.HMAC` are thread-safe.

**Violation consequence:** Race conditions in concurrent scenarios.

### 10. Deterministic HMAC for identical object graphs

**Rule:** Same property values → same HMAC token (given same secret key).

**Enforcement:** `NuciSecurity.HMAC` deterministic serialisation + HMAC-SHA256.

**Violation consequence:** Validation fails for valid requests; replay detection impossible.

## Component invariants

### NuciApiRequest

| Invariant | Description |
|-----------|-------------|
| `HmacToken` null until `SignHMAC` | Default is `null`; only set by `SignHMAC` |
| `SignHMAC` overwrites | Calling twice replaces token |
| Derived properties included | Reflection includes all public instance properties |

### NuciApiResponse

| Invariant | Description |
|-----------|-------------|
| `IsSuccessful` abstract | Must be overridden |
| `HmacToken` excluded | `[JsonIgnore]` + `[HmacIgnore]` |
| Metadata HMAC order fixed | Code=9999997, Message=9999998, IsSuccessful=9999999 |

### NuciApiSuccessResponse

| Invariant | Description |
|-----------|-------------|
| `IsSuccessful == true` | Override guarantees |
| `Content` nullable | Virtual, defaults to null |
| Factory properties immutable | Return new instances each access |

### NuciApiErrorResponse

| Invariant | Description |
|-----------|-------------|
| `IsSuccessful == false` | Override guarantees (sealed) |
| No `Content` property | Not declared |
| Factory properties immutable | Return new instances each access |

### NuciApiContentResponse<T>

| Invariant | Description |
|-----------|-------------|
| `IsSuccessful == true` | Override guarantees |
| `Content` non-null | Constructor requires it |
| `TContent : NuciApiResponseContent` | Generic constraint |
| `[JsonConstructor]` enables deserialisation | Round-trip support |

## Consumer-facing invariants

### For request DTOs

1. Inherit from `NuciApiRequest`
2. Apply `[HmacOrder]` to control property order
3. Call `SignHMAC` before sending
4. Never log `HmacToken` or secret key

### For response DTOs

1. Inherit from appropriate response base
2. Content types inherit from `NuciApiResponseContent`
3. Apply `[HmacOrder]` to content properties
4. Call `SignHMAC` before sending
5. Validate on receipt with `ValidateHMAC`

### For HMAC secret keys

1. Minimum 256 bits (32 bytes)
2. Stored securely (Key Vault, env var, not code)
3. Rotated periodically
4. Same key used for sign + validate

## Versioning invariants

| Invariant | Policy |
|-----------|--------|
| Public API additions | Minor version |
| Public API removals/breaking changes | Major version |
| HMAC algorithm/format changes | Major version |
| Dependency upgrades (NuciSecurity.HMAC) | Test → patch/minor/major |
| Target framework changes | Major version |