

namespace GraphAnalyser.Tests.Domain.Repository.Cassandra
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository.Cassandra;

    public class CompanyRepositoryTests
    {
        private readonly CompanyRepository companyRepository = new CompanyRepository();

        [SetUp]
        public void Setup() {
            ConnectionManager.Instance.Session.Execute("TRUNCATE companies");
        }

        [Test]
        public void InsertTest() {
            var company = new Company()
                {
                    ExcludeYn = 1,
                    Index = Company.ConstituentOfIndex.Ftse100,
                    Symbol = "TST.L",
                    Name = "Tester"
                };

            companyRepository.InsertAll(new List<Company>() {company});
            var fetchedCompany = companyRepository.FindBySymbol("TST.L").Result;
            Assert.AreEqual(company.Index, fetchedCompany.Index);
            Assert.AreEqual(company.ExcludeYn, fetchedCompany.ExcludeYn);
            Assert.AreEqual(company.Name, fetchedCompany.Name);
            Assert.AreEqual(company.Symbol, fetchedCompany.Symbol);
        }

        [Test]
        public void UpdateTest()
        {
            var company = new Company()
            {
                ExcludeYn = 1,
                Index = Company.ConstituentOfIndex.Ftse100,
                Symbol = "TST.L",
                Name = "Tester"
            };

            companyRepository.InsertAll(new List<Company>() { company });
            company.Name = "New Name";
            companyRepository.Update(company);
            var fetchedCompany = companyRepository.FindBySymbol("TST.L").Result;
            Assert.AreEqual(company.Index, fetchedCompany.Index);
            Assert.AreEqual(company.ExcludeYn, fetchedCompany.ExcludeYn);
            Assert.AreEqual(company.Name, fetchedCompany.Name);
            Assert.AreEqual(company.Symbol, fetchedCompany.Symbol);
        }
    }
}
