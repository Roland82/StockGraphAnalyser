
namespace StockGraphAnalyser.Signals
{
    using System;

    public abstract class AbstractTradingStrategy
    {
        public abstract bool IsApplicableTo(DateTime date);
        public abstract bool SatisfiesShortCriteria(DateTime date);
        public abstract bool SatisfiesLongCriteria(DateTime date);
        public abstract bool SatisfiesTakeProfitsFromLongCriteria(DateTime date);
        public abstract bool SatisfiesTakeProfitsFromShortCriteria(DateTime date);
    }
}
