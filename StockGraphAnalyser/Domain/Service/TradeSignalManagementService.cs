
namespace StockGraphAnalyser.Domain.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using StockGraphAnalyser.Domain.Repository.Interfaces;
    using StockGraphAnalyser.Domain.Service.Interfaces;
    using StockGraphAnalyser.Signals;

    public class TradeSignalManagementService : ITradeSignalManagementService
    {
        private readonly IDataPointRepository dataPointRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly ITradeSignalRepository tradeSignalRepository;

        public TradeSignalManagementService(IDataPointRepository dataPointRepository, ICompanyRepository companyRepository, ITradeSignalRepository tradeSignalRepository)
        {
            this.dataPointRepository = dataPointRepository;
            this.companyRepository = companyRepository;
            this.tradeSignalRepository = tradeSignalRepository;
        }

        public void GenerateNewSignals(){

            var companies = new List<string>();
            companies.AddRange(this.companyRepository.FindByIndex(Company.ConstituentOfIndex.Ftse100).Select(c => c.Symbol));
            companies.AddRange(this.companyRepository.FindByIndex(Company.ConstituentOfIndex.Ftse250).Select(c => c.Symbol));
            var signals = new List<Signal>();

            foreach (var company in companies.GroupBy(c => c))
            {
                var datapoints = this.dataPointRepository.FindAll(company.Key).OrderBy(d => d.Date);
                var generator = new MovingAveragePriceCrossSignals(datapoints);
                signals.AddRange(generator.GenerateSignals());
            }

            this.tradeSignalRepository.InsertAll(signals);          
        }
    }
}
