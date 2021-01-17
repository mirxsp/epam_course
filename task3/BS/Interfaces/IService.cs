using ATS.ATS.Interfaces;
using ATS.ATS.Model;
using ATS.BS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATS.BS.Interfaces
{
    interface IService
    {
        public ICollection<Client> Clients { get; set; }

        public ICollection<CallInfo> Calls { get; set; }

        public double CostPerMinute { get; set; }

        public event EventHandler<string> ContractSigned;

        public event EventHandler<string> ReportSend;

        public void CloseReport();

        public void ProcessRequest(object sender, ServiceRequest request);

        public string GetNumberByName(string name);

        public void SignContract(string name);

        public void Link(IStation station);

        public void Unlink(IStation station);
    }
}
