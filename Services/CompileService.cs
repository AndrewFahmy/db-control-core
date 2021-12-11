using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DbControlCore.Helpers;
using DbControlCore.Models;

namespace DbControlCore.Services
{
    public static class CompileService
    {
        public static void Compile(string[] args)
        {
            if (!FileSystemHelper.CheckIfDirectoryExists(Constants.Configurations.RootFolder))
            {
                throw new DirectoryNotFoundException("Couldn't find root directory.");
            }

            ConsoleHelper.WriteInfo("Starting script compilation ...");

            var includeSkippedFiles = args.Any(a =>
                    "--include-skipped".Equals(a, StringComparison.OrdinalIgnoreCase) ||
                    "-S".Equals(a, StringComparison.OrdinalIgnoreCase));

            var directories = FileSystemHelper.GetDirectories(Constants.Configurations.RootFolder);
            var data = new List<DatabaseModel>();
            var startTime = DateTime.Now;

            foreach (var directory in directories)
            {
                var database = directory.ParseDatabaseConfigFile();

                if (database != null)
                {
                    if (database.IsEnabled)
                    {
                        ConsoleHelper.WriteInfo($"Compiling scripts for database: {database.Name} {(includeSkippedFiles ? "(including skipped files)" : string.Empty)}...");

                        database.Queries = CompileDatabase(directory, includeSkippedFiles);

                        data.Add(database);
                    }
                    else
                    {
                        ConsoleHelper.WriteWarning($"Database {database.Name} is not enabled, skipping compilation.");
                    }
                }
                else
                {
                    ConsoleHelper.WriteWarning($"Couldn't find a config file for directory {directory.Name}. Directory will be skipped.");
                }
            }

            FileSystemHelper.DeleteFileIfExists(Constants.Configurations.JsonFileName);

            FileSystemHelper.CreateFile(Constants.Configurations.JsonFileName, JsonHelper.SerializeObject(data));

            ConsoleHelper.WriteSuccess($"Application has finished compiling {directories.Length} folder(s) in {(DateTime.Now - startTime).TotalSeconds} seconds.");
        }

        private static List<QueryModel> CompileDatabase(DirectoryInfo directory, bool includeSkippedFiles)
        {
            var queries = new List<QueryModel>();
            var allFiles = directory.GetDatabaseFiles();
            List<(string fileName, FileInfo file)> information;

            if (includeSkippedFiles)
            {
                information = allFiles.Select(s => (fileName: s.Name.Replace("[Skip]", string.Empty), file: s))
                    .OrderBy(o => o.fileName)
                    .ToList();
            }
            else
            {
                information = allFiles
                    .Where(p => p.Name.IndexOf("[Skip]", StringComparison.OrdinalIgnoreCase) < 0)
                    .Select(s => (fileName: s.Name.Replace("[Skip]", string.Empty), file: s))
                    .OrderBy(o => o.fileName)
                    .ToList();
            }

            foreach (var info in information)
            {
                var directoryName = FileSystemHelper.GetDirectoryName(info.file.DirectoryName);

                if (!directory.Name.Equals(directoryName, StringComparison.OrdinalIgnoreCase))
                {
                    directoryName = $"{directory.Name}/{directoryName}";
                }

                queries.Add(new QueryModel
                {
                    Name = $"{directoryName}/{info.fileName}",
                    Text = info.file.GetFileInfoContent()
                });
            }

            return queries;
        }
    }
}
