# Ambiguities and Open Questions

## Known gaps

### 1. Replay protection

**Status:** Not implemented
**Question:** Should the library provide built-in replay protection (timestamps, nonces)?
**Current:** Consumer responsibility
**Consideration:** Adds complexity; may not fit all use cases

### 2. Key versioning

**Status:** Not supported
**Question:** Should `HmacToken` include a key version identifier?
**Current:** Consumer must implement via custom header
**Consideration:** Enables zero-downtime key rotation

### 3. Algorithm agility

**Status:** Fixed to HMAC-SHA256 via NuciSecurity.HMAC
**Question:** Should library support multiple algorithms?
**Current:** Single algorithm
**Consideration:** Future-proofing vs simplicity

### 4. Request/response correlation

**Status:** Not provided
**Question:** Should responses include request ID for correlation?
**Current:** Consumer adds correlation ID as property
**Consideration:** Useful for debugging; adds property to all responses

### 5. Streaming/large payloads

**Status:** Not optimised
**Question:** How does HMAC work with streamed requests/responses?
**Current:** Requires full object in memory for HMAC
**Consideration:** May not suit large file uploads

### 6. Localisation of messages

**Status:** English only
**Question:** Should `NuciApiResponseMessages` support localisation?
**Current:** Const strings, English only
**Consideration:** Adds complexity; consumers can override with `FromMessage`

### 7. HTTP status code mapping

**Status:** Not provided
**Question:** Should library map codes to HTTP status codes?
**Current:** Consumer maps `Code` to status code
**Consideration:** Could add `HttpStatusCode` property to responses

### 8. Validation attributes

**Status:** Not integrated
**Question:** Should library integrate with `System.ComponentModel.DataAnnotations`?
**Current:** No validation beyond HMAC
**Consideration:** Separation of concerns vs convenience

### 9. OpenAPI/Swagger generation

**Status:** Not provided
**Question:** Should library provide OpenAPI schema generation?
**Current:** Consumer uses ASP.NET Core's built-in support
**Consideration:** Could add attributes for schema customisation

### 10. Async HMAC operations

**Status:** Synchronous only
**Question:** Should `SignHMACAsync`/`ValidateHMACAsync` be provided?
**Current:** Sync methods (fast, CPU-bound)
**Consideration:** Consistency with async ecosystems; minimal benefit

## Design decisions pending

### 11. NuciApiResponseContent as interface vs abstract class

**Current:** Abstract class
**Question:** Should it be an interface for more flexibility?
**Trade-off:** Abstract class enables base implementation; interface enables multiple inheritance

### 12. Immutable response objects

**Current:** Mutable properties (Message, Code, Content settable)
**Question:** Should responses be immutable (init-only, constructor-only)?
**Trade-off:** Immutability safer; mutability convenient for builders

### 13. Generic error response

**Current:** `NuciApiErrorResponse` is sealed, no content
**Question:** Should there be `NuciApiErrorResponse<TErrorContent>`?
**Trade-off:** Richer errors vs simplicity

### 14. Request validation (non-HMAC)

**Current:** None
**Question:** Should base request include validation hooks?
**Trade-off:** Framework integration vs library scope

## Technical debt

### 15. NuciSecurity.HMAC version pinning

**Current:** Pinned to 4.1.3
**Risk:** Security updates not automatically received
**Action:** Regular dependency review schedule needed

### 16. Test coverage gaps

**Current:** ~80% coverage (estimated)
**Gaps:** HMAC failure paths, deserialisation errors, edge cases
**Action:** Expand test suite per `testing.md` recommendations

### 17. Documentation completeness

**Current:** This documentation set
**Gaps:** Migration guides, advanced usage patterns
**Action:** Add as needed based on consumer feedback

## Future considerations

### 18. .NET Standard / multi-targeting

**Current:** net10.0 only
**Question:** Support older .NET versions?
**Trade-off:** Wider adoption vs maintenance burden

### 19. Source generators

**Current:** Reflection-based HMAC
**Question:** Use source generators for compile-time HMAC property discovery?
**Trade-off:** Performance vs complexity

### 20. AOT/Native AOT compatibility

**Current:** Not verified
**Question:** Ensure full Native AOT compatibility?
**Action:** Test and document