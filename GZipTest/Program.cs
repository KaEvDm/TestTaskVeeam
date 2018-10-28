using System;

namespace GZipTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += ExceptionHandler;
            Console.CancelKeyPress += CancelKeyHandler;
            new Application(new Parameters(args)).Run();

            Console.ReadKey();
        }

        private static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Console.WriteLine((args.ExceptionObject as Exception).Message);
            Environment.Exit(1);
        }

        private static void CancelKeyHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Прервано пользователем.");
            Environment.Exit(1);
        }
    }
}
