# NuciApiResponseContent

**Namespace:** `NuciAPI.Responses`
**Assembly:** `NuciAPI.dll`
**Base class:** `object` (abstract)

## Purpose

Abstract base class for response payload content. Enables HMAC participation via `[HmacOrder]` attributes on derived properties.

## Syntax

```csharp
public abstract class NuciApiResponseContent
{
}
```

## Usage

Derive from this class to create typed response content:

```csharp
using NuciAPI.Responses;
using NuciSecurity.HMAC;

public class OrderResponseContent : NuciApiResponseContent
{
    [HmacOrder(1)]
    public string OrderId { get; set; }

    [HmacOrder(2)]
    public decimal Total { get; set; }

    [HmacOrder(3)]
    public DateTime CreatedAt { get; set; }
}

var content = new OrderResponseContent
{
    OrderId = "ORD-123",
    Total = 99.99m,
    CreatedAt = DateTime.UtcNow
};

var response = new NuciApiContentResponse<OrderResponseContent>(content);
response.SignHMAC("secret");
```

## HMAC participation

- All public instance properties of derived classes are included in HMAC computation
- Apply `[HmacOrder(n)]` to control inclusion order (ascending)
- Properties without `[HmacOrder]` default to order 0
- Properties marked `[HmacIgnore]` are excluded

## Constraint

Used as generic constraint for `NuciApiContentResponse<TContent>`:
```csharp
where TContent : NuciApiResponseContent
```

This ensures all content types participate in HMAC.

## Tests

- `NuciApiContentResponseTests` (uses `DummyResponseContent` which inherits from `NuciApiResponseContent`)