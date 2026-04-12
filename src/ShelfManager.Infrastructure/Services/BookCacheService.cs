using ShelfManager.Application.Abstractions.Services;
using StackExchange.Redis;

namespace ShelfManager.Infrastructure.Services;

public class BookCacheService : IBookCacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private const string Prefix = "ShelfManager:books_*";

    public BookCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task InvalidateAsync(CancellationToken cancellationToken = default)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
        //GetEndPoints().First() — baðlý olduðumuz Redis sunucusunun adresini alýr (localhost:6379).
        var keys = server.Keys(pattern: Prefix).ToArray();
        //Redis'te SCAN komutunu çalýþtýrýr — ShelfManager:books_* pattern'ine uyan tüm key'leri bulur.
        if (keys.Length > 0)
            await db.KeyDeleteAsync(keys);//Bulunan tüm keyler tek seferde silinir.
    }
}
