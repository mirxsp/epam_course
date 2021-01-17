using ATS.ATS.Enums;
using ATS.ATS.Model;
using System;

namespace ATS.ATS.Interfaces
{
    interface IPort
    {
         event EventHandler<ITerminal> TerminalLinked;

        event EventHandler<ITerminal> TerminalUnlinked;

        event EventHandler<Request> PortRequest;

        bool Recieve(object sender, Request request);

        bool Link(ITerminal terminal);
    }
}
