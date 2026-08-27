namespace MiniStoreDemo.Application.Abstractions.Persistence
{
    public interface ICategoryRepository
    {
        Task<bool> CheckCategoryExistsAsync(int categoryId, CancellationToken cancellationToken);
    }
}
