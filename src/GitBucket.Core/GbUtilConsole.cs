using System.Text;

namespace GitBucket.Core;

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

    public void Write(string? value)
    {
        var color = ForegroundColor;
        ResetColor();
        Console.Write(value);
        ForegroundColor = color;
    }

    public void WriteLine(string? value)
    {
        var color = ForegroundColor;
        ResetColor();
        Console.WriteLine(value);
        ForegroundColor = color;
    }

    public void WriteWarn(string? value)
    {
        ForegroundColor = ConsoleColor.Yellow;
        Console.Write(value);
        ResetColor();
    }

    public void WriteWarnLine(string? value)
    {
        ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(value);
        ResetColor();
    }

    public void WriteError(string? value)
    {
        ForegroundColor = ConsoleColor.Red;
        Console.Write(value);
        ResetColor();
    }

    public void WriteErrorLine(string? value)
    {
        ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(value);
        ResetColor();
    }

    public void ResetColor() => Console.ResetColor();

    public string? ReadLine() => Console.ReadLine();

    public string? GetPassword()
    {
        var builder = new StringBuilder();
        while (true)
        {
            var consoleKeyInfo = Console.ReadKey(true);
            if (consoleKeyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (consoleKeyInfo.Key == ConsoleKey.Backspace && builder.Length > 0)
            {
                if (builder.Length > 0)
                {
                    Console.Write("\b\0\b");
                    builder.Length--;
                }

                continue;
            }

            Console.Write('*');
            builder.Append(consoleKeyInfo.KeyChar);
        }

        return builder.ToString();
    }
}
