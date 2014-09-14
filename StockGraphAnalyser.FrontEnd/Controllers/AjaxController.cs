

namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System.Web.Mvc;
    using Domain.Service.Interfaces;
    using System.Linq;

    public class AjaxController : Controller
    {
        private readonly ICompanyDataManagementService companyDataManagementService;

        public AjaxController(ICompanyDataManagementService companyDataManagementService) {
            this.companyDataManagementService = companyDataManagementService;
        }

        [HttpGet]
        public ActionResult GetMatchingCompanies(string term) {
            var matching = this.companyDataManagementService.FindAllMatching(term).Select(d => new { label = d.Symbol + ":" + d.Name, value =d.Symbol });
            return Json(matching, JsonRequestBehavior.AllowGet);
        }
    }
}
