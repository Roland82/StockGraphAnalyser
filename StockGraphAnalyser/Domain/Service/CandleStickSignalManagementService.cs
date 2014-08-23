
namespace StockGraphAnalyser.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Processing.Candlestick;
    using Repository.Interfaces;

    public class CandleStickSignalManagementService : ICandleStickSignalManagementService
    {
        private readonly IDataPointRepository datapointsRepository;
        private readonly ICandleStickSignalRepository candleStickSignalRepository;

        public CandleStickSignalManagementService(IDataPointRepository datapointsRepository, ICandleStickSignalRepository candleStickSignalRepository) {
            this.datapointsRepository = datapointsRepository;
            this.candleStickSignalRepository = candleStickSignalRepository;
        }

        public void GenerateLatestSignals(string symbol, DateTime fromDate) {
            var datapointsToCheck = this.datapointsRepository.FindAll(symbol).Where(d => d.Date >= fromDate).ToList();
            var bullishEngulfingLatestOccurence = new EngulfingPatterRecogniser(datapointsToCheck, EngulfingPatterRecogniser.Type.Bullish).LatestOccurence();
            var bearishEngulfingLatestOccurence = new EngulfingPatterRecogniser(datapointsToCheck, EngulfingPatterRecogniser.Type.Bearish).LatestOccurence();
            var signalList = new List<CandleStickSignal>();
            
            if (bullishEngulfingLatestOccurence.HasValue)
            {
                signalList.Add(CandleStickSignal.Create(symbol, bullishEngulfingLatestOccurence.Value, 1));
            }

            if (bearishEngulfingLatestOccurence.HasValue)
            {
                signalList.Add(CandleStickSignal.Create(symbol, bearishEngulfingLatestOccurence.Value, 2));
            }

            this.candleStickSignalRepository.InsertAll(signalList);
        }
    }
}
