using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace lab3
{
    class PresentParser
    {
        public static Present ParseFromFile(string filename)
        {
            StreamReader reader = new StreamReader(File.OpenRead(filename));
            List<Sweet> sweets = new List<Sweet>();
            while (!reader.EndOfStream)
            {
                string name = reader.ReadLine(); //Reading sweet name

                double weight,sugarAmount; 
                if (!double.TryParse(reader.ReadLine(), out weight)) throw new FormatException();
                if (!double.TryParse(reader.ReadLine(), out sugarAmount)) throw new FormatException(); //Reading sweet weight and sugarAmount,checking format
                Sweet obj;
                string type = reader.ReadLine();
                if (type == "Sweet")
                {
                    obj = new Sweet()
                    {
                        Name = name,
                        Weight = weight,
                        SugarAmount = sugarAmount
                    };
                } else if (type == "Chocolate")
                {
                    Chocolate.Types ctype;
                    if (!Chocolate.Types.TryParse(reader.ReadLine(), out ctype)) throw new FormatException(); //Parsing chocolate type

                    double palmOilAmount;
                    if (!double.TryParse(reader.ReadLine(), out palmOilAmount)) throw new FormatException(); //Parsing palm oil amount

                    obj = new Chocolate() {
                        Name = name,
                        Weight = weight,
                        SugarAmount = sugarAmount,
                        PalmOilAmount = palmOilAmount,
                        Type = ctype
                    };
                } else if (type == "Sugar")
                {
                    Sugar.Types ctype;
                    if (!Sugar.Types.TryParse(reader.ReadLine(), out ctype)) throw new FormatException(); //Sugar candy type parsing

                    double hardness;
                    if (!double.TryParse(reader.ReadLine(), out hardness)) throw new FormatException(); //Hardness parsing

                    obj = new Sugar()
                    {
                        Name = name,
                        Weight = weight,
                        SugarAmount = sugarAmount,
                        Hardness = hardness,
                        Type = ctype
                    };
                } else throw new FormatException();

                sweets.Add(obj);
            }
            reader.Close();
            return new Present(sweets);
        }
    }
}
