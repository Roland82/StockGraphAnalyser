
namespace GraphAnalyser.Tests.Web
{
    using NUnit.Framework;
    using StockGraphAnalyser.Domain.Web;

    public class SymbolFinderServiceTests
    {
        SymbolFinderService service = new SymbolFinderService();

        [Test]
        public void GetAllSymbols()
        {
            var companies = service.GetAllSymbols();
            Assert.True(companies.Count > 1000);
        }

        [Test]
        public void GetFtse100Test()
        {
            var companies = service.GetFtse100();
            Assert.True(companies.Count > 90);
        }
    }
}
