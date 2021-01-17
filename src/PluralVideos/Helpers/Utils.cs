using System;

namespace PluralVideos.Helpers
{
    public class Utils
    {
        private static readonly ConsoleColor color_default = Console.ForegroundColor;
        private static readonly object console_lock = new object();

        public static void WriteText(string text, bool newLine = true)
        {
            WriteToConsole(text, newLine: newLine);
        }

        public static void WriteRedText(string text, bool newLine = true)
        {
            WriteToConsole(text, ConsoleColor.Red, newLine);
        }

        public static void WriteYellowText(string text, bool newLine = true)
        {
            WriteToConsole(text, ConsoleColor.Yellow, newLine);
        }

        public static void WriteGreenText(string text, bool newLine = true)
        {
            WriteToConsole(text, ConsoleColor.Green, newLine);
        }

        public static void WriteBlueText(string text, bool newLine = true)
        {
            WriteToConsole(text, ConsoleColor.Blue, newLine);
        }

        public static void WriteCyanText(string text, bool newLine = true)
        {
            WriteToConsole(text, ConsoleColor.Cyan, newLine);
        }

        private static void WriteToConsole(string Text, ConsoleColor color = ConsoleColor.Gray, bool newLine = true)
        {
            lock (console_lock)
            {
                Console.ForegroundColor = color;
                Console.Write(Text);
                if (newLine) Console.Write(Environment.NewLine);
                Console.ForegroundColor = color_default;
            }
        }
    }
}