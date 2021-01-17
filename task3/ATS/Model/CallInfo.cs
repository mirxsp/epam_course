using ATS.ATS.Interfaces;
using System;

namespace ATS.ATS.Model
{
    class CallInfo
    {
        public ITerminal Source { get; set; }

        public ITerminal Target { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public double CostPerMinute { get; set; }

        public double Cost { get; set; }

        public override string ToString()
        {
            return $"{Source} -> {Target} : {StartTime} - {EndTime} / {CostPerMinute}";
        }
    }
}
