namespace System;

public static class StringExtensions
{
    public static string ConvertFirstCharToUpper(this string self)
    {
        return string.IsNullOrEmpty(self) ? self : char.ToUpperInvariant(self.First()) + self[1..];
    }
}
