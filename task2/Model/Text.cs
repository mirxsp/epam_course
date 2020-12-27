using lab2.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace lab2.Model
{
    public class Text : ICollection<ISentence>
    {
        public List<ISentence> Sentences { get; set; }

        public Interface.ISentence this[int param]
        {
            get
            {
                return Sentences[param];
            }
            set
            {
                Sentences[param] = value;
            }
        }

        public Text()
        {
            Sentences = new List<ISentence>();
        }

        public Text(List<ISentence> Sentences)
        {
            this.Sentences = new List<ISentence>(Sentences);
        }

        public int Count => Sentences.Count;

        public bool IsReadOnly => false;

        public void Add(ISentence item)
        {
            Sentences.Add(item);
        }

        public void Clear()
        {
            Sentences.Clear();
        }

        public bool Contains(ISentence item)
        {
            return Sentences.Contains(item);
        }

        public void CopyTo(ISentence[] array, int arrayIndex)
        {
            Sentences.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ISentence> GetEnumerator()
        {
            return Sentences.GetEnumerator();
        }

        public bool Remove(ISentence item)
        {
            return Sentences.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Sentences.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach(ISentence sentence in Sentences)
            {
                result.Append(sentence.ToString());
            }
            return result.ToString();
        }
    }
}
