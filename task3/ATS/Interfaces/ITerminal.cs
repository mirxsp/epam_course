using ATS.ATS.Enums;
using ATS.ATS.Model;
using System;

namespace ATS.ATS.Interfaces
{
    interface ITerminal
    {
        event EventHandler<TerminalState> StateChanging;

        event EventHandler<TerminalState> StateChanged;
public 
        string Number { get; }

        TerminalState State { get; set; }

        event EventHandler<Request> TerminalRequest;

        bool Recieve(object sender, Request request);

        void Call(string target);

        void Answer(bool value);

        void GetFullReport();

        void GetReportByNumber(string number);

        void GetReportByDate(DateTime date);

        void GetReportByCost(double cost);

        void Plug(IPort port);

        void Unplug();
    }
}
