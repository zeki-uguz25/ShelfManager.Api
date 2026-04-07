namespace ShelfManager.Domain.Entities;

public class Fine
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid UserBookId { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public UserBook UserBook { get; set; } = null!;
}
