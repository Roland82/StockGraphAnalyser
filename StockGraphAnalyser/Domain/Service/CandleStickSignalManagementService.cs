
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
            var bullishEngulfingOccurences = new EngulfingPatterRecogniser(datapointsToCheck, EngulfingPatterRecogniser.Type.Bullish).FindOccurences();
            var bearishEngulfingOccurences = new EngulfingPatterRecogniser(datapointsToCheck, EngulfingPatterRecogniser.Type.Bearish).FindOccurences();
            var signalList = new List<CandleStickSignal>();

            signalList.AddRange(bullishEngulfingOccurences.Select(o => CandleStickSignal.Create(symbol, o, 1)));
            signalList.AddRange(bearishEngulfingOccurences.Select(o => CandleStickSignal.Create(symbol, o, 2)));

            this.candleStickSignalRepository.InsertAll(signalList);
        }
    }
}
