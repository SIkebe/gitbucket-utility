using System;

namespace GitBucket.Core
{
    /// <summary>
    /// The default console implementation.
    /// </summary>
    public sealed class GbUtilConsole : IConsole
    {
        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public void Write(string value)
        {
            var color = ForegroundColor;
            ResetColor();
            Console.Write(value);
            ForegroundColor = color;
        }

        public void WriteLine(string value)
        {
            var color = ForegroundColor;
            ResetColor();
            Console.WriteLine(value);
            ForegroundColor = color;
        }

        public void WriteWarn(string value)
        {
            var color = ForegroundColor;
            ForegroundColor = ConsoleColor.Yellow;
            Console.Write(value);
            ResetColor();
        }

        public void WriteWarnLine(string value)
        {
            var color = ForegroundColor;
            ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(value);
            ResetColor();
        }

        public void WriteError(string value)
        {
            var color = ForegroundColor;
            ForegroundColor = ConsoleColor.Red;
            Console.Write(value);
            ResetColor();
        }

        public void WriteErrorLine(string value)
        {
            var color = ForegroundColor;
            ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            ResetColor();
        }

        public void ResetColor() => Console.ResetColor();

        public string ReadLine() => Console.ReadLine();
    }
}