
namespace StockGraphAnalyser.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Processing.Candlestick;
    using Repository.Interfaces;

    public class CandleStickSignalManagementService : ICandleStickSignalManagementService
    {
        private readonly IDataPointRepository datapointsRepository;
        private readonly ICandleStickSignalRepository candleStickSignalRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly ICandlestickPatternRecogniserFactory candlestickPatternFactory;

        public CandleStickSignalManagementService(IDataPointRepository datapointsRepository, ICandleStickSignalRepository candleStickSignalRepository, ICompanyRepository companyRepository, ICandlestickPatternRecogniserFactory candlestickPatternFactory) {
            this.datapointsRepository = datapointsRepository;
            this.candleStickSignalRepository = candleStickSignalRepository;
            this.companyRepository = companyRepository;
            this.candlestickPatternFactory = candlestickPatternFactory;
        }

        public void GenerateLatestSignals(string symbol, DateTime fromDate) {
            this.InsertSignalsForCompany(symbol, fromDate);
        }

        public void GenerateLatestSignals(DateTime fromDate) {
            var companies = new List<Company>();
            companies.AddRange(this.companyRepository.FindByIndex(Company.ConstituentOfIndex.Ftse250));
            companies.AddRange(this.companyRepository.FindByIndex(Company.ConstituentOfIndex.Ftse100));

            foreach (var company in companies)
            {
                this.InsertSignalsForCompany(company.Symbol, fromDate);
            }
        }

        public IEnumerable<CandleStickSignal> GetAllSignalsForCompany(string symbol)
        {
            return this.candleStickSignalRepository.FindAllForCompany(symbol);
        }

        private void InsertSignalsForCompany(string symbol, DateTime fromDate) {
            var datapointsToCheck = this.datapointsRepository.FindAll(symbol).Where(d => d.Date >= fromDate).ToList();
            var calculators = this.candlestickPatternFactory.CreateAll(datapointsToCheck);
            var detectedSignals = new List<CandleStickSignal>();
            Parallel.ForEach(calculators, pattern =>
                {
                    var occurences = pattern.FindOccurences();
                    var signals = occurences.Select(o => CandleStickSignal.Create(symbol, o, pattern.PatternType));
                    detectedSignals.AddRange(signals);
                });

            if (detectedSignals.Any())
            {
                var signalsInDb = this.candleStickSignalRepository.FindAllForCompany(symbol);
                var signalsToInsert = detectedSignals.Where(e => signalsInDb.All(s => s.Date != e.Date));
                this.candleStickSignalRepository.InsertAll(signalsToInsert);
            }
        }
    }
}
