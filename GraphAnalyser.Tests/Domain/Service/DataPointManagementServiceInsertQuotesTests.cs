namespace GraphAnalyser.Tests.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using Moq;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository.Interfaces;
    using StockGraphAnalyser.Domain.Service;
    using StockGraphAnalyser.Domain.StockDataProviders.Interfaces;
    using StockGraphAnalyser.Processing.Calculators;
    using TestUtilities;

    public partial class DataPointManagementServiceTests
    {
        private readonly DateTime monday = new DateTime(2014, 8, 4);
        private Mock<IDataPointRepository> dataPointRepo;
        private Mock<ICompanyRepository> companyRepository;
        private Mock<IYahooStockQuoteServiceClient> yahooServiceClient;
        private Mock<ICalculatorFactory> calculatorFactory;


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

        private IEnumerable<Quote> TestQuotes {
            get {
                for (var i = 0; i <= 6; i++)
                {
                    yield return this.CreateQuote(i);
                }
            }
        }
            
        [SetUp]
        public void Setup() {
            this.dataPointRepo = new Mock<IDataPointRepository>();
            this.yahooServiceClient = new Mock<IYahooStockQuoteServiceClient>();
            this.calculatorFactory = new Mock<ICalculatorFactory>();
        }

        [Test]
        public void InsertNewQuotesToDbTest() {
            this.BuildCalcuatorFactory();
            var latestDataPointDate = monday.AddDays(1);
            this.dataPointRepo.Setup(m => m.FindLatestDataPointDateForSymbol("SGP.L")).Returns(latestDataPointDate);
            this.yahooServiceClient.Setup(m => m.GetQuotes("SGP.L")).Returns(TestQuotes);
            
            var service = new DataPointManagementService(this.dataPointRepo.Object, this.companyRepository.Object, this.yahooServiceClient.Object, this.calculatorFactory.Object);
            service.InsertNewQuotesToDb("SGP.L");

            var expectedDataPoints = TestQuotes.Where(q => q.Date > latestDataPointDate).Select(DataPoints.CreateFromQuote).ToList();

            for (var i = 0; i <= 4; i++)
            {
                this.dataPointRepo.Verify(m => m.InsertAll(It.Is<List<DataPoints>>(d => d[i].Equals(expectedDataPoints[i]))));
            }
        }

        [Test]
        public void QuotesFoundButUpToDateDbTest()
        {
            this.dataPointRepo.Setup(m => m.FindLatestDataPointDateForSymbol("SGP.L")).Returns(monday.AddDays(6));
            this.yahooServiceClient.Setup(m => m.GetQuotes("SGP.L")).Returns(TestQuotes);

            var service = new DataPointManagementService(this.dataPointRepo.Object, this.companyRepository.Object, this.yahooServiceClient.Object, this.calculatorFactory.Object);
            service.InsertNewQuotesToDb("SGP.L");

            this.dataPointRepo.Verify(m => m.InsertAll(It.IsAny<IEnumerable<DataPoints>>()), Times.Never);
        }

        [Test]
        public void TestUpdate() {
            this.BuildCalcuatorFactory();
            this.dataPointRepo.Setup(f => f.FindAll("SGP.L")).ReturnsAsync(TestDataPoints);
            var service = new DataPointManagementService(this.dataPointRepo.Object, this.companyRepository.Object, this.yahooServiceClient.Object, this.calculatorFactory.Object);

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
                            MovingAverageTwentyDay = 8,
                            LowerBollingerBandTwoDeviation = 9,
                            UpperBollingerBandTwoDeviation = 10,
                            ForceIndexOnePeriod = 11m,
                            ForceIndexThirteenPeriod = 12m,
                            IsProcessed = 1
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
                            MovingAverageTwentyDay = 9,
                            LowerBollingerBandTwoDeviation = 10,
                            UpperBollingerBandTwoDeviation = 11,
                            ForceIndexOnePeriod = 12m,
                            ForceIndexThirteenPeriod = 13m,
                            IsProcessed = 1
                        },
                };

            this.dataPointRepo.Verify(f => f.UpdateAll(It.Is<List<DataPoints>>(c => c.Count == 2)));
            this.dataPointRepo.Verify(f => f.UpdateAll(It.Is<List<DataPoints>>(c => c[0].Equals(expectedDatapointsUpdated[0]))));
            this.dataPointRepo.Verify(f => f.UpdateAll(It.Is<List<DataPoints>>(c => c[1].Equals(expectedDatapointsUpdated[1]))));      
        }

        private void BuildCalcuatorFactory() {
            this.SetupCalculator(
                factory => factory.CreateMovingAverageCalculator(It.IsAny<ReadOnlyDictionary<DateTime, decimal>>(), 200),
                new[] {1m, 2m, 3m, 4m, 5m, 6m, 7m });

            this.SetupCalculator(
                factory => factory.CreateMovingAverageCalculator(It.IsAny<ReadOnlyDictionary<DateTime, decimal>>(), 50),
                new[] {2m, 3m, 4m, 5m, 6m, 7m, 8m });

            this.SetupCalculator(
                factory => factory.CreateMovingAverageCalculator(It.IsAny<ReadOnlyDictionary<DateTime, decimal>>(), 20),
                new[] {3m, 4m, 5m, 6m, 7m, 8m, 9m });

            this.SetupCalculator(
                factory =>
                factory.CreateBollingerBandCalculator(It.IsAny<Dictionary<DateTime, decimal>>(),
                                                      It.IsAny<Dictionary<DateTime, decimal>>(),
                                                      BollingerBandCalculator.Band.LowerTwoDeviation),
                new[] {4m, 5m, 6m, 7m, 8m, 9m, 10m });

            this.SetupCalculator(
                factory =>
                factory.CreateBollingerBandCalculator(It.IsAny<Dictionary<DateTime, decimal>>(),
                                                      It.IsAny<Dictionary<DateTime, decimal>>(),
                                                      BollingerBandCalculator.Band.UpperTwoDeviation),
                new[] {5m, 6m, 7m, 8m, 9m, 10m, 11m });

            this.SetupCalculator(
                factory => factory.CreateForceIndexCalculator(It.IsAny<IEnumerable<Tuple<DateTime, decimal, long>>>()),
                new[] {6m, 7m, 8m, 9m, 10m, 11m, 12m });

            this.SetupCalculator(
                factory =>
                factory.CreateExponentialMovingAverageCalculator(It.IsAny<ReadOnlyDictionary<DateTime, decimal>>(), 13),
                new[] {7m, 8m, 9m, 10m, 11m, 12m, 13m });

            this.SetupCalculator(
                factory =>
                factory.CreateExponentialMovingAverageCalculator(It.IsAny<ReadOnlyDictionary<DateTime, decimal>>(), 12),
                new[] { 8m, 9m, 10m, 11m, 12m, 13m, 14m });

            this.SetupCalculator(
                factory =>
                factory.CreateDifferenceCalculator(It.IsAny<Dictionary<DateTime, decimal>>(), It.IsAny<Dictionary<DateTime, decimal>>()),
                new[] { 9m, 10m, 11m, 12m, 13m, 14m, 15m });

            this.SetupCalculator(
                factory =>
                factory.CreateExponentialMovingAverageCalculator(It.IsAny<ReadOnlyDictionary<DateTime, decimal>>(), 22),
                new[] { 9m, 10m, 11m, 12m, 13m, 14m, 15m });

            // Standard deviation calculation result doesnt matter for the purposes of the tests we are writing here.
            this.SetupCalculator(
                factory =>
                factory.CreateStandardDeviationCalculator(It.IsAny<ReadOnlyDictionary<DateTime, decimal>>(), It.IsAny<int>()),
                new decimal[] {});
        }

        private void SetupCalculator(Expression<Func<ICalculatorFactory, ICalculationTool>> calculatorFactorySetup, decimal[] calculationSetup) {
            var returnValues = GraphPlottingUtilities.CreateGraph(monday, calculationSetup);
            var calculator = new Mock<ICalculationTool>();
            calculator.Setup(c => c.CalculateAsync(It.IsAny<DateTime>())).ReturnsAsync(returnValues);
            this.calculatorFactory
                .Setup(calculatorFactorySetup)
                .Returns(calculator.Object);
        }

        private DataPoints CreateDataPoint(int addDays, bool processed)
        {
            return new DataPoints
            {
                Symbol = "SGP.L",
                Date = monday.AddDays(addDays),
                Open = 1m,
                Close = 2m,
                High = 3m,
                Low = 1m,
                Volume = 100,
                IsProcessed = processed ? (short)1 : (short)0
            };
        }

        private Quote CreateQuote(int addDays) {
            return Quote.Create("SGP.L", monday.AddDays(addDays), 1m, 2m, 3m, 1m, 100);
        }
    }
}