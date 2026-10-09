# NuciAPI Documentation

## Repository summary

NuciAPI is a small .NET library for building consistent API contracts around strongly-typed request and response models, with integrated HMAC signing and validation for payload integrity. It provides base classes for requests and responses, standardised success/error codes and messages, and HMAC-based payload signing/validation.

## Root document map

- [ARCHITECTURE.md](../ARCHITECTURE.md) — High-level architecture (if present)
- [SECURITY.md](../SECURITY.md) — Security policy and vulnerability reporting
- [ROADMAP.md](../ROADMAP.md) — Project roadmap (if present)
- [PRIVACY.md](../PRIVACY.md) — Privacy policy (if present)
- [LICENSE](../LICENSE) — GPL-3.0-or-later license

## Documentation catalogue

### Architecture & design
- [architecture.md](./architecture.md) — High-level architecture complementing root ARCHITECTURE.md
- [design-decisions.md](./design-decisions.md) — Key architectural/design choices and rationale
- [repository-overview.md](./repository-overview.md) — Repository purpose, scope, entry points
- [repository-structure.md](./repository-structure.md) — Source tree layout, module organisation

### Components
- [components/request-model.md](./components/request-model.md) — Request base class and HMAC signing
- [components/response-model.md](./components/response-model.md) — Response hierarchy (success, error, content)
- [components/hmac-integration.md](./components/hmac-integration.md) — NuciSecurity.HMAC integration details

### Behaviour guides
- [behaviour/browse-and-search.md](./behaviour/browse-and-search.md) — Finding types, codes, and patterns
- [behaviour/inspect-edit-delete.md](./behaviour/inspect-edit-delete.md) — Inspecting, modifying, and validating requests/responses

### Flows
- [flows/request-signing-validation.md](./flows/request-signing-validation.md) — Request HMAC sign/validate flow
- [flows/response-signing-validation.md](./flows/response-signing-validation.md) — Response HMAC sign/validate flow
- [flows/serialisation.md](./flows/serialisation.md) — JSON serialisation behaviour

### API reference
- [api-reference/INDEX.md](./api-reference/INDEX.md) — API reference index (not applicable — this is a library, not a web API)
- [api-reference/nuciapirequest.md](./api-reference/nuciapirequest.md) — NuciApiRequest class reference
- [api-reference/nuciapiresponse.md](./api-reference/nuciapiresponse.md) — NuciApiResponse base class reference
- [api-reference/nuciapisuccessresponse.md](./api-reference/nuciapisuccessresponse.md) — NuciApiSuccessResponse class reference
- [api-reference/nuciapierrorresponse.md](./api-reference/nuciapierrorresponse.md) — NuciApiErrorResponse class reference
- [api-reference/nuciapicontentresponse.md](./api-reference/nuciapicontentresponse.md) — NuciApiContentResponse generic class reference
- [api-reference/response-codes.md](./api-reference/response-codes.md) — NuciApiResponseCodes reference
- [api-reference/response-messages.md](./api-reference/response-messages.md) — NuciApiResponseMessages reference

### Cross-cutting concerns
- [configuration.md](./configuration.md) — Configuration schema, sources, precedence
- [dependencies.md](./dependencies.md) — External and internal dependencies
- [error-handling.md](./error-handling.md) — Error taxonomy, handling patterns, recovery
- [logging.md](./logging.md) — Logging framework, levels, structured fields, correlation, sinks
- [security.md](./security.md) — Security model, threats, mitigations (complements root SECURITY.md)
- [testing.md](./testing.md) — Test strategy, organisation, coverage
- [build-and-deployment.md](./build-and-deployment.md) — Build pipeline, deployment, environments
- [change-guide.md](./change-guide.md) — How to modify common areas safely
- [invariants.md](./invariants.md) — System-wide invariants and contracts
- [concurrency-and-scheduling.md](./concurrency-and-scheduling.md) — Threading, async, schedulers, locks
- [state-and-persistence.md](./state-and-persistence.md) — State management, stores, caches, migrations
- [integrations.md](./integrations.md) — External system integrations
- [troubleshooting.md](./troubleshooting.md) — Common issues and solutions
- [faq.md](./faq.md) — Frequently asked questions
- [ambiguities-and-open-questions.md](./ambiguities-and-open-questions.md) — Unresolved items, TODOs, known gaps
- [documentation-maintenance.md](./documentation-maintenance.md) — How to keep docs current, ownership
- [api-usage-examples.md](./api-usage-examples.md) — Example requests/responses for API usage
- [quick-start.md](./quick-start.md) — Getting started guide for newcomers

## Navigation aids

- **Start here**: [quick-start.md](./quick-start.md) → [repository-overview.md](./repository-overview.md) → [architecture.md](./architecture.md)
- **Deep dive**: [components/](./components/) → [flows/](./flows/) → [api-reference/](./api-reference/)
- **Flows**: [flows/request-signing-validation.md](./flows/request-signing-validation.md) → [flows/response-signing-validation.md](./flows/response-signing-validation.md)

## Maintenance metadata

- Last reviewed: 2026-10-09
- Owner: Horațiu Mlendea
- Coverage status: Initial generation — comprehensive coverage of all public types and flows