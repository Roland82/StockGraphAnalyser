
namespace GraphAnalyser.Tests.Web
{
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.StockDataProviders;

    public class SymbolFinderServiceTests
    {
        CompanyFinderService service = new CompanyFinderService();

        [Test]
        public void GetAllSymbols()
        {
            var companies = service.GetAllSymbols();
            Assert.True(companies.Count > 1000);
        }

        [TestCase(Company.ConstituentOfIndex.Ftse100)]
        [TestCase(Company.ConstituentOfIndex.Ftse250)]
        public void GetFtse100Test(Company.ConstituentOfIndex index)
        {
            var companies = service.GetFtseIndex(index);
            Assert.True(companies.Count > 50);
        }
    }
}
