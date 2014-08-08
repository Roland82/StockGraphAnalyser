﻿
namespace StockGraphAnalyser.Processing.Types
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

        public static Company Create(String name, String symbol, ConstituentOfIndex constituentOfIndex)
        {
            return new Company(name, symbol, constituentOfIndex);
        }
    }
}
