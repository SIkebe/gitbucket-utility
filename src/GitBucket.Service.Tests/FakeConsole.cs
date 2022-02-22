using GitBucket.Core;

namespace GitBucket.Service.Tests;

/// <summary>
/// Implementation of a fake <see cref="IConsole"/>.
/// </summary>
public class FakeConsole : IConsole
{
    private readonly string _input;
    private ConsoleKind _consoleKind = ConsoleKind.Normal;
    private bool _hasNewLineAtTheEndOfTheMessages;

    public FakeConsole(string input = "test") => _input = input;

    private enum ConsoleKind
    {
        Normal,
        Warn,
        Error
    }

#pragma warning disable SA1000 // The keyword 'new' should be followed by a space
    public List<string?> Messages { get; } = new();
    public List<string?> WarnMessages { get; } = new();
    public List<string?> ErrorMessages { get; } = new();
    public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Gray;
#pragma warning restore CA1304 // The keyword 'new' should be followed by a space

    public void Write(string? value)
    {
        if (!Messages.Any())
        {
            Messages.Add(value);
        }
        else if (_hasNewLineAtTheEndOfTheMessages)
        {
            Messages.Add(value);
        }
        else
        {
            Messages[^1] += value;
        }

        _consoleKind = ConsoleKind.Normal;
        _hasNewLineAtTheEndOfTheMessages = false;
    }

    public void WriteLine(string? value)
    {
        if (!Messages.Any())
        {
            Messages.Add(value);
        }
        else if (_hasNewLineAtTheEndOfTheMessages)
        {
            Messages.Add(value);
        }
        else
        {
            Messages[^1] += value;
        }

        _consoleKind = ConsoleKind.Normal;
        _hasNewLineAtTheEndOfTheMessages = true;
        ResetColor();
    }

    public void WriteWarn(string? value)
    {
        if (!WarnMessages.Any())
        {
            WarnMessages.Add(value);
        }
        else if (_hasNewLineAtTheEndOfTheMessages)
        {
            WarnMessages.Add(value);
        }
        else
        {
            WarnMessages[^1] += value;
        }

        _consoleKind = ConsoleKind.Warn;
        _hasNewLineAtTheEndOfTheMessages = false;
    }

    public void WriteWarnLine(string? value)
    {
        if (!WarnMessages.Any())
        {
            WarnMessages.Add(value);
        }
        else if (_hasNewLineAtTheEndOfTheMessages)
        {
            WarnMessages.Add(value);
        }
        else
        {
            WarnMessages[^1] += value;
        }

        _consoleKind = ConsoleKind.Normal;
        _hasNewLineAtTheEndOfTheMessages = true;
        ResetColor();
    }

    public void WriteError(string? value)
    {
        if (!ErrorMessages.Any())
        {
            ErrorMessages.Add(value);
        }
        else if (_hasNewLineAtTheEndOfTheMessages)
        {
            ErrorMessages.Add(value);
        }
        else
        {
            ErrorMessages[^1] += value;
        }

        _consoleKind = ConsoleKind.Error;
        _hasNewLineAtTheEndOfTheMessages = false;
    }

    public void WriteErrorLine(string? value)
    {
        if (value != null)
        {
            if (!ErrorMessages.Any())
            {
                ErrorMessages.Add(value);
            }
            else if (_hasNewLineAtTheEndOfTheMessages)
            {
                ErrorMessages.Add(value);
            }
            else
            {
                ErrorMessages[^1] += value;
            }
        }

        _consoleKind = ConsoleKind.Normal;
        _hasNewLineAtTheEndOfTheMessages = true;
        ResetColor();
    }

    public void ResetColor() => ForegroundColor = ConsoleColor.Gray;

    public virtual string ReadLine()
    {
        switch (_consoleKind)
        {
            case ConsoleKind.Warn:
                if (!_hasNewLineAtTheEndOfTheMessages)
                {
                    WarnMessages[^1] += _input;
                }
                else
                {
                    WarnMessages.Add(_input);
                }

                break;

            case ConsoleKind.Error:
                if (!_hasNewLineAtTheEndOfTheMessages)
                {
                    ErrorMessages[^1] += _input;
                }
                else
                {
                    ErrorMessages.Add(_input);
                }

                break;

            default:
                if (!_hasNewLineAtTheEndOfTheMessages)
                {
                    Messages[^1] += _input;
                }
                else
                {
                    Messages.Add(_input);
                }

                break;
        }

        _consoleKind = ConsoleKind.Normal;
        _hasNewLineAtTheEndOfTheMessages = true;
        return _input;
    }

    public string? GetPassword()
    {
        throw new NotImplementedException();
    }
}
