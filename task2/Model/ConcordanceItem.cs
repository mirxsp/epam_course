using lab2.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Model
{
    public class ConcordanceItem
    {
        public IWord Word { get; set; }
        public int Count { get; set; }
        public List<int> EnterPoints { get; set; }

        public ConcordanceItem(IWord word)
        {
            Word = word;
            Count = 0;
            EnterPoints = new List<int>();
        }
        public ConcordanceItem(IWord word,int point)
        {
            Word = word;
            Count = 1;
            EnterPoints = new List<int>() { point };
        }
        public ConcordanceItem(IWord word,int point,int count)
        {
            Word = word;
            Count = count;
            EnterPoints = new List<int>() { point };
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append($"{Word,-30} {Count}:");
            foreach (int i in EnterPoints) result.Append(i + " ");
            return result.ToString();
        }

    }
}
