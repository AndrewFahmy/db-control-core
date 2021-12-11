using System;
namespace DbControlCore.Helpers
{
    public static class ConsoleHelper
    {
        public static ConsoleColor _defaultForeGround;

        public static ConsoleColor _defaultBackGround;


        static ConsoleHelper()
        {
            _defaultForeGround = Console.ForegroundColor;
            _defaultBackGround = Console.BackgroundColor;
        }


        public static void WriteInfo(string message)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            Console.WriteLine(message);
            Console.WriteLine("");

            Console.ForegroundColor = _defaultForeGround;
            Console.BackgroundColor = _defaultBackGround;
        }

        public static void WriteWarning(string message)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine(message);
            Console.WriteLine("");

            Console.ForegroundColor = _defaultForeGround;
            Console.BackgroundColor = _defaultBackGround;
        }

        public static void WriteError(string message)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.WriteLine(message);
            Console.WriteLine("");

            Console.ForegroundColor = _defaultForeGround;
            Console.BackgroundColor = _defaultBackGround;
        }

        public static void WriteSuccess(string message)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine(message);
            Console.WriteLine("");

            Console.ForegroundColor = _defaultForeGround;
            Console.BackgroundColor = _defaultBackGround;
        }
    }
}
