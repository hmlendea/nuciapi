# Repository Structure

## Source tree layout

```
NuciAPI.sln                          # Solution file
NuciAPI/                             # Main library project
├── NuciAPI.csproj                   # Project file (net10.0, GPL-3.0-or-later)
├── Requests/
│   └── NuciApiRequest.cs            # Abstract base class for API requests
└── Responses/
    ├── NuciApiResponse.cs           # Abstract base class for all responses
    ├── NuciApiResponseContent.cs    # Abstract base for response payload content
    ├── NuciApiSuccessResponse.cs    # Successful response (no typed content)
    ├── NuciApiErrorResponse.cs      # Error response (sealed)
    ├── NuciApiContentResponse.cs    # Generic successful response with typed content
    ├── NuciApiResponseCodes.cs      # Static success/error code constants
    └── NuciApiResponseMessages.cs   # Static success/error message constants

NuciAPI.UnitTests/                   # Unit test project
├── NuciAPI.UnitTests.csproj         # Test project (net10.0, NUnit, Moq)
├── Helpers/
│   ├── DummyRequest.cs              # Test request implementation
│   ├── EmptyRequest.cs              # Empty request for HMAC comparison
│   ├── DummyResponse.cs             # Test success response implementation
│   ├── EmptyResponse.cs             # Empty response for HMAC comparison
│   └── DummyResponseContent.cs      # Test content with HmacOrder attribute
├── NuciApiRequestTests.cs           # Request HMAC tests
├── NuciApiResponseTests.cs          # Response HMAC tests
├── NuciApiSuccessResponseTests.cs   # Success response tests
├── NuciApiErrorResponseTests.cs     # Error response tests
└── NuciApiContentResponseTests.cs   # Generic content response tests

.github/workflows/
├── dotnet.yml                       # CI: build + test on push/PR to master
└── github-release.yml               # CD: pack + publish to GitHub Release on tag

docs/                                # Documentation (this directory)
```

## Module organisation

| Module | Namespace | Responsibility |
|--------|-----------|----------------|
| Requests | `NuciAPI.Requests` | Request base class, HMAC sign/validate |
| Responses | `NuciAPI.Responses` | Response hierarchy, codes, messages, content |

## Project references

- `NuciAPI.UnitTests` → `NuciAPI` (ProjectReference)
- `NuciAPI` → `NuciSecurity.HMAC` 4.1.3 (PackageReference)

## Build outputs

- `NuciAPI/bin/Debug/net10.0/NuciAPI.dll` — Library assembly
- `NuciAPI.UnitTests/bin/Debug/net10.0/NuciAPI.UnitTests.dll` — Test assembly