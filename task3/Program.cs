using ATS.ATS.Interfaces;
using ATS.ATS.Model;
using ATS.BS.Model;
using System;

namespace ATS
{
    class Program
    {
        static void Main(string[] args)
        {
            int limit = 10;
            Service service = new Service(limit);
            Station station = new Station(limit);
            service.Link(station);
            station.LinkService(service);

            service.SignContract("U A");
            service.SignContract("A A");
            service.SignContract("a");

            string number1 = service.GetNumberByName("U A");
            string number2 = service.GetNumberByName("A A");
            string number3 = service.GetNumberByName("a");
            ITerminal t1 = station.GetTerminalByNumber(number1);
            ITerminal t2 = station.GetTerminalByNumber(number2);
            ITerminal t3 = station.GetTerminalByNumber(number3);


            t1.Call(number2);
            t2.Call(number1);
            t2.Answer(true);


        }
    }
}
