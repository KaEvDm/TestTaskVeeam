using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public class ProgressBar
    {
        private static readonly object mutex = new object();
        private double onePercent;
        private int cursorTopPosition;
        private int cursorLeftPosition;
        int MaxBlocks;

        public ProgressBar(long fileSize)
        {
            MaxBlocks = (int)(fileSize / Parameters.Megabyte);
            onePercent = MaxBlocks / 100;
        }

        public void Run()
        {
            lock (mutex)
            {
                cursorTopPosition = Console.CursorTop;
                cursorLeftPosition = Console.CursorLeft;

                Console.Write("[");
                for (int i = 0; i < 100; i++)
                {
                    Console.Write('.');
                }
                Console.Write("]:0%");
                Console.WriteLine();
            }
        }

        public void Update(int blocksReady)
        {
            lock (mutex)
            {
                var tempCursorTopPosition = Console.CursorTop;
                var tempCursorLeftPosition = Console.CursorLeft;

                Console.SetCursorPosition(cursorLeftPosition, cursorTopPosition);

                double procentsReady = blocksReady / onePercent;

                Console.Write("[");
                for (int i = 0; i < 100; i++)
                {
                    if (i < procentsReady)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('|');
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.Write($"]:{blocksReady/onePercent}%");

                Console.SetCursorPosition(tempCursorLeftPosition, tempCursorTopPosition);
            }
        }
    }
}
