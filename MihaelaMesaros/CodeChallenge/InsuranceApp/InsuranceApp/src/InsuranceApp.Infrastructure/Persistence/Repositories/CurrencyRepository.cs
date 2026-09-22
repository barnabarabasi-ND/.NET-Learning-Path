using InsuranceApp.Application.Abstractions.Persistence;
using InsuranceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence.Repositories;

internal sealed class CurrencyRepository(InsuranceDbContext dbContext) : ICurrencyRepository
{
    public async Task<IReadOnlyList<Currency>> GetCurrenciesAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Currencies
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);
    }

    public Task<Currency?> GetCurrencyForUpdateAsync(int currencyId, CancellationToken cancellationToken)
    {
        return dbContext.Currencies.FirstOrDefaultAsync(x => x.CurrencyId == currencyId, cancellationToken);
    }

    public Task<bool> CurrencyCodeExistsAsync(string code, int? excludeCurrencyId, CancellationToken cancellationToken)
    {
        return dbContext.Currencies.AnyAsync(
            x => x.Code == code && (!excludeCurrencyId.HasValue || x.CurrencyId != excludeCurrencyId.Value),
            cancellationToken
        );
    }

    public async Task AddCurrencyAsync(Currency currency, CancellationToken cancellationToken)
    {
        dbContext.Currencies.Add(currency);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task SaveCurrencyChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}