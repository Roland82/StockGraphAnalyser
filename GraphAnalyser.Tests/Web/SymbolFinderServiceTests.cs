using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAnalyser.Tests.Web
{
    using NUnit.Framework;
    using StockGraphAnalyser.Domain.Web;

    public class SymbolFinderServiceTests
    {
        [Test]
        public void Test()
        {
            var service = new SymbolFinderService();
            service.GetAllSymbols();
        }
    }
}
