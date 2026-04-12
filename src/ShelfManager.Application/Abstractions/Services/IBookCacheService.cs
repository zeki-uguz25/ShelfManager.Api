namespace ShelfManager.Application.Abstractions.Services;

public interface IBookCacheService
{
    Task InvalidateAsync(CancellationToken cancellationToken = default);
}
