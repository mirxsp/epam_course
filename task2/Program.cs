using System;
using lab2.Interface;
using lab2.Model;
using lab2.Model.Parsers;
using lab2.Model.Printers;

namespace lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            FileTextParser fileTextParser = new FileTextParser("text.txt");
            Text result = fileTextParser.Parse();
            fileTextParser.Dispose();
            ConsolePrinter printer = new ConsolePrinter();
            FilePrinter filePrinter = new FilePrinter("output.txt", true);
            if(result==null)
            {
                Console.WriteLine("There is no text in text.txt");
                Console.ReadKey();
                return;
            }
            do
            {
                Console.Clear();
                Console.WriteLine("1.Show by word count.\n" +
                "2.Show unique words by length in interrogative sentences.\n" +
                "3.Remove all words by length that starts with consonant letter.\n" +
                "4.Replace words by length in sentence with substring.\n" +
                "5.Make concordance.\n" +
                "6.Save text model.\n" +
                "7.Show text\n" +
                "ESC.Exit\n");
                ConsoleKey key = Console.ReadKey().Key;
                Console.WriteLine();
                if (key == ConsoleKey.D1)
                {
                    TextProcessor.PrintByWordCount(result, new ConsolePrinter());
                }
                else if (key == ConsoleKey.D6)
                {
                    filePrinter.Print(result.ToString());
                    Console.WriteLine("Check output.txt");
                }
                else if (key == ConsoleKey.Escape) break;
                else if (key == ConsoleKey.D7) Console.WriteLine(result.ToString());
                else if (key == ConsoleKey.D5)
                {
                    Console.WriteLine("Input number of lines on one page:");
                    if(!int.TryParse(Console.ReadLine(),out int linesInPage)) Console.WriteLine("Incorrect input!");
                    else
                    {
                        Concordance concordance = new Concordance(result, linesInPage);
                        filePrinter.Print(concordance.ToString());
                        Console.WriteLine("Check output.txt");
                    }
                }
                else if (key >= ConsoleKey.D2 && key <= ConsoleKey.D4)
                {
                    Console.WriteLine("Input length:");
                    if (!int.TryParse(Console.ReadLine(), out int length)) Console.WriteLine("Incorrect input!");
                    else
                    {
                        if (key == ConsoleKey.D2) TextProcessor.PrintUniqueWords(result, printer, length, Enum.SentenceTypes.Interrogative);
                        else if (key == ConsoleKey.D3)
                        {
                            TextProcessor.RemoveConsonantWordsByLengthFromText(result, length);
                            Console.WriteLine("Succeed!");
                        }
                        else if (key == ConsoleKey.D4)
                        {
                            Console.WriteLine("Input sentence number:");
                            if (!int.TryParse(Console.ReadLine(), out int sentence)) Console.WriteLine("Incorrect input!");
                            else
                            {
                                Console.WriteLine("Input substring:");
                                try
                                {
                                    TextProcessor.ReplaceByLength(result[sentence-1], length, Console.ReadLine());
                                    Console.WriteLine("Succeed!");
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    Console.WriteLine("Incorrect sentence number!");
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            } while (true);
            filePrinter.Dispose();
            
        }
    }
}
