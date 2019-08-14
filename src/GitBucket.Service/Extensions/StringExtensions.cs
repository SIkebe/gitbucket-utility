using System.Linq;

namespace System
{
    public static class StringExtensions
    {
        public static string ConvertFirstCharToUpper(this string self)
        {
            if (string.IsNullOrEmpty(self)) return self;

            return char.ToUpperInvariant(self.First()) + self[1..];
        }
    }
}