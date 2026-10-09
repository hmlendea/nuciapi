# Repository Overview

## Purpose

NuciAPI is a .NET library that provides strongly-typed base classes for building consistent API request/response contracts with integrated HMAC signing and validation. It targets .NET 10.0 and is distributed as a NuGet package.

## Scope

**Owns:**
- Base request class (`NuciApiRequest`) with HMAC signing/validation
- Response hierarchy: `NuciApiResponse` (abstract), `NuciApiSuccessResponse`, `NuciApiErrorResponse`, `NuciApiContentResponse<T>`
- Standardised success/error codes (`NuciApiResponseCodes`) and messages (`NuciApiResponseMessages`)
- Integration with `NuciSecurity.HMAC` for cryptographic operations

**Does not own:**
- HTTP transport, routing, or server infrastructure
- Authentication/authorisation frameworks
- Database or persistence logic
- Serialisation configuration beyond JSON property naming

## Entry points

| Entry point | Location | Purpose |
|-------------|----------|---------|
| Library consumption | `NuciAPI.csproj` → NuGet package `NuciAPI` | Consumers reference the package and inherit from base classes |
| Development build | `dotnet build NuciAPI.sln` | Compiles library and runs unit tests |
| Release | GitHub Release workflow → NuGet | Packs and publishes versioned package |

## Target framework

- .NET 10.0 (`net10.0`)
- `ImplicitUsings` disabled — all namespaces must be explicitly imported

## Version

Current: 3.6.1 (defined in `NuciAPI.csproj`)