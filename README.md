[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/nuciapi)](https://github.com/hmlendea/nuciapi/releases/latest)
[![Build Status](https://github.com/hmlendea/nuciapi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nuciapi/actions/workflows/dotnet.yml)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://gnu.org/licenses/gpl-3.0)

# NuciAPI

## About

NuciAPI is a small .NET library for building consistent API contracts around two common concerns:

- strongly-typed request and response models
- HMAC signing and validation for payload integrity

It provides base classes for requests and responses, plus a set of standard success and error response helpers that can be reused across services.

## Features

- Base request type with built-in HMAC signing and validation
- Base response type with built-in HMAC signing and validation
- Standard success and error response models
- Predefined success responses for common CRUD-style operations
- Reusable response codes and human-readable messages
- JSON-friendly response shape using `success`, `message`, `code`, and `hmac`

## Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciAPI)

### .NET CLI
```bash
dotnet add package NuciAPI
```

## Package Manager
```powershell
Install-Package NuciAPI
```

## Package contents

### Requests

`NuciApiRequest` is the base type for API request models.

It provides:

- `SignHMAC(string secretKey)`
- `HasValidHMAC(string secretKey)`
- `ValidateHMAC(string secretKey)`
- `HmacToken`

The HMAC token itself is ignored by JSON serialization on the base request type, which is useful when the signature is transported outside the request body.

### Responses

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

The library also exposes shared constants through:

- `NuciApiResponseCodes.SuccessCodes`
- `NuciApiResponseCodes.ErrorCodes`
- `NuciApiResponseMessages.SuccessMessages`
- `NuciApiResponseMessages.ErrorMessages`

## Usage

### Define a request

```csharp
using NuciAPI.Requests;

public class CreateOrderRequest : NuciApiRequest
{
	public string CustomerId { get; set; }
	public decimal Total { get; set; }
}
```

### Sign and validate a request

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

### Return a success response

```csharp
using NuciAPI.Responses;

var response = NuciApiSuccessResponse.Default;
response.SignHMAC(secretKey);
```

For common operations, you can use the built-in success helpers directly:

```csharp
var createdResponse = NuciApiSuccessResponse.Created;
var updatedResponse = NuciApiSuccessResponse.Updated;
var deletedResponse = NuciApiSuccessResponse.Deleted;
var fetchedResponse = NuciApiSuccessResponse.Fetched;
var notUpdatedResponse = NuciApiSuccessResponse.NotUpdated;
```

### Return a standard error response

```csharp
using NuciAPI.Responses;

var response = NuciApiErrorResponse.NotFound;
response.SignHMAC(secretKey);
```

### Create a custom response type

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

## Built-in responses

### Success

- `NuciApiSuccessResponse.Default`
- `NuciApiSuccessResponse.Created`
- `NuciApiSuccessResponse.Deleted`
- `NuciApiSuccessResponse.Fetched`
- `NuciApiSuccessResponse.NotUpdated`
- `NuciApiSuccessResponse.Updated`
- `NuciApiSuccessResponse.FromMessage(string message)`

Default success payload values:

- message: `Operation completed successfully.`
- code: `SUCCESS`

Built-in success payload values:

- `Created`: `The new resource was successfully created.` / `CREATED`
- `Deleted`: `The resource was successfully deleted.` / `DELETED`
- `Fetched`: `The resource was successfully fetched.` / `FETCHED`
- `NotUpdated`: `The resource was not updated, as it already has the same content.` / `NOT_UPDATED`
- `Updated`: `The resource was successfully updated.` / `UPDATED`

### Errors

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

### Shared constants

If you need to build custom response types while staying consistent with the built-in contracts, use the exported message and code constants:

```csharp
using NuciAPI.Responses;

var successCode = NuciApiResponseCodes.SuccessCodes.Created;
var successMessage = NuciApiResponseMessages.SuccessMessages.Created;

var errorCode = NuciApiResponseCodes.ErrorCodes.NotFound;
var errorMessage = NuciApiResponseMessages.ErrorMessages.NotFound;
```

## Response shape

Responses are designed to serialize to a predictable structure similar to:

```json
{
  "success": false,
  "message": "The requested resource was not found.",
  "code": "NOT_FOUND",
  "hmac": "..."
}
```

## HMAC behavior

HMAC support is implemented through the `NuciSecurity.HMAC` package.

When signing:

- the token is generated from the object data and the provided secret key
- the token field itself is excluded from the HMAC calculation
- changing signed properties changes the generated token

This makes it suitable for detecting payload tampering between producer and consumer, as long as both sides share the same secret.

## Development

### Build

```bash
dotnet build NuciAPI.sln
```

### Test

```bash
dotnet test NuciAPI.sln
```

## Target Framework

The current package targets `.NET 10.0`.

## License

This project is licensed under the `GNU General Public License v3.0` or later. See [LICENSE](./LICENSE) for details.