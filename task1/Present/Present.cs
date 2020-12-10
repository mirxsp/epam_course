using System;
using System.Collections.Generic;
using System.Text;

namespace lab3
{
    class Present
    {
        public List<Sweet> Sweets { get; private set; }
        public double Weight { get; private set; }

        public Present()
        {
            Sweets = new List<Sweet>();
            Weight = 0;
        }

        public Present(List<Sweet> sweets)
        {
            Sweets = sweets;
            foreach(Sweet obj in Sweets)
            {
                Weight += obj.Weight;
            }
        }
        public void Add(Sweet sweet)
        {
            Sweets.Add(sweet);
            Weight += sweet.Weight;
        }
        public void Remove(Sweet sweet)
        {
            if (!Sweets.Remove(sweet))
            {
                throw new ArgumentOutOfRangeException();
            }
            Weight -= sweet.Weight;
        }
        public void RemoveAt(int index)
        {
            if (index < 0 || index > Sweets.Count) throw new ArgumentOutOfRangeException();
            Weight -= Sweets[index].Weight;
            Sweets.RemoveAt(index);
        }

        public void Sort(Comparison<Sweet> comparer)
        {
            Sweets.Sort(comparer);
        }

        public Sweet FindFirstBySugarPercentageRange(double minimumPercentage, double maximumPercentage)
        {
            return Sweets.Find(x => x.SugarPercentage >= minimumPercentage && x.SugarPercentage <= maximumPercentage);
        }
        public Sugar FindFirstSugarCandy()
        {
            return (Sugar)Sweets.Find(x => x is Sugar);
        }
        public Chocolate FindFirstChocolateCandy()
        {
            return (Chocolate)Sweets.Find(x => x is Chocolate);
        }

    }
}
