using System;
using System.Collections.Generic;
using System.Text;

namespace lab3
{

    class Sugar : Sweet
    {
        public static int compareByHardness(Sugar x,Sugar y)
        {
            if (x == null)
            {
                if (y == null) return 0;
                else return -1;
            }
            else
            {
                if (y == null) return 0;
                return x.Hardness.CompareTo(y.Hardness);
            }
        }
        public enum Types
        {
            Noncrystaline, //Homogeneous (hard\chewy)
            Crystaline //Include small crystall (creamy)
        };


        public Types Type { get; set; }
        public double Hardness { get; set; }

    }
}
