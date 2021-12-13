using System.Collections.Generic;
using System;
using DbControlCore.Helpers;
using DbControlCore.Models;

namespace DbControlCore.Services
{
    public static class GenerationService
    {
        private static string _databaseUsage = "DbControlCore.exe g [database|d] <name>";
        private static string _configUsage = "DbControlCore.exe g [config|c] <location>";

        public static void Generate(string[] args)
        {
            switch (args[1]?.ToLowerInvariant())
            {
                case "d":
                case "database":
                    if (args.Length < 3)
                        throw new ArgumentException($"Insufficient number of parameters. Correct usage is: {_databaseUsage}");

                    GenerateDatabaseFolder(args[2]);
                    break;


                case "c":
                case "config":
                    if (args.Length < 3)
                        throw new ArgumentException($"Insufficient number of parameters. Correct usage is: {_configUsage}");

                    GenerateConfigFile(args[2]);
                    break;
            }
        }


        private static void GenerateDatabaseFolder(string name)
        {
            ConsoleHelper.WriteInfo($"Creating folder for database '{name}' ...");

            var location = $"{Constants.Configurations.RootFolder}/{name}";

            if (!FileSystemHelper.CheckIfDirectoryIsEmpty(location))
                throw new Exception($"Couldn't create database, there's already a folder with the same name. Location: {location}");

            FileSystemHelper.CreateDirectory(location);

            GenerateConfigFile(location, name);

            ConsoleHelper.WriteSuccess($"Database '{name}' creation was successful.");
        }


        private static void GenerateConfigFile(string location, string name = null)
        {
            ConsoleHelper.WriteInfo($"Creating configuration file at location '{location}' ...");

            FileSystemHelper.CreateDirectory(location);

            var database = new ConfigModel
            {
                Name = name ?? FileSystemHelper.GetDirectoryName(location),
                Connection = "",
                IsEnabled = true
            };

            var fileLocation = FileSystemHelper.CombineLocationPath(location, Constants.Configurations.DatabaseConfigFileName);
            FileSystemHelper.CreateFile(fileLocation, JsonHelper.SerializeObject(database));


            ConsoleHelper.WriteSuccess($"Configuration file creation at '{location}' was successful.");
        }
    }
}
