namespace InsuranceApp.Application.DTOs.Currency;

public sealed record CurrencyDto(
    int CurrencyId,
    string Code,
    string Name,
    decimal ExchangeRateToBase,
    bool IsActive
);