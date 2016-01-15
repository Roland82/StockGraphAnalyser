
namespace StockGraphAnalyser.Domain.Repository
{
    using System;

    public abstract class AbstractRepository
    {
        protected readonly string connectionString;

        protected AbstractRepository()
        {
            this.connectionString =
                string.Format(@"Server={0}\SQLEXPRESS;Database=StockGraphAnalyser;Trusted_Connection=True;", Environment.MachineName);
        }

        
    }
}
