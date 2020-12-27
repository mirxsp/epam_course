using lab2.Model;
using lab2.Model.SentenceItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace lab2.Model.Parsers
{
    public class FileTextParser : Interface.ITextParser, IDisposable
    {
        private readonly StreamReader reader;
        private bool disposedValue;

        public FileTextParser(string filename)
        {
            reader = new StreamReader(filename);
        }

        public Text Parse()
        {
            Text text = new Text();
            Sentence tempSentence = new Sentence();
            StringBuilder buffer = new StringBuilder();
            while (!reader.EndOfStream)
            {
                bool isWord = false;
                bool isWhitespace = false;
                bool isNewSentence = false;
                char? typeChar = null;
                foreach (char c in reader.ReadLine())
                {
                    if (Punctuation.IsSentenceSeparator(new Punctuation(c)))
                    {
                        isNewSentence = true;
                    }
                    else
                    {
                        if(isNewSentence)
                        {
                            isNewSentence = false;
                            typeChar = null;
                            if (buffer.Length > 0)
                            {
                                typeChar = buffer[^1];
                                tempSentence.Add(new Punctuation(buffer.ToString()));
                                buffer.Clear();
                            }
                            if(typeChar!=null)
                            {
                                if (typeChar == '?') tempSentence.Type = Enum.SentenceTypes.Interrogative;
                                else if (typeChar == '!') tempSentence.Type = Enum.SentenceTypes.Exclamative;
                                else if (typeChar == '.') tempSentence.Type = Enum.SentenceTypes.Declarative;
                                else tempSentence.Type = Enum.SentenceTypes.Undefined;
                            }else tempSentence.Type = Enum.SentenceTypes.Undefined;
                            text.Add(tempSentence);
                            tempSentence = new Sentence();

                        }
                    }
                    if (char.IsWhiteSpace(c))
                    {
                        if (isWhitespace) continue;
                        isWhitespace = true;
                    }
                    else isWhitespace = false;
                    if (char.IsLetter(c))
                    {
                        if (!isWord)
                        {
                            isWord = true;
                            if (buffer.Length > 0)
                            {
                                tempSentence.Add(new Punctuation(buffer.ToString()));
                                buffer.Clear();
                            }
                        }
                        buffer.Append(c);
                    }
                    else if (Punctuation.IsPunctuation(new Punctuation(c)))
                    {
                        if (isWord)
                        {
                            isWord = false;
                            if (buffer.Length > 0)
                            {
                                tempSentence.Add(new Word(buffer.ToString()));
                                buffer.Clear();
                            }
                        }
                        buffer.Append(c);
                    }
                    else isWord = false;
                }
                typeChar = null;
                if (buffer.Length > 0)
                {
                    typeChar = buffer[^1];
                    Interface.ISentenceItem item;
                    if (isWord) item = new Word(buffer.ToString());
                    else item = new Punctuation(buffer.ToString());
                    tempSentence.Add(item);
                    buffer.Clear();
                }
                if (typeChar != null)
                {
                    if (typeChar == '?') tempSentence.Type = Enum.SentenceTypes.Interrogative;
                    else if (typeChar == '!') tempSentence.Type = Enum.SentenceTypes.Exclamative;
                    else if (typeChar == '.') tempSentence.Type = Enum.SentenceTypes.Declarative;
                    else tempSentence.Type = Enum.SentenceTypes.Undefined;
                }
                else tempSentence.Type = Enum.SentenceTypes.Undefined;
                tempSentence.Add(new Punctuation('\n'));
                text.Add(tempSentence);
                tempSentence = new Sentence();
            }
            return text;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    reader.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
