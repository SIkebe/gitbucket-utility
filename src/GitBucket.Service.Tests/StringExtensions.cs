using System;

namespace GitBucket.Service.Tests
{
    public static class StringExtensions
    {
        public static string? RemoveLineEndings(this string self)
        {
            if (self is null)
            {
                return null;
            }

            return self.Replace($"{Environment.NewLine}", "", StringComparison.OrdinalIgnoreCase);
        }
    }
}
