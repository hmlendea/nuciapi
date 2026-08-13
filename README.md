[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/nuciapi)](https://github.com/hmlendea/nuciapi/releases/latest)
[![Build Status](https://github.com/hmlendea/nuciapi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nuciapi/actions/workflows/dotnet.yml)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://gnu.org/licenses/gpl-3.0)

# NuciAPI

NuciAPI is a small .NET library for building consistent API contracts around strongly-typed request and response models, with integrated HMAC signing and validation for payload integrity.

## 📑 Table of Contents

- [Table of Contents](#-table-of-contents)
- [Capabilities](#-capabilities)
- [Usage](#-usage)
- [Installation](#-installation)
  - [CLI Installation](#cli-installation)
- [Development](#-development)
  - [Requirements](#requirements)
  - [Setup](#setup)
  - [Build](#build)
  - [Run](#run)
  - [Test](#test)
  - [Release](#release)
  - [Dependencies](#dependencies)
- [Project Structure](#-project-structure)
- [Contributing](#-contributing)
- [Related Projects](#-related-projects)
- [Security](#-security)
- [Supporting the Project](#-supporting-the-project)
- [License](#-license)

## ✨ Capabilities

- Base request and response contracts with built-in HMAC signing and validation
- Standardised success and error response models for consistent API behaviour

## 🚀 Usage

```csharp
using NuciAPI.Requests;
using NuciAPI.Responses;

var request = new CreateOrderRequest
{
	CustomerId = "CUST-001",
	Total = 149.99m
};

request.SignHMAC("super-secret-key");
request.ValidateHMAC("super-secret-key");

var response = NuciApiSuccessResponse.Created;
response.SignHMAC("super-secret-key");
```

## 📦 Installation

[![Obtain it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciAPI)
[![Obtain it from GitHub](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/github.png)](https://github.com/hmlendea/nuciapi/releases)

### CLI Installation

```bash
dotnet add package NuciAPI
```

Or, via the `Package Manager Console`:
```powershell
Install-Package NuciAPI
```

## 🛠️ Development

### Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Setup

All NuGet dependencies are restored automatically by `dotnet restore`.

### Build

```bash
dotnet build NuciAPI.sln
```

### Run

```bash
dotnet run --project NuciAPI/NuciAPI.csproj
```

### Test

```bash
dotnet test NuciAPI.sln
```

### Release

```bash
dotnet pack NuciAPI/NuciAPI.csproj -c Release
```

### Dependencies

| Package | Purpose |
|---------|---------|
| NuciSecurity.HMAC | HMAC signing and validation implementation |

## 🗂️ Project Structure

The solution contains the subsequent projects:
- NuciAPI: Main reusable library containing request and response contracts
- NuciAPI.UnitTests: Unit test project for contract and response behaviour

The key directories inside `NuciAPI/` are:
| Directory | Purpose |
|-----------|---------|
| Requests  | Base request contracts and request-side HMAC functionality |
| Responses | Standard response models, messages, and response codes |

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so, please:
- Maintain cross-platform compatibility
- Maintain the existing public contract intact unless a breaking change is intentional
- Maintain the pull requests as focused and consistent with the existing code style
- Maintain your branch up-to-date with `master`
- Revise the documentation when behaviour changes
- Properly test all changes, including edge cases and error conditions
- Add unit tests for any new or changed functionality

## 🔗 Related Projects

- [NuciAPI.Controllers](https://github.com/hmlendea/nuciapi.controllers): Controller-focused extensions built on NuciAPI contracts
- [NuciAPI.Middleware](https://github.com/hmlendea/nuciapi.middleware): Middleware-oriented helpers for integrating NuciAPI in service pipelines
- [NuciAPI.Middleware.ExceptionHandling](https://github.com/hmlendea/nuciapi.middleware.exceptionhandling): Exception-handling middleware integrations
- [NuciAPI.Middleware.Logging](https://github.com/hmlendea/nuciapi.middleware.logging): Logging middleware integrations
- [NuciAPI.Middleware.Security](https://github.com/hmlendea/nuciapi.middleware.security): Security-focused middleware integrations

## 🔒 Security

For information on reporting security vulnerabilities, see [SECURITY.md](./SECURITY.md).

## 💝 Supporting the Project

Discovered a problem or have a suggestion? [Open an issue](https://github.com/hmlendea/nuciapi/issues)!

If you find this project useful, consider [funding it](https://hmlendea.go.ro/funding) or starring ⭐️ it on GitHub!

[![Donate](https://raw.githubusercontent.com/hmlendea/readme-assets/master/donate_generic.png)](https://hmlendea.go.ro/funding)

## 📄 License

This project is being distributed under the `GNU General Public License v3.0` or later.
See [LICENSE](./LICENSE) for further information.
