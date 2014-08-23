

namespace GraphAnalyser.Tests.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository.Interfaces;
    using StockGraphAnalyser.Domain.Service;
    using StockGraphAnalyser.Domain.StockDataProviders.Interfaces;

    public class CompanyDataManagementServiceTests
    {
        private Mock<ICompanyFinderService> companyFinderService;
        private Mock<ICompanyRepository> companyRepository;

        private Dictionary<String, String> CompanyFinderResult {
            get {
                return new Dictionary<string, string>
                    {
                        {"TST1.L", "Test 1"},
                        {"TST2.L", "Test 2"},
                        {"TST3.L", "Test 3"}
                    };
            }
        }

        [SetUp]
        public void Setup() {
            companyFinderService = new Mock<ICompanyFinderService>();
            companyRepository = new Mock<ICompanyRepository>();
        }

        [Test]
        public void CompanysAllExistInDatabaseAlready() {
            var companyRepositoryResult = CompanyFinderResult.Select(d => Company.Create(d.Value, d.Key, Company.ConstituentOfIndex.Unknown));
            this.SetupMocks(CompanyFinderResult, companyRepositoryResult);
            var service = new CompanyDataManagementService(companyFinderService.Object, companyRepository.Object);
            service.GetNewCompanies();

            companyRepository.Verify(m => m.InsertAll(It.IsAny<IEnumerable<Company>>()), Times.Never);
        }

        [Test]
        public void CompanyDoesntExistInDatabase() {
            var companyRepositoryResult = CompanyFinderResult.Select(d => Company.Create(d.Value, d.Key, Company.ConstituentOfIndex.Unknown));
            this.SetupMocks(CompanyFinderResult, companyRepositoryResult.Skip(1).Take(2));
            var service = new CompanyDataManagementService(companyFinderService.Object, companyRepository.Object);
            service.GetNewCompanies();

            companyRepository.Verify(m => m.InsertAll(It.Is<IEnumerable<Company>>(c => c.Count() == 1)), Times.Once);
            companyRepository.Verify(m => m.InsertAll(It.Is<IEnumerable<Company>>(c => c.Count(r => r.Name == "Test 1") == 1)), Times.Once);
        }

        private void SetupMocks(Dictionary<string, string> companyFinderReturn,
                                IEnumerable<Company> companyRepositoryFindAllReturn) {
            companyFinderService.Setup(m => m.GetAllSymbols()).Returns(companyFinderReturn);
            companyRepository.Setup(m => m.FindAll()).Returns(companyRepositoryFindAllReturn);
        }
    }
}
