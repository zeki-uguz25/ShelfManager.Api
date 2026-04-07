namespace ShelfManager.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Book> Books { get; set; } = [];
}
