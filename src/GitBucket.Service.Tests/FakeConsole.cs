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
        private bool hasNewLineAtTheEndOfTheMessages = false;
        public List<string> Messages { get; set; } = new List<string>();
        public List<string> WarnMessages { get; set; } = new List<string>();
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Gray;

        public void Write(string value)
        {
            if (!Messages.Any())
            {
                Messages.Add(value);
                hasNewLineAtTheEndOfTheMessages = false;
            }
            else if (hasNewLineAtTheEndOfTheMessages)
            {
                Messages.Add(value);
                hasNewLineAtTheEndOfTheMessages = false;
            }
            else
            {
                Messages[Messages.Count - 1] += value;
                hasNewLineAtTheEndOfTheMessages = false;
            }
        }

        public void WriteLine(string value)
        {
            if (!Messages.Any())
            {
                Messages.Add(value);
                hasNewLineAtTheEndOfTheMessages = true;
            }
            else if (hasNewLineAtTheEndOfTheMessages)
            {
                Messages.Add(value);
                hasNewLineAtTheEndOfTheMessages = true;
            }
            else
            {
                Messages[Messages.Count - 1] += value;
                hasNewLineAtTheEndOfTheMessages = true;
            }
        }

        public void WriteWarn(string value)
        {
            if (!Messages.Any())
            {
                WarnMessages.Add(value);
                hasNewLineAtTheEndOfTheMessages = false;
            }
            else if (hasNewLineAtTheEndOfTheMessages)
            {
                WarnMessages.Add(value);
                hasNewLineAtTheEndOfTheMessages = false;
            }
            else
            {
                WarnMessages[WarnMessages.Count - 1] += value;
                hasNewLineAtTheEndOfTheMessages = false;
            }
        }

        public void WriteWarnLine(string value)
        {
            if (!Messages.Any())
            {
                WarnMessages.Add(value);
                hasNewLineAtTheEndOfTheMessages = true;
            }
            else if (hasNewLineAtTheEndOfTheMessages)
            {
                WarnMessages.Add(value);
                hasNewLineAtTheEndOfTheMessages = true;
            }
            else
            {
                WarnMessages[WarnMessages.Count - 1] += value;
                hasNewLineAtTheEndOfTheMessages = true;
            }
        }

        public void WriteError(string value)
        {
            if (!Messages.Any())
            {
                ErrorMessages.Add(value);
                hasNewLineAtTheEndOfTheMessages = false;
            }
            else if (hasNewLineAtTheEndOfTheMessages)
            {
                ErrorMessages.Add(value);
                hasNewLineAtTheEndOfTheMessages = false;
            }
            else
            {
                ErrorMessages[ErrorMessages.Count - 1] += value;
                hasNewLineAtTheEndOfTheMessages = false;
            }
        }

        public void WriteErrorLine(string value)
        {
            if (!Messages.Any())
            {
                ErrorMessages.Add(value);
                hasNewLineAtTheEndOfTheMessages = true;
            }
            else if (hasNewLineAtTheEndOfTheMessages)
            {
                ErrorMessages.Add(value);
                hasNewLineAtTheEndOfTheMessages = true;
            }
            else
            {
                ErrorMessages[ErrorMessages.Count - 1] += value;
                hasNewLineAtTheEndOfTheMessages = true;
            }
        }

        public void ResetColor() => ForegroundColor = ConsoleColor.Gray;

        public virtual string ReadLine() => "test";
    }
}