namespace StockGraphAnalyser.Domain.Repository.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Cassandra;

    public interface ICompanyRepository
    {
        void InsertAll(IEnumerable<Company> companies);
        void UpdateAll(IEnumerable<Company> companies);
        void Update(Company company);
        Task<IEnumerable<Company>> FindAll(string wordMatch);
        Task<IEnumerable<Company>> FindByIndex(Company.ConstituentOfIndex index, bool omitExcluded);
        Task<IEnumerable<Company>> FindAll();
        Task<Company> FindBySymbol(string symbol);
        Task<Company> FindById(string id);
    }
}