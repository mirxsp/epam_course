using System;
using System.Collections.Generic;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Present present = PresentParser.ParseFromFile("sweetlist.txt");
            int n;
            while(true)
            {
                Console.Clear();
                Console.WriteLine("1.Show present containment");
                Console.WriteLine("2.Find chocolate candy");
                Console.WriteLine("3.Find sugar candy");
                Console.WriteLine("4.Find candy by sugar range");
                Console.WriteLine("5.Sort by sugar percentage");
                Console.WriteLine("6.Sort by weight");
                Console.WriteLine("7.Sort by name");
             
                Console.WriteLine("8.Exit");
                while (!int.TryParse(Console.ReadLine(),out n))
                {
                    Console.WriteLine("Wrong input");
                }
                if (n == 1)
                {
                    foreach(Sweet ns in present.Sweets)
                    {
                        Console.WriteLine(ns.Name);
                        Console.WriteLine(ns.Weight);
                        Console.WriteLine(ns.SugarPercentage);
                    }
                }
                if (n == 2)
                {
                    Chocolate res = present.FindFirstChocolateCandy();
                    if (res != null)
                    {
                        Console.WriteLine(res.Name);
                        Console.WriteLine(res.Type);
                        Console.WriteLine(res.Weight);
                    }
                    else
                        Console.WriteLine("There is no such candy");
                    
                }
                if (n == 3)
                {
                    
                    Sugar res = present.FindFirstSugarCandy();
                    if (res != null)
                    {
                        Console.WriteLine(res.Name);
                        Console.WriteLine(res.Type);
                        Console.WriteLine(res.Weight);
                    }
                    else
                        Console.WriteLine("There is no such candy");
                }
                if (n == 4)
                {
                    double x, y;
                    Console.WriteLine("Input sugar range:");
                    while (!double.TryParse(Console.ReadLine(), out x))
                    {
                        Console.WriteLine("Wrong input");
                    }
                    while (!double.TryParse(Console.ReadLine(), out y))
                    {
                        Console.WriteLine("Wrong input");
                    }
                    Sweet res = present.FindFirstBySugarPercentageRange(x, y);
                    if (res != null)
                        Console.WriteLine(res.Name);
                    else
                        Console.WriteLine("There is no such candy");
                }
                if (n == 5)
                {
                    present.Sort(Sweet.CompareSweetsBySugarPercentage);
                }
                if (n == 6)
                {
                    present.Sort(Sweet.CompareSweetsByWeight);
                }
                if (n == 7)
                {
                    present.Sort(Sweet.CompareSweetsByName);
                }
                if (n == 8) break;
                Console.ReadKey();
            }
       
        }
    }
}
