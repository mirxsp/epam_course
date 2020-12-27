using lab2.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Model
{
    public class Concordance
    {
        private List<ConcordanceItem> items;
        private Text text;
        public int LinesInPage { get; private set; }

        public Concordance(Text text, int linesInPage)
        {
            items = new List<ConcordanceItem>();
            this.text = text;
            LinesInPage = linesInPage;
            Build();
            
        }

        private void Build()
        {
            int line = 1;
            for (int i = 0; i < text.Count; i++)
            {
                for (int j = 0; j < text[i].Count; j++)
                {
                    if (text[i][j] is IWord word)
                    {
                        word.Value = word.Value.ToLower();
                        int index = items.FindIndex(x => x.Word.Value == word.Value);
                        int lineNumber = line / (LinesInPage + 1) + 1;
                        if (index == -1)
                        {
                            items.Add(new ConcordanceItem(word, lineNumber));
                        }
                        else
                        {
                            int index2 = items[index].EnterPoints.FindIndex(x => x == lineNumber);
                            if (index2 == -1)
                            {
                                items[index].EnterPoints.Add(lineNumber);
                            }
                            items[index].Count++;
                        }
                    }
                }
                if (text[i].Count > 0 && text[i].ToString()[^1] == '\n') line++;
            }
            items.Sort((x, y) => x.Word.Value.CompareTo(y.Word.Value));
        }

        public override string ToString()
        {
            if (items == null || items.Count == 0) return string.Empty;
            StringBuilder result = new StringBuilder();
            char letter = items[0].Word.Value[0];
            result.Append($"{letter}\n");
            foreach (ConcordanceItem item in items)
            {
                if (letter != item.Word.Value[0])
                {
                    letter = item.Word.Value[0];
                    result.Append($"{letter}\n");
                }
                result.Append(item.ToString() + '\n');
            }
            return result.ToString();
        }
    }
}
