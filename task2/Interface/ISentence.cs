using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Interface
{
    public interface ISentence : ICollection<ISentenceItem>
    {
        List<ISentenceItem> Items { get; }
        Enum.SentenceTypes Type { get; }
        int WordCount { get; }
        public Interface.ISentenceItem this[int param] { get;set;}
        public void Insert(int index, ISentenceItem item);
    }
}
