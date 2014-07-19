namespace GraphAnalyser.Tests.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository.Interfaces;
    using StockGraphAnalyser.Domain.Service;
    using StockGraphAnalyser.Domain.Web.Interfaces;
    using StockGraphAnalyser.Processing.Types;

    public class DataPointManagementServiceTests
    {
        private Mock<IDataPointRepository> mockRepository;
        private Mock<IYahooStockQuoteServiceClient> mockYahooServiceClient;

        [SetUp]
        public void Setup() {
            this.mockRepository = new Mock<IDataPointRepository>();
            this.mockYahooServiceClient = new Mock<IYahooStockQuoteServiceClient>();
        }

        [Test]
        public void InsertNewQuotesToDbTest() {
            this.mockRepository.Setup(m => m.FindLatestDataPointDateForSymbol("SGP.L"))
                .Returns(DateTime.Today.AddDays(-7));
            var returnedQuotes = GetYahooQuotes();

            var expectedDataPoints = returnedQuotes.Select(DataPoints.CreateFromQuote).Skip(2);
            this.mockYahooServiceClient.Setup(m => m.GetQuotes("SGP.L")).Returns(returnedQuotes);
            var service = new DataPointManagementService(this.mockRepository.Object, this.mockYahooServiceClient.Object);
            service.InsertNewQuotesToDb("SGP.L");

            this.mockRepository.Verify(m => m.InsertAll(It.Is<IEnumerable<DataPoints>>(d =>
                                                                                       d.Min(x => x.Date) ==
                                                                                       expectedDataPoints.Min(
                                                                                           x => x.Date) &&
                                                                                       d.Max(x => x.Date) ==
                                                                                       expectedDataPoints.Max(
                                                                                           x => x.Date) &&
                                                                                       d.Count() ==
                                                                                       expectedDataPoints.Count()
                                                            )), Times.Once);
        }

        [Test]
        public void QuotesFoundButUpToDateDbTest() {
            this.mockRepository.Setup(m => m.FindLatestDataPointDateForSymbol("SGP.L"))
                .Returns(DateTime.Today.AddDays(-2));
            var returnedQuotes = GetYahooQuotes();

            this.mockYahooServiceClient.Setup(m => m.GetQuotes("SGP.L")).Returns(returnedQuotes);
            var service = new DataPointManagementService(this.mockRepository.Object, this.mockYahooServiceClient.Object);
            service.InsertNewQuotesToDb("SGP.L");

            this.mockRepository.Verify(m => m.InsertAll(It.IsAny<IEnumerable<DataPoints>>()), Times.Never);
        }

        private static IEnumerable<Quote> GetYahooQuotes() {
            return new List<Quote>
                {
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-8), 1m, 2m, 3m, 1m),
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-7), 1m, 2m, 3m, 1m),
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-6), 1m, 2m, 3m, 1m),
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-5), 1m, 2m, 3m, 1m),
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-4), 1m, 2m, 3m, 1m),
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-3), 1m, 2m, 3m, 1m),
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-2), 1m, 2m, 3m, 1m),
                };
        }
    }
}