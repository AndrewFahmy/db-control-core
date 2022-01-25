using System.IO;
using System;
using DbControlCore.Helpers;
using DbControlCore.Models;

namespace DbControlCore.Services
{
    public static class UpdateInfoService
    {
        private static string _connectionUsage = "db-control.exe u [connection|c] <name> <value>";
        private static string _statusUsage = "db-control.exe u [status|s] <name> <true/false>";

        public static void Update(string[] args)
        {
            switch (args[1]?.ToLowerInvariant())
            {
                case "c":
                case "connection":
                    if (args.Length < 4)
                        throw new ArgumentException($"Insufficient number of parameters. Correct usage is: {_connectionUsage}");

                    UpdateConnectionStirng(args[2], args[3]);
                    break;


                case "s":
                case "status":
                    if (args.Length < 4)
                        throw new ArgumentException($"Insufficient number of parameters. Correct usage is: {_statusUsage}");

                    UpdateStatus(args[2], args[3]);
                    break;
                
                default:
                    throw new Exception("Invalid parameter, the update command supports only these actions: [connection|c], [status|s]");
            }
        }

        
        private static void UpdateConnectionStirng(string name, string connection)
        {
            ConsoleHelper.WriteInfo($"Updating connection string of database '{name}' ...");

            var location = $"{Constants.Configurations.RootFolder}/{name}/{Constants.Configurations.DatabaseConfigFileName}";

            if (!FileSystemHelper.CheckIfFileExists(location))
                throw new FileNotFoundException($"Couldn't find the database configuration file at provided location: {location}");

            var data = FileSystemHelper.GetFileContents(location);
            var config = JsonHelper.DerializeObject<ConfigModel>(data);

            config.Connection = connection;

            FileSystemHelper.CreateFile(location, JsonHelper.SerializeObject(config));

            ConsoleHelper.WriteSuccess($"Database '{name}' connection was updated successfully.");
        }


        private static void UpdateStatus(string name, string status)
        {
            ConsoleHelper.WriteInfo($"Updating status of database '{name}' ...");

            var location = $"{Constants.Configurations.RootFolder}/{name}/{Constants.Configurations.DatabaseConfigFileName}";

            if (!FileSystemHelper.CheckIfFileExists(location))
                throw new FileNotFoundException($"Couldn't find the database configuration file at provided location: {location}");

            var data = FileSystemHelper.GetFileContents(location);
            var config = JsonHelper.DerializeObject<ConfigModel>(data);

            config.IsEnabled = bool.Parse(status);

            FileSystemHelper.CreateFile(location, JsonHelper.SerializeObject(config));

            ConsoleHelper.WriteSuccess($"Database '{name}' status was updated successfully.");
        }
    
    }
}
