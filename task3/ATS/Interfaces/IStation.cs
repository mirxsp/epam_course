using ATS.ATS.Model;
using ATS.BS.Interfaces;
using ATS.BS.Model;
using System;

namespace ATS.ATS.Interfaces
{
    interface IStation
    {
        event EventHandler<CallInfo> CallEnded;

        event EventHandler<ServiceRequest> ServiceRequested;

        ITerminal GetTerminalByNumber(string number);

        IPort GetPortByNumber(string number);

        void Allocate(object sender, string number);

        void Recieve(object sender, Request request);

        void Send(object sender, Request request);

        void LinkService(IService service);

        void UnlinkService(IService service);

        void ProcessReport(object sender, string report);
    }

}
