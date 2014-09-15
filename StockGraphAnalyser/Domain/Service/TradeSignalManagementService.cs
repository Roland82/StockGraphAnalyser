
namespace StockGraphAnalyser.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StockGraphAnalyser.Domain.Repository.Interfaces;
    using StockGraphAnalyser.Domain.Service.Interfaces;
    using StockGraphAnalyser.Signals;

    public class TradeSignalManagementService : ITradeSignalManagementService
    {
        private readonly IDataPointRepository dataPointRepository;
        private readonly ITradeSignalRepository tradeSignalRepository;
        private readonly ICandleStickSignalRepository candleStickSignalRepository;
        private readonly ICompanyRepository companyRepository;

        public TradeSignalManagementService(IDataPointRepository dataPointRepository, ITradeSignalRepository tradeSignalRepository, ICandleStickSignalRepository candleStickSignalRepository, ICompanyRepository companyRepository) {
            this.dataPointRepository = dataPointRepository;
            this.tradeSignalRepository = tradeSignalRepository;
            this.candleStickSignalRepository = candleStickSignalRepository;
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
            this.GenerateSignals();  
        }

        public void GenerateNewSignals(string company) {
            throw new NotImplementedException("Fix this method");
        }

        private void GenerateSignals()
        {
            var signals = new List<Signal>();
            var indexes = new[] { Company.ConstituentOfIndex.Ftse100, Company.ConstituentOfIndex.Ftse250, Company.ConstituentOfIndex.SmallCap };
            var datapoints = this.dataPointRepository.FindAll(indexes);
            foreach (var datapointsGroup in datapoints.GroupBy(c => c.Symbol))
            {
                var company = this.companyRepository.FindBySymbol(datapointsGroup.Key);
                if (company.ExcludeYn == 0)
                {
                    var generator = new MovingAveragePriceCrossSignals(datapointsGroup,this.candleStickSignalRepository.FindAllForCompany(datapointsGroup.Key));
                    var generatedSignals = generator.GenerateSignals();
                    if (generatedSignals.Any())
                    {
                        signals.AddRange(generatedSignals);
                    }
                }
            }

            this.tradeSignalRepository.InsertAll(signals);
        }
    }
}
