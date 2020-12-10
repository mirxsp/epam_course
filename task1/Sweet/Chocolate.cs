using System;
using System.Collections.Generic;
using System.Text;

namespace lab3
{
    class Chocolate : Sweet
    {
        public static int compareByPalmOil(Chocolate x, Chocolate y)
        {
            if (x == null)
            {
                if (y == null) return 0;
                else return -1;
            }
            else
            {
                if (y == null) return 0;
                return x.PalmOilAmount.CompareTo(y.PalmOilAmount);
            }
        }
        public enum Types
        {
            Dark,
            Milk,
            White,
            Mixed
        };
        public Types Type { get; set; }
        public double PalmOilAmount { get; set; }

        public double getPalmOilPercentage()
        {
            return PalmOilAmount / Weight * 100;
        }
    }
}
