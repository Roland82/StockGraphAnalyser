namespace StockGraphAnalyser.Domain.Repository
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Dapper;
    using StockGraphAnalyser.Processing.Types;

    public class CompanyRepository
    {
        private const string ConnectionString = @"Server=ROLAND-PC\SQLEXPRESS;Database=StockGraphAnalyser;Trusted_Connection=True;";

        public void InsertAll(IEnumerable<Company> companies)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(@"INSERT INTO Companies VALUES (@Id,@Symbol,@Name)", companies);
            }
        }

        public IEnumerable<Company> FindAll(char letter)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<Company>(string.Format("SELECT * FROM Companies WHERE Symbol LIKE '{0}%'", letter));
            }
        }
    }
}