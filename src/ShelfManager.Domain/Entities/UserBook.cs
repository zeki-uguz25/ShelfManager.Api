namespace ShelfManager.Domain.Entities;

public class UserBook
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime ReturnDeadline { get; set; }
    public int? Rating { get; set; }
    public bool IsReturned { get; set; }
    public string? Comment { get; set; }

    public User User { get; set; } = null!;
    public Book Book { get; set; } = null!;
    public Fine? Fine { get; set; }
}
