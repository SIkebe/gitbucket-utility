using System;
using System.Collections.Generic;
using System.Linq;
using GitBucket.Core;

namespace GitBucket.Service.Tests
{
    /// <summary>
    /// Implementation of a fake <see cref="IConsole"/>.
    /// </summary>
    public class FakeConsole : IConsole
    {
        private readonly string _input;
        private ConsoleKind _consoleKind = ConsoleKind.Normal;
        private bool _hasNewLineAtTheEndOfTheMessages = false;

        public FakeConsole(string input = "test") => _input = input;

        private enum ConsoleKind
        {
            Normal,
            Warn,
            Error
        }

        public List<string> Messages { get; } = new List<string>();
        public List<string> WarnMessages { get; } = new List<string>();
        public List<string> ErrorMessages { get; } = new List<string>();
        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Gray;

        public void Write(string value)
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
                Messages[Messages.Count - 1] += value;
            }

            _consoleKind = ConsoleKind.Normal;
            _hasNewLineAtTheEndOfTheMessages = false;
        }

        public void WriteLine(string value)
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
                Messages[Messages.Count - 1] += value;
            }

            _consoleKind = ConsoleKind.Normal;
            _hasNewLineAtTheEndOfTheMessages = true;
            ResetColor();
        }

        public void WriteWarn(string value)
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
                WarnMessages[WarnMessages.Count - 1] += value;
            }

            _consoleKind = ConsoleKind.Warn;
            _hasNewLineAtTheEndOfTheMessages = false;
        }

        public void WriteWarnLine(string value)
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
                WarnMessages[WarnMessages.Count - 1] += value;
            }

            _consoleKind = ConsoleKind.Normal;
            _hasNewLineAtTheEndOfTheMessages = true;
            ResetColor();
        }

        public void WriteError(string value)
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
                ErrorMessages[ErrorMessages.Count - 1] += value;
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
                    ErrorMessages[ErrorMessages.Count - 1] += value;
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
                        WarnMessages[WarnMessages.Count - 1] += _input;
                    }
                    else
                    {
                        WarnMessages.Add(_input);
                    }

                    break;

                case ConsoleKind.Error:
                    if (!_hasNewLineAtTheEndOfTheMessages)
                    {
                        ErrorMessages[ErrorMessages.Count - 1] += _input;
                    }
                    else
                    {
                        ErrorMessages.Add(_input);
                    }

                    break;

                default:
                    if (!_hasNewLineAtTheEndOfTheMessages)
                    {
                        Messages[Messages.Count - 1] += _input;
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

        public string GetPassword()
        {
            throw new NotImplementedException();
        }
    }
}