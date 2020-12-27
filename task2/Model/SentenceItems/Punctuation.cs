using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Model.SentenceItems
{
    public class Punctuation : Interface.IPunctuation
    {
        private static readonly List<char> SentenceSeparators = new List<char>() { '.', '!', '?','\n' };
        private static readonly List<char> WordSeparators = new List<char>() { ' ', '#', '$', '%', '&', '`','"', '/', '@', ',', '-', ':', ';', '(', ')', '{', '}', '[', ']' };

        public string Value { get; set; }

        public Punctuation(char Char)
        {
            Value = Char.ToString();
        }

        public Punctuation(string String)
        {
            Value = String;
        }

        public bool IsSentenceSeparator()
        {
            return IsSentenceSeparator(this);
        }

        public bool IsWordSeparator()
        {
            return IsWordSeparator(this);
        }

        public static bool IsPunctuation(Punctuation c)
        {
            return IsWordSeparator(c)|| IsSentenceSeparator(c);
        }

        public static bool IsWordSeparator(Punctuation c)
        {
            bool flag = false;
            foreach (char ch in c.Value)
            {
                if (WordSeparators.Contains(ch)) flag = true;
            }
            return flag;
        }

        public static bool IsSentenceSeparator(Punctuation c)
        {
            bool flag = false;
            foreach (char ch in c.Value)
            {
                if (SentenceSeparators.Contains(ch)) flag = true;
            }
            return flag;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
