
namespace StockGraphAnalyser.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using StockGraphAnalyser.Domain.Repository.Interfaces;
    using StockGraphAnalyser.Domain.Service.Interfaces;
    using StockGraphAnalyser.Signals;
    using StockGraphAnalyser.Signals.TradingStrategies;

    /// <summary>
    /// TODO: This class needs some serious thought about how it will be more testable due to hard coded dependencies
    /// and also how equity totalling should be attached to the generated signals
    /// </summary>
    public class TradeSignalManagementService : ITradeSignalManagementService
    {
        private readonly IDataPointRepository dataPointRepository;
        private readonly ITradeSignalRepository tradeSignalRepository;
        private readonly ICompanyRepository companyRepository;

        public TradeSignalManagementService(IDataPointRepository dataPointRepository, ITradeSignalRepository tradeSignalRepository, ICompanyRepository companyRepository) {
            this.dataPointRepository = dataPointRepository;
            this.tradeSignalRepository = tradeSignalRepository;
            this.companyRepository = companyRepository;
        }

        public IEnumerable<Signal> GetAll(DateTime fromDate) {
            return this.tradeSignalRepository.GetAll(fromDate);
        }

        public IEnumerable<Signal> GetAll(string symbol) {
            return this.tradeSignalRepository.GetAllForCompany(symbol);
        }

        public void GenerateNewSignals()
        {
            this.tradeSignalRepository.DeleteAll();
            var signals = new List<Signal>();
            var indexes = new[] { Company.ConstituentOfIndex.Ftse100, Company.ConstituentOfIndex.Ftse250 };
            var datapoints = this.dataPointRepository.FindAll(indexes);

            Parallel.ForEach(datapoints.GroupBy(c => c.Symbol), e =>
                {
                    var generatedSignals = this.GenerateSignals(e.Key, e);
                    lock (signals)
                    {
                        signals.AddRange(generatedSignals);
                    }
                });

            this.tradeSignalRepository.InsertAll(signals); 
        }

        public void GenerateNewSignals(string symbol) {
            var datapoints = this.dataPointRepository.FindAll(symbol);   
            var signals = this.GenerateSignals(symbol, datapoints);

            this.tradeSignalRepository.DeleteAll(symbol);
            this.tradeSignalRepository.InsertAll(signals);
        }

        private IEnumerable<Signal> GenerateSignals(string symbol, IEnumerable<DataPoints> dataPoints) {
            var newSignals = new List<Signal>();

            var company = this.companyRepository.FindBySymbol(symbol);
            if (company.ExcludeYn == 0)
            {
                var generator = new SignalGenerator(dataPoints, this.CreateStrategies(dataPoints));
                var generatedSignals = generator.Generate().ToList();
                if (generatedSignals.Any())
                {
                    var totaller = new SignalEquityPositionTotaller(generatedSignals, 100);
                    var totals = totaller.Calculate();
                    var signalsToInsert = generatedSignals.Where(s => totals.ContainsKey(s.Date)).Select(s =>
                        {
                            s.CurrentEquity = totals[s.Date];
                            return s;
                        });

                    newSignals.AddRange(signalsToInsert);
                }
            }

            return newSignals;
        }

        private IEnumerable<AbstractTradingStrategy> CreateStrategies(IEnumerable<DataPoints> dataPoints) {
            return new AbstractTradingStrategy[] {new HighMomentumShortTradingStrategy(dataPoints), new HighMomentumBuyTradingStrategy(dataPoints)};
        } 
    }
}
