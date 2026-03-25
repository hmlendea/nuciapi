[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Latest GitHub release](https://img.shields.io/github/v/release/hmlendea/nuciapi)](https://github.com/hmlendea/nuciapi/releases/latest) [![Build Status](https://github.com/hmlendea/nuciapi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nuciapi/actions/workflows/dotnet.yml)

# NuciAPI

NuciAPI is a small .NET library for building consistent API contracts around two common concerns:

- strongly-typed request and response models
- HMAC signing and validation for payload integrity

It provides base classes for requests and responses, plus a set of standard success and error response helpers that can be reused across services.

# Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciAPI)

**.NET CLI**:
```bash
dotnet add package NuciAPI
```

**Package Manager**:
```powershell
Install-Package NuciAPI
```

# Features

- Base request type with built-in HMAC signing and validation
- Base response type with built-in HMAC signing and validation
- Standard success and error response models
- Reusable response codes and human-readable messages
- JSON-friendly response shape using `success`, `message`, `code`, and `hmac`

# Target framework

The package currently targets `.NET 10.0`.

# Package contents

## Requests

`NuciApiRequest` is the base type for API request models.

It provides:

- `SignHMAC(string secretKey)`
- `HasValidHMAC(string secretKey)`
- `ValidateHMAC(string secretKey)`
- `HmacToken`

The HMAC token itself is ignored by JSON serialization on the base request type, which is useful when the signature is transported outside the request body.

## Responses

`NuciApiResponse` is the base type for API responses.

It exposes:

- `IsSuccessful`
- `Message`
- `Code`
- `HmacToken`
- `SignHMAC(string secretKey)`
- `HasValidHMAC(string secretKey)`
- `ValidateHMAC(string secretKey)`

Two concrete response types are included:

- `NuciApiSuccessResponse`
- `NuciApiErrorResponse`

# Usage

## Define a request

```csharp
using NuciAPI.Requests;

public class CreateOrderRequest : NuciApiRequest
{
	public string CustomerId { get; set; }
	public decimal Total { get; set; }
}
```

## Sign and validate a request

```csharp
var secretKey = "super-secret-key";

var request = new CreateOrderRequest
{
	CustomerId = "CUST-001",
	Total = 149.99m
};

request.SignHMAC(secretKey);

bool isValid = request.HasValidHMAC(secretKey);

request.ValidateHMAC(secretKey);
```

## Return a success response

```csharp
using NuciAPI.Responses;

var response = NuciApiSuccessResponse.Default;
response.SignHMAC(secretKey);
```

## Return a standard error response

```csharp
using NuciAPI.Responses;

var response = NuciApiErrorResponse.NotFound;
response.SignHMAC(secretKey);
```

## Create a custom response type

```csharp
using NuciAPI.Responses;

public class OrderCreatedResponse : NuciApiResponse
{
	public OrderCreatedResponse(string orderId)
		: base("Order created successfully.", "ORDER_CREATED")
	{
		OrderId = orderId;
	}

	public override bool IsSuccessful => true;

	public string OrderId { get; }
}
```

# Built-in responses

## Success

- `NuciApiSuccessResponse.Default`
- `NuciApiSuccessResponse.FromMessage(string message)`

Default success payload values:

- message: `Operation completed successfully.`
- code: `SUCCESS`

## Errors

`NuciApiErrorResponse` includes a default response plus a set of common predefined errors:

- `Default`
- `AlreadyExists`
- `AlreadyProcessed`
- `AuthenticationFailure`
- `BadRequest`
- `ClientClosedTheRequest`
- `InternalServerError`
- `InvalidRequest`
- `NotFound`
- `NotImplemented`
- `ServiceDependencyUnavailable`
- `Timeout`
- `Unauthorised`

If you need a custom message while keeping the default error code, use:

```csharp
var response = NuciApiErrorResponse.FromMessage("The supplied payload is not acceptable.");
```

# Response shape

Responses are designed to serialize to a predictable structure similar to:

```json
{
  "success": false,
  "message": "The requested resource was not found.",
  "code": "NOT_FOUND",
  "hmac": "..."
}
```

# HMAC behavior

HMAC support is implemented through the `NuciSecurity.HMAC` package.

When signing:

- the token is generated from the object data and the provided secret key
- the token field itself is excluded from the HMAC calculation
- changing signed properties changes the generated token

This makes it suitable for detecting payload tampering between producer and consumer, as long as both sides share the same secret.

# Development

## Build

```bash
dotnet build NuciAPI.sln
```

## Test

```bash
dotnet test NuciAPI.sln
```

# License

This project is licensed under the `GPL-3.0-or-later` license. See `LICENSE` for details.
