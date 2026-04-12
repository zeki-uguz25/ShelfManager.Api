namespace ShelfManager.Application.Constants;

public static class CacheKeys
{
    public static string Books(int pageNumber, int pageSize)
        => $"books_page{pageNumber}_size{pageSize}";

    public static string Categories(int pageNumber, int pageSize)
        => $"categories_page{pageNumber}_size{pageSize}";
}
