namespace StockGraphAnalyser.Domain.Service.Interfaces
{
    public interface IDataPointManagementService
    {
        /// <summary>Inserts the new quotes into database.</summary>
        /// <param name="symbol">The symbol.</param>
        void InsertNewQuotesToDb(string symbol);

        void FillInMissingProcessedData(string symbol);
    }
}