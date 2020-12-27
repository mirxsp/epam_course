using lab2.Extensions;
using lab2.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lab2.Model
{
    public class TextProcessor
    {
        public static void PrintByWordCount(Text text,Interface.IPrinter printer)
        {
            List<Interface.ISentence> sentences = (List<Interface.ISentence>)text.Sentences;
            sentences.Sort((x, y) => x.WordCount.CompareTo(y.WordCount));
            StringBuilder stringBuilder = new StringBuilder();
            foreach(Interface.ISentence sentence in sentences)
            {
                stringBuilder.Append(sentence.ToString());
            }
            printer.Print(stringBuilder.ToString());
        }

        public static void PrintUniqueWords(Text text,Interface.IPrinter printer, int length, Enum.SentenceTypes type)
        {
            List<Interface.IWord> words = new List<Interface.IWord>();
            foreach(Interface.ISentence sentence in text)
            {
                if (sentence.Type == type)
                {
                    foreach(Interface.ISentenceItem word in sentence)
                    {
                        if(word is Interface.IWord tempWord && tempWord.Length==length)
                        {
                            bool flag = true;
                            foreach(var i in words)
                            {
                                if (i.Value.ToLower() == tempWord.Value.ToLower()) flag = false;
                            }
                            if(flag)words.Add(tempWord);
                        }
                    }
                }
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach(Interface.IWord word in words)
            {
                stringBuilder.Append(word.ToString() + " ");
            }
            printer.Print(stringBuilder.ToString());
        }

        public static void RemoveConsonantWordsByLengthFromText(Text text,int length)
        {
            foreach(Interface.ISentence sentence in text)
            {
                for(int i = 0;i<sentence.Count;i++)
                {
                    if (sentence[i] is Interface.IWord word && word.Length != 0 && word.Length==length && !word[0].IsVowel()) sentence.Remove(sentence[i--]);
                }
            }
        }

        public static void ReplaceByLength(ISentence sentence, int length,string substring)
        {
            for(int i = 0; i < sentence.Count; i++)
            {
                if(sentence[i] is Interface.IWord word && word.Length == length)
                {
                    sentence.Remove(word);
                    sentence.Insert(i, new SentenceItems.Word(substring));
                } 
            }
        }
    }
}
