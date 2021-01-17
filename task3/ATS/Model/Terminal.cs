using ATS.ATS.Enums;
using ATS.ATS.Interfaces;
using System;

namespace ATS.ATS.Model
{
    class Terminal : ITerminal
    {
        private Request _currentRequest;

        private object _currentSender;

        public event EventHandler<TerminalState> StateChanging;
        
        public event EventHandler<TerminalState> StateChanged;

        public string Number { get; private set; }

        private TerminalState _state;

        public TerminalState State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    OnStateChanging(_state);
                    _state = value;
                    OnStateChanged(_state);
                }
            }
        }

        public Terminal(string number)
        {
            Number = number;
        }

        public event EventHandler<Request> TerminalRequest;

        private void Request(RequestCode code,string target, string message = null)
        {
            Request request = new Request(code, target,message);
            if (code == RequestCode.CALL)
            {
                _currentRequest = request;
                _currentSender = this;
                State = TerminalState.Busy;
            }
            TerminalRequest?.Invoke(this, request);
        }

        public bool Recieve(object sender,Request request)
        {
            Console.WriteLine("Terminal recieve");
            if (State == TerminalState.Plugged && request.Code == RequestCode.CALL)
            {
                Console.WriteLine(Number + " call started");
                State = TerminalState.Busy;
                _currentRequest = request;
                _currentSender = sender;
                return true;
            }
            if(request.Code==RequestCode.DECLINE)
            {
                Console.WriteLine(Number + " call ended");
                State = TerminalState.Plugged;
                _currentSender = null;
                _currentRequest = null;
                return true;

            }
            if (request.Code==RequestCode.ACCEPT)
            {
                Console.WriteLine(Number + " call accepted");
                return true;
            }
            if (request.Code==RequestCode.GET)
            {
                Console.WriteLine(request.Message);
                return true;
            }
            return false;
        }

        public void Call(string target)
        {
            if (State == TerminalState.Plugged)
            {
                Console.WriteLine(Number + ": calling " + target);
                Request(RequestCode.CALL, target);
            }
        }

        public void Answer(bool value)
        {
            if(_currentRequest!=null && _currentRequest.Code == RequestCode.CALL)
            {
                RequestCode Code = value? RequestCode.ACCEPT : RequestCode.DECLINE;
                if (_currentSender is Terminal t)
                {
                    if (!value)
                    {
                        Console.WriteLine(Number + " call declinded");
                        Code = RequestCode.DECLINE;
                        State = TerminalState.Plugged;
                        _currentSender = null;
                        _currentRequest = null;
                        Request(Code, t.Number);
                    }
                    else if (value)
                    {
                        if (_currentRequest.TargetNumber == Number)
                        {
                            Request(Code, t.Number);
                            Console.WriteLine(Number + " call accepted");
                        }
                    }
                }
            }
        }

        public void GetFullReport()
        {
            Request(RequestCode.GET, "SERVICE", "0");
        }

        public void GetReportByNumber(string number)
        {
            Request(RequestCode.GET, "SERVICE", $"1{number}");
        }

        public void GetReportByDate(DateTime date)
        {
            Request(RequestCode.GET, "SERVICE", $"3{date}");
        }

        public void GetReportByCost(double cost)
        {
            Request(RequestCode.GET, "SERVICE", $"2{cost}");
        }

        private void OnStateChanging(TerminalState state)
        {
            StateChanging?.Invoke(this, state);
        }

        private void OnStateChanged(TerminalState state)
        {
            StateChanged?.Invoke(this, state);
        }

        public void Plug(IPort port)
        {
            if (port.Link(this))
            {
                State = TerminalState.Plugged;
            }
            else
            {
                Console.WriteLine("Plug failed");
            }
        }

        public void Unplug()
        {
            Answer(false);
            State = TerminalState.Unplugged;
        }

        public override string ToString()
        {
            return Number;
        }
    }
}
