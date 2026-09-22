using InsuranceApp.Application.Common;
using InsuranceApp.Application.DTOs.Currency;

namespace InsuranceApp.Application.Abstractions.Services;

public interface ICurrencyService
{
    Task<Result<IReadOnlyList<CurrencyDto>>> GetCurrenciesAsync(CancellationToken cancellationToken);

    Task<Result<CurrencyDto>> CreateCurrencyAsync(CreateCurrencyDto createCurrencyDto, CancellationToken cancellationToken);

    Task<Result<CurrencyDto>> UpdateCurrencyAsync(int currencyId, UpdateCurrencyDto updateCurrencyDto, CancellationToken cancellationToken);
}