using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Domain.Enums
{
    public enum Roles
    {
        [Description("{\"En\":\"Admin\",\"Tr\":\"Sistem Yöneticisi\"}")]
        Admin = 1,

        [Description("{\"En\":\"System Moderator\",\"Tr\":\"Sistem Moderatörü\"}")]
        SystemModerator = 2,

        [Description("{\"En\":\"Member\",\"Tr\":\"Üye\"}")]
        Member = 3,
    }
}
