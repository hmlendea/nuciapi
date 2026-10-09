# Dependencies

## External dependencies

### NuGet packages

| Package | Version | Purpose | License |
|---------|---------|---------|---------|
| `NuciSecurity.HMAC` | 4.1.3 | HMAC encoding/validation, attributes | Unknown (external) |
| `Microsoft.NET.Test.Sdk` | 17.14.1 | Test runner (test project only) | MIT |
| `Moq` | 4.20.72 | Mocking framework (test project only) | BSD-3-Clause |
| `NUnit` | 4.3.2 | Test framework (test project only) | MIT |
| `NUnit3TestAdapter` | 5.0.0 | VS Test adapter (test project only) | MIT |

### Framework dependencies

| Dependency | Version | Purpose |
|------------|---------|---------|
| `System.Text.Json` | Built-in (net10.0) | JSON serialisation |
| `System.Runtime` | Built-in (net10.0) | Base types |

## Internal dependencies

### Project references

```
NuciAPI.UnitTests → NuciAPI (ProjectReference)
```

### Namespace dependencies

| Namespace | Depends on |
|-----------|------------|
| `NuciAPI.Requests` | `NuciSecurity.HMAC` |
| `NuciAPI.Responses` | `NuciSecurity.HMAC`, `System.Text.Json` |
| `NuciAPI.UnitTests` | `NuciAPI.Requests`, `NuciAPI.Responses`, `NUnit`, `Moq` |

## Dependency graph

```
NuciAPI (library)
├── NuciSecurity.HMAC 4.1.3 (runtime)
└── System.Text.Json (built-in)

NuciAPI.UnitTests (test)
├── NuciAPI (project)
├── NUnit 4.3.2
├── Moq 4.20.72
├── NUnit3TestAdapter 5.0.0
└── Microsoft.NET.Test.Sdk 17.14.1
```

## Version constraints

- **NuciSecurity.HMAC** — Pinned to 4.1.3; upgrade requires testing HMAC compatibility
- **Target framework** — net10.0; consumers must use .NET 10.0+
- **Test packages** — Latest compatible at time of release; not part of library distribution

## Transitive dependencies

NuciAPI has no transitive dependencies exposed to consumers beyond `NuciSecurity.HMAC`. Consumers only need:
- `NuciAPI` package
- `NuciSecurity.HMAC` (transitively installed)

## Security considerations

- `NuciSecurity.HMAC` is a cryptographic dependency — verify its integrity and update for security patches
- Test dependencies are not shipped with the library
- No native dependencies or platform-specific code