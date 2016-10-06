using System.Web.Mvc;

namespace MyShop.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult HttpError403()
        {
            return this.View();
        }

        public ActionResult HttpError404()
        {
            return this.View();
        }

        public ActionResult HttpError500()
        {
            return this.View();
        }
    }
}