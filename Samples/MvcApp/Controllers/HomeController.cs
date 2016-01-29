using System.Collections.Generic;
using System.Web.Mvc;
using Sample.Business;
using Sample.Business.Models;

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

            return View("Index", _agent.GetShoppingCart(new GetShoppingCartFormModel { UserId = 10 }));
        }
    }
}
