using System;
using DbControlCore.Helpers;
using DbControlCore.Services;

namespace DbControlCore
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (args == null || args.Length <= 0)
                    throw new ArgumentException("No arguments were found, the application is terminating...");

                switch (args[0]?.ToLowerInvariant())
                {
                    case "c":
                    case "compile":
                        CompileService.Compile(args);
                        break;

                    case "e":
                    case "exec":
                        ExecutionService.Execute(args);
                        break;

                    case "g":
                    case "generate":
                        GenerationService.Generate(args);
                        break;

                    case "u":
                    case "update":
                        UpdateInfoService.Update(args);
                        break;

                    default:
                        throw new ArgumentException("Invalid command. Applicatin only supports these commands: [compile|c], [exec|e], [generate|g], [update|u]");
                }

                return 0;
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError(ex.Message);

                return 1;
            }
        }
    }
}
