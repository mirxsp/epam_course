using lab2.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace lab2.Model.SentenceItems
{
    public class Word : Interface.IWord
    {
        public string Value { get; set; }

        public char this[int param]
        {
            get
            {
                return Value[param];
            }
            set
            {
                StringBuilder stringBuilder = new StringBuilder(Value);
                stringBuilder.Remove(param, 1);
                stringBuilder.Insert(param, value);
                Value = stringBuilder.ToString();
                stringBuilder.Clear();
            }
        }

        public int Length
        {
            get => Value.Length;
        }

        public Word(string i)
        {
            Value = i;
        }

        public override string ToString()
        {
            return Value;
        }

        public IEnumerator<char> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }
}
