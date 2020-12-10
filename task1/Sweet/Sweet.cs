using System;
using System.Collections.Generic;
using System.Text;

namespace lab3
{
    class Sweet
    {
        public static int CompareSweetsBySugarPercentage(Sweet x, Sweet y)
        {
            if (x == null)
            {
                if (y == null) return 0;
                else return -1;
            }
            else
            {
                if (y == null) return 0;
                return x.SugarPercentage.CompareTo(y.SugarPercentage);
            }
        }
        public static int CompareSweetsByName(Sweet x, Sweet y)
        {
            if (x == null)
            {
                if (y == null) return 0;
                else return -1;
            }
            else
            {
                if (y == null) return 0;
                return x.Name.CompareTo(y.Name);
            }
        }

        public static int CompareSweetsByWeight(Sweet x,Sweet y)
        {
            if (x == null)
            {
                if (y == null) return 0;
                else return -1;
            }
            else
            {
                if (y == null) return 0;
                return x.Weight.CompareTo(y.Weight);
            }
        }

        public double Weight { get; set; }
        public double SugarAmount { get; set; }
        public double SugarPercentage
        {
            get
            {
                return SugarAmount / Weight * 100;
            }
        }
        public string Name { get; set; }


    }
}
