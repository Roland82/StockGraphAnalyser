namespace StockGraphAnalyser.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Dapper;
    using Interfaces;

    public class CompanyRepository  : AbstractRepository, ICompanyRepository
    {
        public void InsertAll(IEnumerable<Company> companies)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(@"INSERT INTO Companies VALUES (@Id,@Symbol,@Name, @Index)", companies);
            }
        }

        public void UpdateAll(IEnumerable<Company> companies)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
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

        public IEnumerable<Company> FindAll(string wordMatch)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Company>(string.Format("SELECT * FROM Companies WHERE Symbol LIKE '%{0}%' OR Name LIKE '%{0}%'", wordMatch));
            }
        }

        public IEnumerable<Company> FindByIndex(Company.ConstituentOfIndex index)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Company>(string.Format("SELECT * FROM Companies WHERE [Index] = {0}", index.GetHashCode()));
            }
        }

        public IEnumerable<Company> FindAll()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Company>(string.Format("SELECT * FROM Companies"));
            }
        }
    }
}