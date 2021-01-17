using System;

namespace ATS.BS.Model
{
    class Client
    {
        public string Name { get; private set; }

        public string Number { get; private set; }

        public DateTime RegistrationDate { get; private set; }

        public Client(string Name,string Number,DateTime RegistrationDate)
        {
            this.Name = Name;
            this.Number = Number;
            this.RegistrationDate = RegistrationDate;
        }
    }
}
