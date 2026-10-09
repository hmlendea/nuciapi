# Documentation Maintenance

## Ownership

- **Primary maintainer:** Horațiu Mlendea
- **Review cadence:** Per release (when version changes)
- **Update trigger:** Any public API change, behavioural change, or new feature

## Documentation structure

```
docs/
├── INDEX.md                          # Master index (this file links to all)
├── architecture.md                   # High-level architecture
├── repository-overview.md            # Purpose, scope, entry points
├── repository-structure.md           # Source tree layout
├── components/
│   ├── request-model.md              # NuciApiRequest
│   ├── response-model.md             # Response hierarchy
│   └── hmac-integration.md           # NuciSecurity.HMAC integration
├── flows/
│   ├── request-signing-validation.md # Request HMAC flow
│   ├── response-signing-validation.md # Response HMAC flow
│   └── serialisation.md              # JSON serialisation
├── api-reference/
│   ├── INDEX.md                      # API reference index
│   ├── nuciapirequest.md
│   ├── nuciapiresponse.md
│   ├── nuciapisuccessresponse.md
│   ├── nuciapierrorresponse.md
│   ├── nuciapicontentresponse.md
│   ├── nuciapiresponsecontent.md
│   ├── response-codes.md
│   └── response-messages.md
├── configuration.md
├── dependencies.md
├── error-handling.md
├── logging.md
├── security.md
├── testing.md
├── build-and-deployment.md
├── change-guide.md
├── invariants.md
├── concurrency-and-scheduling.md
├── state-and-persistence.md
├── integrations.md
├── troubleshooting.md
├── faq.md
├── ambiguities-and-open-questions.md
├── design-decisions.md
├── api-usage-examples.md
├── quick-start.md
└── documentation-maintenance.md      # This file
```

## Update procedures

### When adding a public type/member

1. Add/update API reference file in `api-reference/`
2. Update `api-reference/INDEX.md`
3. Add component documentation in `components/` if new concept
4. Add flow documentation in `flows/` if new execution path
5. Update `INDEX.md` catalogue

### When changing behaviour

1. Update relevant component/flow documentation
2. Update `change-guide.md` with migration notes
3. Update `invariants.md` if invariants change
4. Update `testing.md` if test strategy changes
5. Update `ambiguities-and-open-questions.md` if resolved

### When fixing a bug

1. Update `troubleshooting.md` with symptom/fix
2. Update `faq.md` if common question
3. Add test case (see `testing.md`)

### When releasing

1. Update version references if any
2. Verify all links work
3. Check `INDEX.md` last reviewed date

## Link validation

All cross-references use relative paths from `docs/` root:
- `[Architecture](./architecture.md)`
- `[Request Model](./components/request-model.md)`

Verify with: `markdown-link-check` or similar tool.

## Style guide

- **Headings:** Sentence case (`## Repository purpose`)
- **Code symbols:** Backticks with full namespace (`` `NuciAPI.Requests.NuciApiRequest` ``)
- **File references:** Relative links (`./components/request-model.md`)
- **External links:** Absolute URLs
- **Tables:** For structured data (API members, factory properties, etc.)
- **Code blocks:** Language specified (`csharp`, `json`, `bash`, `yaml`)

## Tools

- **Editor:** VS Code with Markdown extensions
- **Preview:** Built-in Markdown preview
- **Link check:** `markdown-link-check` (CI optional)
- **Spell check:** `cspell` (optional)

## Archive policy

- Old versions not archived — documentation reflects current release
- Major version changes: create new docs structure if needed
- `ambiguities-and-open-questions.md` tracks unresolved items across versions