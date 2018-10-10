using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public static class Parameters
    {
        public const int Megabyte = 1024 * 1024;
        public static ProcessMode Mode;
        public static string PathToSourceFile;
        public static string PathToResultFile;
        public static Func<Block, ProcessedBlock> Process;

        public static void Parse(string[] args)
        {
            if (args.Count() != 3)
            {
                throw new ArgumentException("Некорректное количество параметров!");
            }

            ModeCheckDialog(args[0]);
            Mode = (ProcessMode)Enum.Parse(typeof(ProcessMode), args[0]);
            ProcessСhoice();

            PathCheck(args[1]);
            PathCheck(args[2]);
            PathToSourceFile = args[1];
            PathToResultFile = args[2];

            if (!File.Exists(PathToSourceFile))
            {
                throw new ArgumentException($"{PathToSourceFile} - файл не существует!");
            }
            if (File.Exists(PathToResultFile))
            {
                RewriteFileDialog(PathToResultFile);
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

        private static void ProcessСhoice()
        {
            switch (Mode)
            {
                case ProcessMode.compress:
                {
                    Process = Compressor.CompressBlock;
                    break;
                }
                case ProcessMode.decompress:
                {
                    Process = Compressor.DecompressBlock;
                    break;
                }
            }
        }
    }
}
