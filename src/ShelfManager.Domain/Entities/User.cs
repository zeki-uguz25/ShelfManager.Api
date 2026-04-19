using ShelfManager.Domain.Common;

namespace ShelfManager.Domain.Entities;

public class User : AuditableEntity
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public bool IsBanned { get; set; }
    public bool IsActive { get; set; }
    public string? Address { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<UserBook> UserBooks { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
    public ICollection<Fine> Fines { get; set; } = [];
}
