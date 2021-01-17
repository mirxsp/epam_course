using ATS.ATS.Interfaces;
using ATS.BS.Enums;

namespace ATS.BS.Model
{
    class ServiceRequest : IRequest<ServiceRequestCode>
    {
        public ServiceRequestCode Code { get; private set; }

        public string TargetNumber { get; set; }

        public string Message { get; set; }

        public ServiceRequest(ServiceRequestCode code,string targetNumber = null,string message = null)
        {
            Code = code;
            TargetNumber = targetNumber;
            Message = message;
        }
    }
}
