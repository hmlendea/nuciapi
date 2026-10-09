# Change Guide

## Modifying request/response base classes

### Adding a property to NuciApiRequest

**Impact:** All derived request types inherit the property; HMAC includes it automatically.

**Steps:**
1. Add property to `NuciApiRequest` in `NuciAPI/Requests/NuciApiRequest.cs`
2. Apply `[HmacOrder(n)]` if ordering matters
3. Update tests in `NuciApiRequestTests`
4. Consider serialization impact (property will appear in JSON)

### Adding a property to NuciApiResponse

**Impact:** All response types (success, error, content) inherit the property; HMAC includes it.

**Steps:**
1. Add property to `NuciApiResponse` in `NuciAPI/Responses/NuciApiResponse.cs`
2. Apply `[JsonPropertyName]` for JSON naming
3. Apply `[HmacOrder(n)]` — use high value (9999996+) to keep metadata last
4. Update `NuciApiSuccessResponse`, `NuciApiErrorResponse`, `NuciApiContentResponse<T>` if needed
5. Update serialisation tests

### Changing HMAC property order

**Impact:** Breaks HMAC compatibility — existing signed requests/responses will fail validation.

**Steps:**
1. Only change if absolutely necessary
2. Document as breaking change
3. Bump major version
4. Provide migration guide for consumers

## Modifying response factories

### Adding a new success factory

**Location:** `NuciApiSuccessResponse.cs`

**Steps:**
1. Add constants to `NuciApiResponseCodes.SuccessCodes` and `NuciApiResponseMessages.SuccessMessages`
2. Add static property to `NuciApiSuccessResponse`
3. Follow naming pattern: `PascalCase` property, descriptive message

### Adding a new error factory

**Location:** `NuciApiErrorResponse.cs`

**Steps:**
1. Add constants to `NuciApiResponseCodes.ErrorCodes` and `NuciApiResponseMessages.ErrorMessages`
2. Add static property to `NuciApiErrorResponse`
3. Follow naming pattern

## Modifying NuciApiContentResponse<T>

**Constraints:** `TContent : NuciApiResponseContent` — cannot be removed without breaking generic constraint.

**Safe changes:**
- Add constructors
- Add factory methods

**Breaking changes:**
- Change generic constraint
- Remove constructors
- Change property types

## Upgrading NuciSecurity.HMAC

**Risk:** HMAC algorithm/token format changes break compatibility.

**Steps:**
1. Update version in `NuciAPI.csproj`
2. Run full test suite
3. Verify HMAC tokens generated with old version validate with new (or document incompatibility)
4. Test with consumer applications
5. Bump version appropriately (patch if compatible, major if not)

## Changing target framework

**Current:** net10.0

**Steps:**
1. Update `<TargetFramework>` in `NuciAPI.csproj` and `NuciAPI.UnitTests.csproj`
2. Update GitHub Actions workflows (`.github/workflows/*.yml`) — `dotnet-version`
3. Update README.md requirements
4. Test on new framework
5. Consider multi-targeting if supporting multiple versions

## Adding new public API

**Checklist:**
- [ ] Add XML documentation comments
- [ ] Add unit tests
- [ ] Update API reference documentation
- [ ] Consider HMAC implications (attributes, ordering)
- [ ] Verify serialisation shape
- [ ] Update version (minor for additions, major for breaking)

## Removing public API

**Process:**
1. Mark `[Obsolete]` with message and version
2. Wait one minor release cycle
3. Remove in next major version
4. Update documentation

## Serialisation changes

**Risk:** Breaks JSON compatibility with consumers.

**Safe:**
- Add optional properties (nullable, default values)
- Add `[JsonPropertyName]` aliases

**Breaking:**
- Remove properties
- Change property types
- Change property names (without alias)
- Change `JsonNamingPolicy`

## HMAC attribute changes

**Risk:** Breaks HMAC validation for existing signed payloads.

**Never change without major version bump:**
- `[HmacOrder]` values on base class properties
- `[HmacIgnore]` on `HmacToken`
- Property inclusion logic

## Testing changes

**Before committing:**
```bash
dotnet build
dotnet test
```

**Before releasing:**
- All tests pass
- No compiler warnings
- Package builds (`dotnet pack -c Release`)
- Manual verification with consumer app if possible