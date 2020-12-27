using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Interface
{
    public interface IWord : ISentenceItem, IEnumerable<char>
    {
        public char this[int param] { get; set; }
        public int Length{get;} 
    }
}
