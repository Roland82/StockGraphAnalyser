
namespace GraphAnalyser.Tests.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using StockGraphAnalyser.Domain;

    public partial class DataPointManagementServiceTests
    {
        private static IEnumerable<Quote> OtherTestQuotes
        {
            get
            {
                return new List<Quote>
                    {
                        Quote.Create("SGP.L", DateTime.Today.AddDays(-8), 1m, 2m, 3m, 1m, 100),
                        Quote.Create("SGP.L", DateTime.Today.AddDays(-7), 1m, 2m, 3m, 1m, 100),
                        Quote.Create("SGP.L", DateTime.Today.AddDays(-6), 1m, 2m, 3m, 1m, 100),
                        Quote.Create("SGP.L", DateTime.Today.AddDays(-5), 1m, 2m, 3m, 1m, 100),
                        Quote.Create("SGP.L", DateTime.Today.AddDays(-4), 1m, 2m, 3m, 1m, 100),
                        Quote.Create("SGP.L", DateTime.Today.AddDays(-3), 1m, 2m, 3m, 1m, 100),
                        Quote.Create("SGP.L", DateTime.Today.AddDays(-2), 1m, 2m, 3m, 1m, 100),
                    };
            }
        }

    }
}
