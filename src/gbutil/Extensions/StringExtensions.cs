using System;
using System.Linq;

namespace gbutil.Extensions
{
    public static class StringExtensions
    {
        public static string ConvertFirstCharToUpper(this string self)
        {
            if (string.IsNullOrEmpty(self)) return self;

            return char.ToUpperInvariant(self.First()) + self.Substring(1, self.Length - 1);
        }
    }
}