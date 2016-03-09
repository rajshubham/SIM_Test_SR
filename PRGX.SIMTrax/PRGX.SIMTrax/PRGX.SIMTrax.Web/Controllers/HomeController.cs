using PRGX.SIMTrax.ServiceFacade;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using System.Web.Mvc;

namespace PRGX.SIMTrax.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var userName = Domain.Util.ReadResource.GetResource("ADMIN_EMAIL_ID", Domain.Util.ResourceType.Email);
            return View();
        }
    }
}