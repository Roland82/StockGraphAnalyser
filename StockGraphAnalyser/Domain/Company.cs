
namespace StockGraphAnalyser.Domain
{
    using System;
    using Cassandra;

    public class Company
    {
        public enum ConstituentOfIndex
        {
            Unknown = 0,
            Ftse100 = 1,
            Ftse250 = 2,
            FtseSmallCap = 3
        }

        public string Name { get; set; }
        public string Symbol { get; set; }
        public int ExcludeYn { get; set; }
        
        /// <summary>
        /// TODO: Turn this into an enum when i know how to use dapper properly
        /// </summary>
        public ConstituentOfIndex Index { get; set; }

        private Company(String name, String symbol, ConstituentOfIndex constituentOfIndex)
        {
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

        public static Company CreateFromRow(Row row) {
            return new Company
                {
                    Name = row.GetValue<string>("Name"),
                    Symbol = row.GetValue<string>("Symbol"),
                    Index = (ConstituentOfIndex) row.GetValue<int>("Index"),
                    ExcludeYn = row.GetValue<int>("ExcludeYn")
                };
        }
    }
}
