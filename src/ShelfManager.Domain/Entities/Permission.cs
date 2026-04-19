using ShelfManager.Domain.Common;

namespace ShelfManager.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string Code { get; set; } = null!; // "books.create", "users.ban" gibi
        public string Name { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }

}
