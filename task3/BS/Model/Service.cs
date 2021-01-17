using ATS.ATS.Interfaces;
using ATS.ATS.Model;
using ATS.BS.Enums;
using ATS.BS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATS.BS.Model
{
    class Service : IService
    {
        public ICollection<Client> Clients { get; set; }

        public ICollection<CallInfo> Calls { get; set; }

        private IList<string> _numberPool;

        private DateTime _reportOpenDate;

        public double CostPerMinute { get; set; }

        public event EventHandler<string> ContractSigned;

        public event EventHandler<string> ReportSend;

        public Service(int clientLimit)
        {
            Calls = new List<CallInfo>();
            Clients = new List<Client>();
            _numberPool = new List<string>(clientLimit);
            int buf = clientLimit-1;
            StringBuilder formatString = new StringBuilder();
            formatString.Append("0");
            do
            {
                formatString.Append("0");
                buf /= 10;
            } while (buf / 10 > 0);
            for (var i = 0;i<clientLimit;i++)
            {
                _numberPool.Add(i.ToString(formatString.ToString()));
            }
        }

        public void CloseReport()
        {
            Calls.Clear();
            _reportOpenDate = DateTime.Now;
        }

        public void ProcessRequest(object sender, ServiceRequest request)
        {
            Console.WriteLine("Service: Recieve");
            if(request.Code==ServiceRequestCode.GETREPORT)
            {
                if(request.Message.Length>0)
                {
                    switch(request.Message[0])
                    {
                        case '0':
                            string Report = GetReport(x => 
                            (x.Source.Number == request.TargetNumber ||
                            x.Target.Number == request.TargetNumber));
                            ReportSend(this, request.TargetNumber + "\n" + Report);
                            break;
                        case '1':
                            Report = GetReport(x =>
                            (x.Source.Number == request.TargetNumber ||
                            x.Target.Number == request.TargetNumber) && 
                            x.Target.Number==request.Message[1..]);
                            ReportSend(this, request.TargetNumber + "\n" + Report);
                            break;
                        case '2':
                            Report = GetReport(x =>
                            (x.Source.Number == request.TargetNumber ||
                            x.Target.Number == request.TargetNumber) &&
                            x.Cost == double.Parse(request.Message[1..]));
                            ReportSend(this, request.TargetNumber + "\n" + Report);
                            break;
                        case '3':
                            Report =GetReport(x =>
                            (x.Source.Number == request.TargetNumber ||
                            x.Target.Number == request.TargetNumber) &&
                            x.StartTime.Date == DateTime.Parse(request.Message[1..]).Date);
                            ReportSend(this, request.TargetNumber + "\n" + Report);
                            break;
                    }
                }
            }
        }

        private string GetReport(Predicate<CallInfo> predicate)
        {
            StringBuilder result = new StringBuilder();
            foreach(CallInfo info in Calls)
            {
                if(predicate(info))
                {
                    result.Append(info.ToString() + '\n');
                }
            }
            if (result.Length == 0) result.Append("No call history");
            return result.ToString();
        }

        public string GetNumberByName(string name)
        {
            Client client = Clients.FirstOrDefault(x => x.Name == name);
            if(client!=null)
            {
                return client.Number;
            }
            else
            {
                throw new Exception("There is no such user");
            }
        }

        public void SignContract(string name)
        {
            if (_numberPool==null || _numberPool.Count <= 0) throw new Exception("All numbers are occupied!");
            string number = _numberPool[^1];
            Clients.Add(new Client(name,number,DateTime.Now));
            _numberPool.RemoveAt(_numberPool.Count-1);
            ContractSigned?.Invoke(this, number);
        }

        public void Link(IStation station)
        {
            station.ServiceRequested += ProcessRequest;
            station.CallEnded += ProcessCall;
        }

        public void Unlink(IStation station)
        {
            station.ServiceRequested -= ProcessRequest;
            station.CallEnded -= ProcessCall;
        }

        private void ProcessCall(object sender, CallInfo call)
        {
            call.CostPerMinute = CostPerMinute;
            call.Cost = call.EndTime.Subtract(call.StartTime).TotalMinutes * CostPerMinute;
            Calls.Add(call);
        }
    }
}
