using System;
using System.Collections.Generic;
using System.Text;

namespace ATS.ATS.Interfaces
{
    interface IRequest<T>
    {
        T Code { get; }
        string TargetNumber { get; set; }
        string Message { get; set; }
    }
}
