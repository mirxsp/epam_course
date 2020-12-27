using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Extensions
{
    public static class CharExtension
    {
        public static bool IsVowel(this char c)
        {
            return "aeiou".Contains(Char.ToLower(c));
        }
    }
}
