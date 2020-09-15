#nullable enable
using System;

namespace GitBucket.Core
{
    /// <summary>
    /// Represents console output.
    /// </summary>
    public interface IConsole
    {
        ConsoleColor ForegroundColor { get; set; }
        void Write(string? value);
        void WriteLine(string? value);
        void WriteWarn(string? value);
        void WriteWarnLine(string? value);
        void WriteError(string? value);
        void WriteErrorLine(string? value);
        void ResetColor();
        string? ReadLine();
        string? GetPassword();
    }
}