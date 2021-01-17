using ATS.ATS.Enums;
using ATS.ATS.Interfaces;
using System;

namespace ATS.ATS.Model
{
    class Port : IPort
    {
        public event EventHandler<ITerminal> TerminalLinked;

        public event EventHandler<ITerminal> TerminalUnlinked;

        public event EventHandler<Request> PortRequest;

        ITerminal _linkedTerminal;

        void ProcessRequest(object sender, Request request)
        {
            Console.WriteLine("Port: process");
            PortRequest?.Invoke(sender, request);
        }

        public bool Recieve(object sender,Request request)
        {
            Console.WriteLine("Port: recieve");
            if (_linkedTerminal == null) return false;
            return _linkedTerminal.Recieve(sender, request);
        }

        public bool Link(ITerminal terminal)
        {
            if (_linkedTerminal==null)
            {
                terminal.TerminalRequest += ProcessRequest;
                terminal.StateChanged += Unlink;
                TerminalLinked?.Invoke(this, terminal);
                _linkedTerminal = terminal;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Unlink(object sender,TerminalState state)
        {
            if (state == TerminalState.Unplugged)
            {
                if ((sender is ITerminal terminal) && terminal == _linkedTerminal)
                {
                    terminal.TerminalRequest -= ProcessRequest;
                    terminal.StateChanged -= Unlink;
                    _linkedTerminal = null;
                    TerminalUnlinked?.Invoke(this, terminal);
                }
            }
        }
    }
}
