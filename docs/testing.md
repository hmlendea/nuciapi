# Testing

## Test strategy

| Level | Framework | Coverage | Location |
|-------|-----------|----------|----------|
| Unit | NUnit 4.3.2 | All public types, HMAC flows | `NuciAPI.UnitTests/` |
| Integration | None | N/A | N/A |
| Contract | None | N/A | N/A |
| Performance | None | N/A | N/A |

## Test organisation

```
NuciAPI.UnitTests/
├── Helpers/
│   ├── DummyRequest.cs          # Request with properties for HMAC testing
│   ├── EmptyRequest.cs          # Empty request for HMAC comparison
│   ├── DummyResponse.cs         # Success response with content
│   ├── EmptyResponse.cs         # Empty response for HMAC comparison
│   └── DummyResponseContent.cs  # Content with [HmacOrder]
├── NuciApiRequestTests.cs       # Request HMAC tests
├── NuciApiResponseTests.cs      # Response HMAC tests
├── NuciApiSuccessResponseTests.cs  # Success response tests
├── NuciApiErrorResponseTests.cs    # Error response tests
└── NuciApiContentResponseTests.cs  # Generic content response tests
```

## Test naming convention

```
Given<Context>_When<Action>_Then<Outcome>
```

Examples:
- `GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated`
- `GivenAnErrorResponse_WhenSerialising_ThenTheContentPropertyIsAbsent`

## Test execution

```bash
# Run all tests
dotnet test NuciAPI.sln

# Run with verbosity
dotnet test NuciAPI.sln --verbosity normal

# Run specific test class
dotnet test NuciAPI.UnitTests/NuciAPI.UnitTests.csproj --filter "FullyQualifiedName~NuciApiRequestTests"

# Run specific test
dotnet test NuciAPI.UnitTests/NuciAPI.UnitTests.csproj --filter "FullyQualifiedName~GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated"
```

## CI integration

`.github/workflows/dotnet.yml` runs tests on every push/PR to master:

```yaml
- name: Test
  run: dotnet test --no-build --verbosity normal
```

## Coverage areas

### HMAC signing/validation

| Test | Verifies |
|------|----------|
| `NuciApiRequestTests.GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated` | `SignHMAC` populates `HmacToken` |
| `NuciApiRequestTests.GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenWasBuiltUsingAllProperties` | Different properties → different tokens |
| `NuciApiResponseTests.GivenAResponse_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated` | Response signing works |
| `NuciApiResponseTests.GivenAResponse_WhenSigningTheHmac_ThenTheHmacTokenWasBuiltUsingAllProperties` | Response content affects token |

### Response factories

| Test | Verifies |
|------|----------|
| `NuciApiSuccessResponseTests.GivenASuccessResponse_WhenGettingTheIsSuccessfulProperty_ThenTrueIsReturned` | `IsSuccessful == true` |
| `NuciApiSuccessResponseTests.GivenASuccessResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedMessageIsUsed` | Default message |
| `NuciApiSuccessResponseTests.GivenASuccessResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedCodeIsUsed` | Default code |
| `NuciApiSuccessResponseTests.GivenASuccessResponse_WhenCreatingTheDefaultResponse_ThenTheContentIsNull` | Content null by default |
| `NuciApiErrorResponseTests.GivenAnErrorResponse_WhenGettingTheIsSuccessfulProperty_ThenFalseIsReturned` | `IsSuccessful == false` |
| `NuciApiErrorResponseTests.GivenAnErrorResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedMessageIsUsed` | Default error message |
| `NuciApiErrorResponseTests.GivenAnErrorResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedCodeIsUsed` | Default error code |

### Serialisation

| Test | Verifies |
|------|----------|
| `NuciApiSuccessResponseTests.GivenASuccessResponse_WhenSerialising_ThenTheMessageAndCodePropertiesAreAtTheRootLevel` | JSON shape |
| `NuciApiErrorResponseTests.GivenAnErrorResponse_WhenSerialising_ThenTheContentPropertyIsAbsent` | No content in error JSON |
| `NuciApiContentResponseTests.GivenContent_WhenSerialisingAResponse_ThenTheContentPropertiesAreSerialised` | Content serialised |
| `NuciApiContentResponseTests.GivenContent_WhenDeserialisingAResponse_ThenTheContentPropertiesAreDeserialised` | Round-trip deserialisation |

### Content responses

| Test | Verifies |
|------|----------|
| `NuciApiContentResponseTests.GivenContent_WhenCreatingAResponse_ThenTheContentIsRetained` | Content reference preserved |
| `NuciApiContentResponseTests.GivenContent_WhenCreatingAResponse_ThenTheDefaultMetadataIsUsed` | Default message/code |

## Test helpers

### DummyRequest

```csharp
public class DummyRequest : NuciApiRequest
{
    public string DummyProperty { get; set; }
}
```

### EmptyRequest

```csharp
public class EmptyRequest : NuciApiRequest { }
```

### DummyResponse

```csharp
public class DummyResponse(string message) : NuciApiSuccessResponse(message, "DUMMY_CODE")
{
    public override NuciApiResponseContent Content { get; set; } = new DummyResponseContent();
}
```

### EmptyResponse

```csharp
public class EmptyResponse(string message) : NuciApiSuccessResponse(message, "TEST_CODE")
{
    public override NuciApiResponseContent Content { get; set; }
}
```

### DummyResponseContent

```csharp
public sealed class DummyResponseContent : NuciApiResponseContent
{
    [HmacOrder(1)]
    public string DummyProperty { get; set; }
}
```

## Gaps and recommendations

### Missing test coverage

1. **HMAC validation failure paths** — No tests for `ValidateHMAC` throwing
2. **Deserialisation error handling** — No tests for malformed JSON
3. **Edge cases** — Null content, empty collections, nested objects
4. **Property ordering** — Explicit `[HmacOrder]` vs default ordering
5. **Inheritance chains** — Multi-level request/response inheritance

### Recommended additions

```csharp
// HMAC validation failure
[Test]
public void GivenInvalidHmac_WhenValidating_ThenThrowsHmacValidationException()

// Deserialisation errors
[Test]
public void GivenMalformedJson_WhenDeserialising_ThenThrowsJsonException()

// Property ordering
[Test]
public void GivenSamePropertiesDifferentOrder_WhenSigning_ThenDifferentTokens()

// Null handling
[Test]
public void GivenNullContent_WhenCreatingContentResponse_ThenThrowsArgumentNullException()
```

## Running tests locally

```bash
# Restore, build, test
dotnet restore
dotnet build
dotnet test

# Watch mode (requires dotnet-watch)
dotnet watch test
```

## Test maintenance

- Tests are in `NuciAPI.UnitTests` project (not packed)
- Add tests for new public API surface
- Update tests when changing HMAC behaviour
- Keep test helpers minimal and focused