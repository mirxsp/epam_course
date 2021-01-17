using ATS.ATS.Enums;
using ATS.ATS.Interfaces;

namespace ATS.ATS.Model
{
    class Request : IRequest<RequestCode>
    {
        public RequestCode Code { get; private set; }

        public string TargetNumber { get; set; }

        public string Message { get; set; }

        public Request(RequestCode code,string targetNumber = null,string message = null)
        {
            Code = code;
            TargetNumber = targetNumber;
            Message = message;
        }
    }
}
