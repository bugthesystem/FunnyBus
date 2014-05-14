using System.Web.Mvc;
using Sample.BusinessLayer;
using Sample.BusinessLayer.Models;

namespace Sample.MvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomerControllerAgent _agent;

        public HomeController(IHomerControllerAgent agent)
        {
            _agent = agent;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View("Index", _agent.GetOrders(new GetOrdersModel { UserId = 10 }));
        }
    }
}
