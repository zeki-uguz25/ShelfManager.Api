using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Domain.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!; // "books.create", "users.ban" gibi
        public string Name { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }

}
