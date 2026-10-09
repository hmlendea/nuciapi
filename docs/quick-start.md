# Quick Start

## Install

```bash
dotnet add package NuciAPI
```

## Define a request

```csharp
using NuciAPI.Requests;

public sealed class GetUserRequest : NuciApiRequest
{
    public int UserId { get; set; }
}
```

## Define a response content

```csharp
using NuciAPI.Responses;

public sealed class UserContent : NuciApiResponseContent
{
    public string Name { get; set; }
    public string Email { get; set; }
}
```

## Define a response

```csharp
using NuciAPI.Responses;

public sealed class GetUserResponse : NuciApiContentResponse<UserContent>
{
}
```

## Sign a request (client)

```csharp
var request = new GetUserRequest { UserId = 42 };
var secretKey = Convert.FromBase64String("your-base64-secret-key");

request.SignHMAC(secretKey);
// request.HmacToken now contains the signature
// Send request as JSON
```

## Validate a request (server)

```csharp
var request = JsonSerializer.Deserialize<GetUserRequest>(json);
var secretKey = Convert.FromBase64String("your-base64-secret-key");

if (!request.HasValidHMAC(secretKey))
{
    return new NuciApiErrorResponse.AuthenticationFailure();
}

var user = await _userService.GetAsync(request.UserId);
return new GetUserResponse
{
    Content = new UserContent { Name = user.Name, Email = user.Email }
};
```

## Sign a response (server)

```csharp
var response = new GetUserResponse
{
    Content = new UserContent { Name = "John", Email = "john@example.com" }
};

response.SignHMAC(secretKey);
// response.HmacToken now contains the signature
// Return response as JSON
```

## Validate a response (client)

```csharp
var response = JsonSerializer.Deserialize<GetUserResponse>(json);
var secretKey = Convert.FromBase64String("your-base64-secret-key");

if (!response.HasValidHMAC(secretKey))
{
    throw new InvalidOperationException("Invalid response signature");
}

var user = response.Content; // UserContent with Name, Email
```

## Factory properties

```csharp
// Success responses
var ok = NuciApiSuccessResponse.Default;
var created = NuciApiSuccessResponse.Created;
var deleted = NuciApiSuccessResponse.Deleted;
var fetched = NuciApiSuccessResponse.Fetched;
var notUpdated = NuciApiSuccessResponse.NotUpdated;
var updated = NuciApiSuccessResponse.Updated;

// Error responses
var error = NuciApiErrorResponse.Default;
var notFound = NuciApiErrorResponse.NotFound;
var badRequest = NuciApiErrorResponse.BadRequest;
var authFailure = NuciApiErrorResponse.AuthenticationFailure();
var alreadyExists = NuciApiErrorResponse.AlreadyExists();
var alreadyProcessed = NuciApiErrorResponse.AlreadyProcessed();
var internalError = NuciApiErrorResponse.InternalServerError();
var invalidRequest = NuciApiErrorResponse.InvalidRequest();
var clientClosed = NuciApiErrorResponse.ClientClosedTheRequest();
```

## Custom message/code

```csharp
var custom = NuciApiSuccessResponse.FromMessage("Operation completed", "CUSTOM_001");
var customError = NuciApiErrorResponse.FromMessage("Rate limited", "RATE_LIMITED");
```

## Next steps

- [Architecture](./architecture.md) — Understand the design
- [Request model](./components/request-model.md) — Request details
- [Response model](./components/response-model.md) — Response hierarchy
- [HMAC integration](./components/hmac-integration.md) — Signing/validation internals
- [API reference](./api-reference/INDEX.md) — Complete API surface
- [Examples](./api-usage-examples.md) — More usage patterns