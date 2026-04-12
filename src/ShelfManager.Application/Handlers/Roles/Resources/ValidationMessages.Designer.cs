namespace ShelfManager.Application.Handlers.Roles.Resources
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
                        "ShelfManager.Application.Handlers.Roles.Resources.ValidationMessages",
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

        public static string Name_Required => ResourceManager.GetString("Name_Required", resourceCulture)!;
        public static string Name_MaxLength => ResourceManager.GetString("Name_MaxLength", resourceCulture)!;
        public static string Code_Required => ResourceManager.GetString("Code_Required", resourceCulture)!;
    }
}
