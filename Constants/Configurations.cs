namespace DbControlCore
{
    public static partial class Constants
    {
        public static class Configurations
        {
            public const string RootFolder = "./Databases";

            public const string DatabaseConfigFileName = "config.json";

            public const string JsonFileName = "./compiled.json";

            public const string BatchSeparatorPattern = @"\bgo\b";
        }
    }
}
