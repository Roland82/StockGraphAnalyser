
namespace StockGraphAnalyser.Processing.Types
{
    using System;

    public class Company
    {
        public enum ConstituentOfIndex
        {
            Ftse100 = 1
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        
        /// <summary>
        /// TODO: Turn this into an enum when i know how to use dapper properly
        /// </summary>
        public int Index { get; set; }

        private Company(String name, String symbol, ConstituentOfIndex constituentOfIndex)
        {
            Id = Guid.NewGuid();
            Name = name;
            Symbol = symbol;
            Index = constituentOfIndex.GetHashCode();
        }

        public Company() {}

        internal static Company Create(String name, String symbol, ConstituentOfIndex constituentOfIndex)
        {
            return new Company(name, symbol, constituentOfIndex);
        }
    }
}
