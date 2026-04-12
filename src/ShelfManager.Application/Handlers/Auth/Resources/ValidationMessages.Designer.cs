namespace ShelfManager.Application.Handlers.Auth.Resources
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
                        "ShelfManager.Application.Handlers.Auth.Resources.ValidationMessages",
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

        public static string FullName_Required => ResourceManager.GetString("FullName_Required", resourceCulture)!;
        public static string FullName_MaxLength => ResourceManager.GetString("FullName_MaxLength", resourceCulture)!;
        public static string Email_Required => ResourceManager.GetString("Email_Required", resourceCulture)!;
        public static string Email_Invalid => ResourceManager.GetString("Email_Invalid", resourceCulture)!;
        public static string Password_Required => ResourceManager.GetString("Password_Required", resourceCulture)!;
        public static string Password_MinLength => ResourceManager.GetString("Password_MinLength", resourceCulture)!;
    }
}
