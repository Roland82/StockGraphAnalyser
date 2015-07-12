

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

        [Test]
        public void UpdateCompanyMetaDataTest() {
            var ftse100 = new Dictionary<string, string> { { "TST", "Test Company"}, { "HI", "Another company"}};
            var ftse250 = new Dictionary<string, string> { { "HOD", "HOD Company" }, { "HEL", "Hello company" } };
            var smallCap = new Dictionary<string, string> { { "SMA", "Small Company" }, { "ASC", "Another small company" } };
            this.companyFinderService.Setup(m => m.GetFtseIndex(Company.ConstituentOfIndex.Ftse100)).Returns(ftse100);
            this.companyFinderService.Setup(m => m.GetFtseIndex(Company.ConstituentOfIndex.Ftse250)).Returns(ftse250);
            this.companyFinderService.Setup(m => m.GetFtseIndex(Company.ConstituentOfIndex.FtseSmallCap)).Returns(smallCap);

            var companiesInDb = new[]
                {
                    Company.Create("", "TST.L", Company.ConstituentOfIndex.Unknown),
                    Company.Create("", "HI.L", Company.ConstituentOfIndex.Unknown),
                    Company.Create("", "HOD.L", Company.ConstituentOfIndex.Unknown),
                    Company.Create("", "HEL.L", Company.ConstituentOfIndex.Unknown),
                    Company.Create("", "SMA.L", Company.ConstituentOfIndex.Unknown),
                    Company.Create("", "NOT.L", Company.ConstituentOfIndex.Unknown),
                };
            this.companyRepository.Setup(m => m.FindAll()).ReturnsAsync(companiesInDb);
            var service = new CompanyDataManagementService(this.companyFinderService.Object, this.companyRepository.Object);
            service.UpdateCompanyMetaData();
            this.companyRepository.Verify(m => m.UpdateAll(It.Is<IEnumerable<Company>>(c => c.Count() == 2)), Times.Exactly(2));
            this.companyRepository.Verify(m => m.UpdateAll(It.Is<IEnumerable<Company>>(c => c.Count() == 1)), Times.Exactly(1));
            this.companyRepository.Verify(m => m.UpdateAll(It.Is<IEnumerable<Company>>(c => c.Any(e => e.Symbol == "TST.L" && e.Index == Company.ConstituentOfIndex.Ftse100))));
            this.companyRepository.Verify(m => m.UpdateAll(It.Is<IEnumerable<Company>>(c => c.Any(e => e.Symbol == "HI.L" && e.Index == Company.ConstituentOfIndex.Ftse100))));
            this.companyRepository.Verify(m => m.UpdateAll(It.Is<IEnumerable<Company>>(c => c.Any(e => e.Symbol == "HOD.L" && e.Index == Company.ConstituentOfIndex.Ftse250))));
            this.companyRepository.Verify(m => m.UpdateAll(It.Is<IEnumerable<Company>>(c => c.Any(e => e.Symbol == "HEL.L" && e.Index == Company.ConstituentOfIndex.Ftse250))));
            this.companyRepository.Verify(m => m.UpdateAll(It.Is<IEnumerable<Company>>(c => c.Any(e => e.Symbol == "SMA.L" && e.Index == Company.ConstituentOfIndex.FtseSmallCap))));
            this.companyRepository.Verify(m => m.UpdateAll(It.Is<IEnumerable<Company>>(c => c.Any(e => e.Symbol == "NOT.L"))), Times.Never);
        }

        private void SetupMocks(Dictionary<string, string> companyFinderReturn,
                                IEnumerable<Company> companyRepositoryFindAllReturn) {
            companyFinderService.Setup(m => m.GetAllSymbols()).Returns(companyFinderReturn);
            companyRepository.Setup(m => m.FindAll()).ReturnsAsync(companyRepositoryFindAllReturn);
        }
    }
}
