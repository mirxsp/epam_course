using System;
namespace Task4.Model
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public virtual Client Client { get; set; }

        public virtual Item Item { get; set; }
    }
}
