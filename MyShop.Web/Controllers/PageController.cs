using AutoMapper;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Models;
using System.Web.Mvc;

namespace MyShop.Web.Controllers
{
    public class PageController : Controller
    {
        private IPageService _pageService;

        public PageController(IPageService pageService)
        {
            this._pageService = pageService;
        }

        public ActionResult Index(string alias)
        {
            var page = _pageService.GetByAlias(alias);
            var model = Mapper.Map<Page, PageViewModel>(page);
            return View(model);
        }      
    }
}