using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Domain.Constants
{
    public static class Permissions
    {
        public static class Books
        {
            public const string Read = "books.read";
            public const string Create = "books.create";
            public const string Update = "books.update";
            public const string Delete = "books.delete";
        }

        public static class Categories
        {
            public const string Manage = "categories.manage";
        }

        public static class Users
        {
            public const string Ban = "users.ban";
            public const string GetUser = "users.get";
        }

        public static class Roles
        {
            public const string Manage = "roles.manage";
        }

        public static class UserBooks
        {
            public const string Borrow = "userbooks.borrow";
            public const string Return = "userbooks.return";
        }

        public static class Fines
        {
            public const string Pay = "fines.pay";
        }

        public static class Notifications
        {
            public const string Read = "notifications.read";
        }
    }

}
