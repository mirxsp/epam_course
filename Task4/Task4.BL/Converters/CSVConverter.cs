using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task4.Model;

namespace Task4.Converters.BL
{
    public static class CSVConverter
    {
        public static Sale Convert(string filepath)
        {
            StreamReader reader = new StreamReader(filepath);
            string[] s = reader.ReadToEnd().Split(',');
            if (s.Length < 4 || !DateTime.TryParse(s[0], out DateTime dateTime) || !double.TryParse(s[3], out double price))
            {
                throw new FormatException(filepath + " invalid format.");
            }
            Client client = new Client() { Name = s[1] };
            Item item = new Item() { Name = s[2], Price = price };
            reader.Dispose();
            return new Sale { Date = dateTime, Client = client, Item = item };
        }
    }
}
