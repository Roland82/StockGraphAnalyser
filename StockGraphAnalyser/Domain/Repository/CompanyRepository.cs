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

        public void UpdateAll(IEnumerable<Company> companies)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var company in companies)
                    {
                        connection.Execute(
                            @"UPDATE Companies SET [Index] = @Index Where Id = @Id",
                            new { Index = company.Index, Id = company.Id }, 
                            transaction: transaction);
                    }  

                    transaction.Commit();
                }
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

        public IEnumerable<Company> FindByIndex(Company.ConstituentOfIndex index)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<Company>(string.Format("SELECT * FROM Companies WHERE [Index] = {0}", index.GetHashCode()));
            }
        }

        public IEnumerable<Company> FindAll()
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<Company>(string.Format("SELECT * FROM Companies"));
            }
        }
    }
}