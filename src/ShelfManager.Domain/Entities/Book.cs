using ShelfManager.Domain.Common;

namespace ShelfManager.Domain.Entities;

public class Book : AuditableEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int PageCount { get; set; }
    public string Author { get; set; } = null!;
    public int StockCount { get; set; }
    public int TotalCount { get; set; }
    public int PublishYear { get; set; }
    public string Publisher { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Language { get; set; }
    public Guid CategoryId { get; set; }
    public string? CoverImageUrl { get; set; }

    public Category Category { get; set; } = null!;
    public ICollection<UserBook> UserBooks { get; set; } = [];
}
