using ShelfManager.Domain.Common;

namespace ShelfManager.Domain.Entities;

public class Notification : AuditableEntity
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; }

    public User User { get; set; } = null!;
}
