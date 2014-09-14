namespace StockGraphAnalyser.Domain.Repository.Interfaces
{
    using System.Collections.Generic;

    public interface ICompanyRepository
    {
        void InsertAll(IEnumerable<Company> companies);
        void UpdateAll(IEnumerable<Company> companies);
        IEnumerable<Company> FindAll(string wordMatch);
        IEnumerable<Company> FindByIndex(Company.ConstituentOfIndex index);
        IEnumerable<Company> FindAll();
    }
}