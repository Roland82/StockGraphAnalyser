namespace GraphAnalyser.Tests.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Moq;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository.Interfaces;
    using StockGraphAnalyser.Domain.Service;
    using StockGraphAnalyser.Domain.Web.Interfaces;
    using StockGraphAnalyser.Processing.Calculators;
    using StockGraphAnalyser.Processing.Types;
    using TestUtilities;

    public partial class DataPointManagementServiceTests
    {
        private readonly DateTime monday = new DateTime(2014, 8, 4);
        private Mock<IDataPointRepository> mockRepository;
        private Mock<IYahooStockQuoteServiceClient> mockYahooServiceClient;
        private Mock<ICalculatorFactory> mockCalculatorFactory;

        private DataPoints CreateDataPoint(int addDays, bool processed) {
            return new DataPoints
                {
                    Symbol = "SGP.L", Date = monday.AddDays(addDays), Open = 1m,
                    Close = 2m, High = 3m, Low = 1m, Volume = 100, IsProcessed = processed
                };
        }

        private IEnumerable<DataPoints> TestDataPoints {
            get {
                return new List<DataPoints>
                    {
                        this.CreateDataPoint(0, true),
                        this.CreateDataPoint(1, true),
                        this.CreateDataPoint(2, true),
                        this.CreateDataPoint(3, true),
                        this.CreateDataPoint(4, true),
                        this.CreateDataPoint(7, false),
                        this.CreateDataPoint(8, false)
                    };
            }
        }

        [SetUp]
        public void Setup() {
            this.mockRepository = new Mock<IDataPointRepository>();
            this.mockYahooServiceClient = new Mock<IYahooStockQuoteServiceClient>();
            this.mockCalculatorFactory = new Mock<ICalculatorFactory>();
        }

        //[Test]
        //public void InsertNewQuotesToDbTest() {
        //    this.mockRepository.Setup(m => m.FindLatestDataPointDateForSymbol("SGP.L")).Returns(monday.AddDays(1));

        //    this.mockYahooServiceClient.Setup(m => m.GetQuotes("SGP.L")).Returns(TestQuotes);
        //    var service = new DataPointManagementService(this.mockRepository.Object, this.mockYahooServiceClient.Object,
        //                                                 this.mockCalculatorFactory.Object);
        //    service.InsertNewQuotesToDb("SGP.L");

        //    this.mockRepository.Verify(m => m.InsertAll(It.Is<IEnumerable<DataPoints>>(d =>
        //                                                                               d.Min(x => x.Date) ==
        //                                                                               expectedDataPoints.Min(
        //                                                                                   x => x.Date) &&
        //                                                                               d.Max(x => x.Date) ==
        //                                                                               expectedDataPoints.Max(
        //                                                                                   x => x.Date) &&
        //                                                                               d.Count() ==
        //                                                                               expectedDataPoints.Count()
        //                                                    )), Times.Once);
        //}

        //[Test]
        //public void QuotesFoundButUpToDateDbTest() {
        //    this.mockRepository.Setup(m => m.FindLatestDataPointDateForSymbol("SGP.L"))
        //        .Returns(DateTime.Today.AddDays(-2));

        //    this.mockYahooServiceClient.Setup(m => m.GetQuotes("SGP.L")).Returns(TestQuotes);
        //    var service = new DataPointManagementService(this.mockRepository.Object, this.mockYahooServiceClient.Object,
        //                                                 this.mockCalculatorFactory.Object);
        //    service.InsertNewQuotesToDb("SGP.L");

        //    this.mockRepository.Verify(m => m.InsertAll(It.IsAny<IEnumerable<DataPoints>>()), Times.Never);
        //}

        [Test]
        public void TestUpdate() {
            this.BuildCalcuatorFactory();
            this.mockRepository.Setup(f => f.FindAll("SGP.L")).Returns(TestDataPoints);
            var service = new DataPointManagementService(this.mockRepository.Object, this.mockYahooServiceClient.Object, this.mockCalculatorFactory.Object);
            service.FillInMissingProcessedData("SGP.L");

            var expectedDatapointsUpdated = new List<DataPoints>
                {
                    new DataPoints
                        {
                            Symbol = "SGP.L",
                            Date = monday.AddDays(7),
                            Open = 1m,
                            Close = 2m,
                            High = 3m,
                            Low = 1m,
                            Volume = 100,
                            MovingAverageTwoHundredDay = 6m,
                            MovingAverageFiftyDay = 7,
                            LowerBollingerBand = 9,
                            UpperBollingerBand = 10,
                            ForceIndexOnePeriod = 11m,
                            ForceIndexThirteenPeriod = It.IsAny<Decimal>(),
                            IsProcessed = true
                        },
                    new DataPoints
                        {
                            Symbol = "SGP.L",
                            Date = monday.AddDays(8),
                            Open = 1m,
                            Close = 2m,
                            High = 3m,
                            Low = 1m,
                            Volume = 100,
                            MovingAverageTwoHundredDay = 7m,
                            MovingAverageFiftyDay = 8m,
                            LowerBollingerBand = 10,
                            UpperBollingerBand = 11,
                            ForceIndexOnePeriod = 12m,
                            ForceIndexThirteenPeriod = It.IsAny<Decimal>(),
                            IsProcessed = true
                        },
                };
            this.mockRepository.Verify(f => f.UpdateAll(expectedDatapointsUpdated));
        }

        private void BuildCalcuatorFactory() {
            this.SetupCalculator(
                factory => factory.CreateMovingAverageCalculator(It.IsAny<Dictionary<DateTime, decimal>>(), 200),
                new[] {1m, 2m, 3m, 4m, 5m, 6m, 7m });

            this.SetupCalculator(
                factory => factory.CreateMovingAverageCalculator(It.IsAny<Dictionary<DateTime, decimal>>(), 50),
                new[] {2m, 3m, 4m, 5m, 6m, 7m, 8m });

            this.SetupCalculator(
                factory => factory.CreateMovingAverageCalculator(It.IsAny<Dictionary<DateTime, decimal>>(), 20),
                new[] {3m, 4m, 5m, 6m, 7m, 8m, 9m });

            this.SetupCalculator(
                factory =>
                factory.CreateBollingerBandCalculator(It.IsAny<Dictionary<DateTime, decimal>>(),
                                                      It.IsAny<Dictionary<DateTime, decimal>>(),
                                                      BollingerBandCalculator.Band.Lower),
                new[] {4m, 5m, 6m, 7m, 8m, 9m, 10m });

            this.SetupCalculator(
                factory =>
                factory.CreateBollingerBandCalculator(It.IsAny<Dictionary<DateTime, decimal>>(),
                                                      It.IsAny<Dictionary<DateTime, decimal>>(),
                                                      BollingerBandCalculator.Band.Upper),
                new[] {5m, 6m, 7m, 8m, 9m, 10m, 11m });

            this.SetupCalculator(
                factory => factory.CreateForceIndexCalculator(It.IsAny<IEnumerable<Tuple<DateTime, decimal, long>>>()),
                new[] {6m, 7m, 8m, 9m, 10m, 11m, 12m });

            this.SetupCalculator(
                factory =>
                factory.CreateExponentialMovingAverageCalculator(It.IsAny<Dictionary<DateTime, decimal>>(), 13),
                new[] {7m, 8m, 9m, 10m, 11m, 12m, 13m });

            // Standard deviation calculation result doesnt matter for the purposes of the tests we are writing here.
            this.SetupCalculator(
                factory =>
                factory.CreateStandardDeviationCalculator(It.IsAny<Dictionary<DateTime, decimal>>(), It.IsAny<int>()),
                new decimal[] {});
        }

        private void SetupCalculator(Expression<Func<ICalculatorFactory, ICalculationTool>> calculatorFactorySetup, decimal[] calculationSetup) {
            var returnValues = GraphPlottingUtilities.CreateGraph(monday, calculationSetup);
            var calculator = new Mock<ICalculationTool>();
            calculator.Setup(c => c.CalculateAsync(It.IsAny<DateTime>())).ReturnsAsync(returnValues);
            this.mockCalculatorFactory
                .Setup(calculatorFactorySetup)
                .Returns(calculator.Object);
        }
    }
}