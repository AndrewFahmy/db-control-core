using System.Linq;
using System.IO;
using DbControlCore.Models;

namespace DbControlCore.Helpers
{
    public static class FileSystemHelper
    {
        public static string CombineLocationPath(params string[] locationParts)
        {
            return Path.Combine(locationParts);
        }


        public static DirectoryInfo[] GetDirectories(params string[] locationParts)
        {
            var location = CombineLocationPath(locationParts);

            var directory = new DirectoryInfo(location);

            return directory.GetDirectories();
        }

        public static bool CheckIfDirectoryExists(string location)
        {
            return Directory.Exists(location);
        }

        public static bool CheckIfDirectoryIsEmpty(string location)
        {
            if (!CheckIfDirectoryExists(location)) return true;

            return !Directory.EnumerateFileSystemEntries(location).Any();
        }

        public static bool CheckIfFileExists(string location)
        {
            return File.Exists(location);
        }

        public static string GetDirectoryName(string location)
        {
            return new DirectoryInfo(location).Name;
        }

        public static FileInfo[] GetDatabaseFiles(this DirectoryInfo directory)
        {
            return directory.GetFiles("*.sql", SearchOption.AllDirectories);
        }

        public static DatabaseModel ParseDatabaseConfigFile(this DirectoryInfo directory)
        {
            var fileInfo = directory
                            .GetFiles(Constants.Configurations.DatabaseConfigFileName, SearchOption.TopDirectoryOnly)
                            .FirstOrDefault();

            if (fileInfo == null) return null;

            return JsonHelper.DerializeObject<DatabaseModel>(fileInfo.GetFileInfoContent());
        }

        public static string GetFileInfoContent(this FileInfo file)
        {
            using (var reader = file.OpenText())
            {
                return reader.ReadToEnd();
            }
        }

        public static string GetFileContents(string location)
        {
            return File.ReadAllText(location);
        }

        public static void CreateFile(string location, string data)
        {
            DeleteFileIfExists(location);

            File.WriteAllText(location, data);
        }

        public static void CreateDirectory(string location)
        {
            if (CheckIfDirectoryExists(location)) return;

            Directory.CreateDirectory(location);
        }

        public static void DeleteFileIfExists(string location)
        {
            if (File.Exists(location))
                File.Delete(location);
        }
    }
}
