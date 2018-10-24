using System;
using System.IO;
using System.Linq;

namespace GZipTest
{
    public class Parameters
    {
        public const int Megabyte = 1024 * 1024;
        public readonly ProcessMode Mode;
        public readonly bool IsNeedMultithreading;
        public readonly string PathToSourceFile;
        public readonly string PathToResultFile;
        public readonly long SourceFileSize;
        public readonly int ProcessorCount;
        public readonly IHandler Handler;

        public Parameters(string[] args)
        {
            Validation(args);

            Mode = (ProcessMode)Enum.Parse(typeof(ProcessMode), args[0]);
            PathToSourceFile = args[1];
            PathToResultFile = args[2];

            SourceFileSize = new FileInfo(PathToSourceFile).Length;
            ProcessorCount = Environment.ProcessorCount;

            if (ProcessorCount == 1 || SourceFileSize < 2 * ProcessorCount * Megabyte)
            {
                IsNeedMultithreading = false;
            }
            else
            {
                IsNeedMultithreading = true;
            }

            Handler = HandlerSelection();
        }

        private static void Validation(string[] args)
        {
            if (args.Count() != 3)
            {
                throw new ArgumentException("Некорректное количество параметров!");
            }

            ModeCheckDialog(args[0]);
            PathCheck(args[1]);
            PathCheck(args[2]);

            if (!File.Exists(args[1]))
            {
                throw new ArgumentException($"{args[1]} - файл не существует!");
            }
            if (File.Exists(args[2]))
            {
                RewriteFileDialog(args[2]);
            }
        }


        private static void ModeCheckDialog(string mode)
        {
            mode = mode.ToLower();

            while (mode != "compress" && mode != "decompress")
            {
                Console.WriteLine($"У программы нет режима {mode}.");
                Console.WriteLine("Программа работает в двух режимах compress или decompress!");
                Console.Write("Введите желаемый режим: ");
                mode = Console.ReadLine();
            }
        }

        private static void PathCheck(string path)
        {
            var a = Path.GetInvalidPathChars();
            if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException($"{path} - некорректный путь к файлу!");
        }

        private static void RewriteFileDialog(string path)
        {
            Console.WriteLine($"{path} - файл существует!");
            Console.WriteLine("Перезаписать?(введите: да/нет)");

            var answer = Console.ReadLine();

            while (answer != "да" && answer != "нет")
            {
                Console.WriteLine("Перезаписать существующий файл?(введите: да/нет)");
                answer = Console.ReadLine();
            }

            if (answer == "нет")
                throw new ArgumentException($"{path} - неверный файл.");
        }

        public IHandler HandlerSelection()
        {
            var factory = new ArchiverFactory();
            switch (Mode)
            {
                case ProcessMode.compress:
                {
                    return factory.CreateHandler();
                }
                case ProcessMode.decompress:
                {
                    return factory.CreateDehandler();
                }
                default:
                {
                    throw new Exception("Неверный режим работы!");
                }
            }
        }
    }
}
