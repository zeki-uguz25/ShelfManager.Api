namespace ShelfManager.Application.Handlers.UserRoles.Resources
{
    public class ValidationMessages
    {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;

        public static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    resourceMan = new global::System.Resources.ResourceManager(
                        "ShelfManager.Application.Handlers.UserRoles.Resources.ValidationMessages",
                        typeof(ValidationMessages).Assembly);
                }
                return resourceMan;
            }
        }

        public static global::System.Globalization.CultureInfo Culture
        {
            get { return resourceCulture; }
            set { resourceCulture = value; }
        }

        public static string UserId_Required => ResourceManager.GetString("UserId_Required", resourceCulture)!;
        public static string RoleId_Required => ResourceManager.GetString("RoleId_Required", resourceCulture)!;
    }
}
