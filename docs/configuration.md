# Configuration

## Overview

NuciAPI has **no configuration system**. It is a stateless library with no runtime configuration, no dependency injection setup, and no configuration files.

## Configuration sources

| Source | Supported | Notes |
|--------|-----------|-------|
| `appsettings.json` | No | Not used |
| Environment variables | No | Not used |
| Command line args | No | Not used |
| `IConfiguration` | No | Not used |
| `IOptions<>` | No | Not used |
| Compile-time constants | Yes | `NuciApiResponseCodes`, `NuciApiResponseMessages` |

## Behaviour modification

The only way to modify library behaviour is:

1. **Subclassing** — Inherit from `NuciApiRequest`, `NuciApiSuccessResponse`, etc.
2. **Factory methods** — Use `FromMessage` for custom messages
3. **HMAC secret key** — Passed at call time to `SignHMAC`/`ValidateHMAC`
4. **`[HmacOrder]` attributes** — Control property inclusion order in derived types

## Version-specific behaviour

| Version | Target framework | ImplicitUsings |
|---------|------------------|----------------|
| 3.6.1 | net10.0 | Disabled |

No configuration controls target framework or language features.

## NuGet package configuration

Defined in `NuciAPI.csproj`:

```xml
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
  <RootNamespace>Small .NET library for building consistent API contracts.</RootNamespace>
  <Version>3.6.1</Version>
  <Description>NuciAPI</Description>
  <Authors>Horațiu Mlendea</Authors>
  <Copyright>Copyright 2026 © Horațiu Mlendea</Copyright>
  <RepositoryUrl>https://github.com/hmlendea/nuciapi</RepositoryUrl>
  <PackageLicenseExpression>GPL-3.0-or-later</PackageLicenseExpression>
  <PackageTags>REST API</PackageTags>
  <ImplicitUsings>disable</ImplicitUsings>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="NuciSecurity.HMAC" Version="4.1.3" />
</ItemGroup>
```

## Consumer configuration

Consumers configure NuciAPI by:

1. **Referencing the package** — `dotnet add package NuciAPI`
2. **Choosing secret keys** — Application-specific, not library-managed
3. **Defining DTOs** — Inherit from base classes, apply `[HmacOrder]`
4. **Calling HMAC methods** — At appropriate points in their pipeline