
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
            var indexes = new[] { Company.ConstituentOfIndex.Ftse100, Company.ConstituentOfIndex.Ftse250, Company.ConstituentOfIndex.SmallCap };
            var datapoints = this.dataPointRepository.FindAll(indexes);
            foreach (var datapointsGroup in datapoints.GroupBy(c => c.Symbol))
            {
                this.GenerateSignals(datapointsGroup.Key, datapointsGroup);
            }

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
                var generator = new MovingAveragePriceCrossSignals(dataPoints);
                var generatedSignals = generator.GenerateSignals().ToList();
                if (generatedSignals.Any())
                {
                    newSignals.AddRange(generatedSignals);
                }
            }

            return newSignals;
        }
    }
}
