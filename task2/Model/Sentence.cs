using lab2.Enum;
using lab2.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab2.Model
{
    public class Sentence : Interface.ISentence
    {
        public List<ISentenceItem> Items { get; set; }

        public List<IWord> Words { get => (List<IWord>)Items.Select(x => x is IWord); }
        
        public List<IPunctuation> Punctuation { get => (List<IPunctuation>)Items.Select(x => x is IPunctuation); }

        public SentenceTypes Type { get; set; }

        public Interface.ISentenceItem this[int param]
        {
            get
            {
                return Items[param];
            }
            set
            {
                Items[param] = value;
            }
        }

        public int Count => Items.Count;

        public int WordCount => Items.Select(x => x is IWord).Count();

        public bool IsReadOnly => false;

        public Sentence(IEnumerable<ISentenceItem> Items)
        {
            this.Items = new List<ISentenceItem>(Items);
        }

        public Sentence()
        {
            Items = new List<ISentenceItem>();
        }

        public void Add(ISentenceItem item)
        {
            Items.Add(item);
        }

        public void Insert(int index, ISentenceItem item)
        {
            Items.Insert(index, item);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(ISentenceItem item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(ISentenceItem[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ISentenceItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public bool Remove(ISentenceItem item)
        {
            return Items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(ISentenceItem item in Items)
            {
                stringBuilder.Append(item.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}
