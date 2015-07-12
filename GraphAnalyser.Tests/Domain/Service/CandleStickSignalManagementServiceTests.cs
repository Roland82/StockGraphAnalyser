
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
    using StockGraphAnalyser.Processing.Candlestick;

    public class CandleStickSignalManagementServiceTests
    {
        private const string Symbol = "SGP.L";
        private Mock<IDataPointRepository> mockDatapointRepository;
        private Mock<ICandleStickSignalRepository> mockCandlestickSignalRepository;
        private Mock<ICompanyRepository> mockCompanyRepository;
        private Mock<ICandlestickPatternRecogniserFactory> candlestickPatternRecogniserFactory;

        [SetUp]
        public void Setup() {
            this.mockDatapointRepository = new Mock<IDataPointRepository>();
            this.mockCandlestickSignalRepository = new Mock<ICandleStickSignalRepository>();
            this.mockCompanyRepository = new Mock<ICompanyRepository>();
            this.candlestickPatternRecogniserFactory = new Mock<ICandlestickPatternRecogniserFactory>();
        }

        /// <summary>
        /// Proves that all pattern detector results are inserted into the DB.
        /// </summary>
        [Test]
        public void ResultsFromAllSignalDetectorsInserted() {
            this.mockDatapointRepository.Setup(o => o.FindAll(Symbol)).ReturnsAsync(new List<DataPoints>());
            this.mockCandlestickSignalRepository.Setup(o => o.FindAllForCompany(Symbol)).Returns(new List<CandleStickSignal>());

            var mockSignalDetector1 = SetupSignalDetectorMock(new[] { DateTime.Today, DateTime.Today.AddDays(7) }, 1);
            var mockSignalDetector2 = SetupSignalDetectorMock(new[] {DateTime.Today.AddDays(1), DateTime.Today.AddDays(5)}, 2);

            this.candlestickPatternRecogniserFactory.Setup(o => o.CreateAll(It.IsAny<IEnumerable<DataPoints>>()))
                .Returns(new[] {mockSignalDetector1.Object, mockSignalDetector2.Object});
            
            var service = new CandleStickSignalManagementService(
                this.mockDatapointRepository.Object,
                this.mockCandlestickSignalRepository.Object,
                this.mockCompanyRepository.Object,
                this.candlestickPatternRecogniserFactory.Object
                );

            service.GenerateLatestSignals(Symbol, DateTime.MinValue);
            this.VerifyCandlestickRepositoryCalledWith(1, DateTime.Today);
            this.VerifyCandlestickRepositoryCalledWith(1, DateTime.Today.AddDays(7));
            this.VerifyCandlestickRepositoryCalledWith(2, DateTime.Today.AddDays(1));
            this.VerifyCandlestickRepositoryCalledWith(2, DateTime.Today.AddDays(5));
        }

        /// <summary>
        /// Proves that if a signal is already in the DB and the algo has a signal for that date, then its not inserted a second time.
        /// </summary>
        [Test]
        public void SignalAlreadyDetectedForDateTest() {
            var candlestickSignalsInDb = new[] {CandleStickSignal.Create(Symbol, DateTime.Today, 1)};           
            this.mockCandlestickSignalRepository.Setup(o => o.FindAllForCompany(Symbol)).Returns(candlestickSignalsInDb);
            this.mockDatapointRepository.Setup(o => o.FindAll(Symbol)).ReturnsAsync(new List<DataPoints>());
            var mockSignalDetector = SetupSignalDetectorMock(new[] { DateTime.Today, DateTime.Today.AddDays(5) }, 1);

            this.candlestickPatternRecogniserFactory.Setup(o => o.CreateAll(It.IsAny<IEnumerable<DataPoints>>()))
                .Returns(new[] { mockSignalDetector.Object });

            var service = new CandleStickSignalManagementService(
                this.mockDatapointRepository.Object,
                this.mockCandlestickSignalRepository.Object,
                this.mockCompanyRepository.Object,
                this.candlestickPatternRecogniserFactory.Object
                );

            service.GenerateLatestSignals(Symbol, DateTime.MinValue);
            this.mockCandlestickSignalRepository.Verify(o => o.InsertAll(It.Is<IEnumerable<CandleStickSignal>>(l => l.Count() == 1)));
            this.VerifyCandlestickRepositoryCalledWith(1, DateTime.Today.AddDays(5));
        }

        private static Mock<IDetectPattern> SetupSignalDetectorMock(IEnumerable<DateTime> datesDetected, int signalType) {
            var mockSignalDetector = new Mock<IDetectPattern>();
            mockSignalDetector.Setup(o => o.FindOccurences()).Returns(datesDetected);
            mockSignalDetector.SetupGet(o => o.PatternType).Returns(signalType);
            return mockSignalDetector;
        }


        private void VerifyCandlestickRepositoryCalledWith(int candlestickType, DateTime date) {
            this.mockCandlestickSignalRepository.Verify(o => o.InsertAll(
                It.Is<IEnumerable<CandleStickSignal>>(l => l.Any(e => e.Date == date && e.CandleStickSignalType == candlestickType && e.Symbol == Symbol)))
                );
        }
    }
}
