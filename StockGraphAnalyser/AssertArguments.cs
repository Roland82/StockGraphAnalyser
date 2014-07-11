

using System;

namespace StockGraphAnalyser
{
    static class AssertArguments
    {
        public static void GreaterThan(decimal thisNumberIsGreaterThan, decimal thatNumber)
        {
            if (thisNumberIsGreaterThan <= thatNumber)
            {
                throw new ArgumentException();    
            }
        }

        public static void GreaterThanOrEqualTo(decimal thisNumberIsGreaterThanOrEqualTo, decimal thatNumber)
        {
            if (thisNumberIsGreaterThanOrEqualTo < thatNumber)
            {
                throw new ArgumentException();
            }
        }

        public static void EqualTo(int thisNumber, int thatNumber)
        {
            if (thisNumber != thatNumber)
            {
                throw new ArgumentException();
            }
        }
    }
}
