
namespace StockGraphAnalyser.Domain
{
    using System;

    public class Company
    {
        public enum ConstituentOfIndex
        {
            Unknown = 0,
            Ftse100 = 1,
            Ftse250 = 2,
            SmallCap = 3
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public short ExcludeYn { get; set; }
        
        /// <summary>
        /// TODO: Turn this into an enum when i know how to use dapper properly
        /// </summary>
        public ConstituentOfIndex Index { get; set; }

        private Company(String name, String symbol, ConstituentOfIndex constituentOfIndex)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Symbol = symbol;
            this.Index = constituentOfIndex;
            this.ExcludeYn = 0;
        }

        public Company() {}

        public static Company Create(String name, String symbol, ConstituentOfIndex constituentOfIndex)
        {
            return new Company(name, symbol, constituentOfIndex);
        }
    }
}
