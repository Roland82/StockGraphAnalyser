

namespace StockGraphAnalyser.Domain.Repository.Cassandra
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using GraphAnalyser.Tests.Domain.Repository.Cassandra;
    using StockGraphAnalyser.Domain.Repository.Interfaces;
    using global::Cassandra;

    public class CompanyRepository : AbstractCassandraRepository, ICompanyRepository 
    {
        private readonly PreparedStatement upsertStatement;

        public CompanyRepository() {

            this.upsertStatement =
                Connection.Session.Prepare(
                    @"UPDATE companies SET ""Index"" = ?, ""Name"" = ?, ""ExcludeYn"" = ? WHERE ""Symbol"" = ?");
        }

        public void InsertAll(IEnumerable<Company> companies) {
            this.SafeBatch(companies, (batchStatement, c) => batchStatement.Add(this.upsertStatement.Bind((int)c.Index, c.Name, c.ExcludeYn, c.Symbol)));
        }

        [Obsolete]
        public void UpdateAll(IEnumerable<Company> companies) {
            this.InsertAll(companies);
        }

        public void Update(Company company) {
            Connection.Session.Execute(this.upsertStatement.Bind((int)company.Index, company.Name, company.ExcludeYn, company.Symbol));
        }

        public Task<IEnumerable<Company>> FindAll(string wordMatch) {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Company>> FindByIndex(Company.ConstituentOfIndex index, bool omitExcluded) {
            // Need to replace this
            return this.FindAll().ContinueWith(t => t.Result.ToList().Where(c => c.Index == index));
        }

        public Task<IEnumerable<Company>> FindAll() {
            var statement = new SimpleStatement("SELECT * FROM companies");
            var resultTask = Connection.Session.ExecuteAsync(statement).ContinueWith((r) => r.Result.GetRows().ToList().Select(Company.CreateFromRow));
            return resultTask;
        }

        public Task<Company> FindBySymbol(string symbol) {
            var statement = this.Connection.Session.Prepare(@"SELECT * FROM companies WHERE ""Symbol"" = ?").Bind(symbol);
            return Connection.Session.ExecuteAsync(statement).ContinueWith(r => this.Transform(r));
        }

        public Company Transform(Task<RowSet> r) {
            var result = r.Result.GetRows().ToList();
            return result.Count() == 1 ? Company.CreateFromRow(result.First()) : null;
        }

        [Obsolete]
        public Task<Company> FindById(string id) {
            return this.FindBySymbol(id);
        }
    }
}
