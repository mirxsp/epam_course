using ATS.ATS.Enums;
using ATS.ATS.Interfaces;
using ATS.BS.Interfaces;
using ATS.BS.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATS.ATS.Model
{
    class Station : IStation
    {
        public event EventHandler<CallInfo> CallEnded;

        public event EventHandler<ServiceRequest> ServiceRequested;

        int _clientLimit;

        IDictionary<IPort, ITerminal> _mapping;

        IDictionary<ITerminal, Request> _pendingRequests;

        ICollection<CallInfo> _activeCalls;

        public Station(int clientLimit)
        {
            _clientLimit = clientLimit;
            _mapping = new Dictionary<IPort, ITerminal>();
            _pendingRequests = new Dictionary<ITerminal, Request>();
            _activeCalls = new List<CallInfo>();
        }

        public ITerminal GetTerminalByNumber(string number)
        {
           return _mapping.FirstOrDefault(x => x.Value.Number == number).Value;
        }

        public IPort GetPortByNumber(string number)
        {
            return _mapping.FirstOrDefault(x => x.Value.Number == number).Key;
        }

        public void Allocate(object sender, string number)
        {
            if(_mapping.Count<_clientLimit)
            {
                ITerminal terminal = new Terminal(number);
                IPort port = new Port();
                Add(terminal, port);
            }
        }

        private void Add(ITerminal t, IPort p)
        {
            p.PortRequest += Recieve;
            p.TerminalLinked += TerminalMapping;
            p.TerminalUnlinked += TerminalUnmapping;
            t.Plug(p);
        }

        public void Recieve(object sender,Request request)
        {
            Console.WriteLine("Station: process");
            if(sender is ITerminal t)
            {
                switch (request.Code)
                {
                    case RequestCode.CALL:
                        _pendingRequests.Add(t, request);
                        Send(sender, request);
                        break;
                    case RequestCode.ACCEPT:
                        {
                            ITerminal terminalToAnswer = _pendingRequests.FirstOrDefault(x => x.Value.TargetNumber == t.Number).Key;
                            CallInfo info = new CallInfo() { Source = terminalToAnswer, Target = t, StartTime = DateTime.Now };
                            if (terminalToAnswer != null)
                            {
                                request.TargetNumber = terminalToAnswer.Number;
                                Send(sender, request);
                            }
                            _activeCalls.Add(info);
                            break;
                        }
                    case RequestCode.DECLINE:
                        {
                            ITerminal terminalToAnswer = _pendingRequests.FirstOrDefault(x => x.Value.TargetNumber == t.Number).Key;
                            if (terminalToAnswer == null)
                            {
                                string targetNumber = _pendingRequests[t].TargetNumber;
                                terminalToAnswer = GetTerminalByNumber(targetNumber);
                            }
                            if (terminalToAnswer != null)
                            {
                                _pendingRequests.Remove(terminalToAnswer);
                                request.TargetNumber = terminalToAnswer.Number;
                                Send(sender, request);
                            }
                            CallInfo currentCall = _activeCalls.FirstOrDefault(x => x.Source == sender || x.Target == sender || x.Target==terminalToAnswer);
                            if (currentCall == null) break;
                            currentCall.EndTime = DateTime.Now;
                            CallEnded(this, currentCall);
                            _activeCalls.Remove(currentCall);
                            break;
                        }
                    case RequestCode.GET:
                        {
                            if(request.TargetNumber=="SERVICE")
                            {
                                ServiceRequested(this, new ServiceRequest(BS.Enums.ServiceRequestCode.GETREPORT, t.Number, request.Message));
                            }
                            break;
                        }
                }
            }
        }

        private void TerminalMapping(object sender, ITerminal terminal)
        {
            if (sender is Port port)
            {
                _mapping.Add(port, terminal);
            }
        }

        private void TerminalUnmapping(object sender, ITerminal terminal)
        {
            if (sender is Port port)
            {
                _mapping.Remove(port);
            }
        }

        public void Send(object sender,Request request)
        {
            IPort targetPort = GetPortByNumber(request.TargetNumber);
            if (targetPort != null)
            {
                if(!targetPort.Recieve(sender, request))
                {
                    if(sender is ITerminal t)
                    {
                        _pendingRequests.Remove(t);
                        targetPort = GetPortByNumber(t.Number);
                        targetPort.Recieve(this, new Request(RequestCode.DECLINE));
                    }
                }
            }
        }

        public void LinkService(IService service)
        {
            service.ReportSend += ProcessReport;
            service.ContractSigned += Allocate;
        }

        public void UnlinkService(IService service)
        {
            service.ReportSend -= ProcessReport;
            service.ContractSigned -= Allocate;
        }

        public void ProcessReport(object sender, string report)
        {
            Console.WriteLine("Station: recieve");
            string tNumber = report.Substring(0, report.IndexOf('\n'));
            Send(this, new Request(RequestCode.GET, tNumber, report));
        }
    }
}
