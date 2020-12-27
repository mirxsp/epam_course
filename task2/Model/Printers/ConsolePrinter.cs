using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Model.Printers
{
    public class ConsolePrinter : Interface.IPrinter
    {
        public void Print(string String)
        {
            Console.Write(String);
        }
    }
}
