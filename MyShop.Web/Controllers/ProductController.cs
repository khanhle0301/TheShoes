using AutoMapper;
using MyShop.Common;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Infrastructure.Core;
using MyShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyShop.Web.Controllers
{
    public class ProductController : Controller
    {
        private ICommonService _commonService;
        private IProductService _productService;
        private IProviderService _providerService;
        private IProductCategoryService _productCategoryService;
        private IColorService _colorService;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService,
                                    IProviderService providerService, IColorService colorService, ICommonService commonService)
        {
            this._commonService = commonService;
            this._providerService = providerService;
            this._colorService = colorService;
            this._productService = productService;
            this._productCategoryService = productCategoryService;
        }

        public ActionResult Detail(int id)
        {
            var productModel = _productService.GetAllById(id);
            var viewModel = Mapper.Map<Product, ProductViewModel>(productModel);
            List<string> listImages = new JavaScriptSerializer().Deserialize<List<string>>(viewModel.MoreImages);
            ViewBag.MoreImages = listImages;
            ViewBag.Tags = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_productService.GetListTagByProductId(id));
            ViewBag.Sizes = Mapper.Map<IEnumerable<Size>, IEnumerable<SizeViewModel>>(_productService.GetListSizeByProductId(id));
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_productService.GetListColorByProductId(id));
            ViewBag.Related = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productService.GetReatedProducts(id, 12));

            _productService.IncreaseView(id);
            _productService.Save();

            var recentProduct = (List<RecentProductItem>)Session[Common.CommonConstants.RecentProductSession];
            if (recentProduct == null)
            {
                recentProduct = new List<RecentProductItem>();
            }
            if (recentProduct.Any(x => x.ProductId == id))
            {
                foreach (var item in recentProduct)
                {
                    if (item.ProductId == id)
                    {
                        item.CreateDate = DateTime.Now;
                    }
                }
            }
            else
            {
                RecentProductItem newItem = new RecentProductItem();
                newItem.ProductId = id;
                newItem.Product = productModel;
                newItem.CreateDate = DateTime.Now;
                recentProduct.Add(newItem);
            }
            Session[Common.CommonConstants.RecentProductSession] = recentProduct;

            return View(viewModel);
        }

        public ActionResult Category(string alias)
        {
            var category = _productCategoryService.GetByAlias(alias);
            ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
            ViewBag.Providers = Mapper.Map<IEnumerable<Provider>, IEnumerable<ProviderViewModel>>(_providerService.GetAll());
            ViewBag.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(_colorService.GetAll());
            ViewBag.Materials = Mapper.Map<IEnumerable<Material>, IEnumerable<MaterialViewModel>>(_commonService.GetMaterial());
            ViewBag.CategoryID = category.ID;
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(int id, int page, int pageSize, string sort = "", string price = "", string provider = "", string color = "", string chatlieu = "")
        {
            var model = _productService.Demo(id, sort, price, provider, color, chatlieu);
            int totalRow = model.Count();
            model = model.Skip((page - 1) * pageSize).Take(pageSize);

            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ViewAllProduct()
        {
            return View();
        }

        public ActionResult ViewNewProduct()
        {
            return View();
        }

        public ActionResult ViewOnSaleProduct()
        {
            return View();
        }

        public ActionResult ViewHotProduct()
        {
            return View();
        }

        public ActionResult ViewSaleHotProduct()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RecentProduct()
        {
            var recentProduct = Session[Common.CommonConstants.RecentProductSession];
            var list = new List<RecentProductItem>();
            if (recentProduct != null)
            {
                list = (List<RecentProductItem>)recentProduct;
            }
            return PartialView(list);
        }

        [ChildActionOnly]
        public ActionResult ProductCategory()
        {
            var model = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(_productCategoryService.GetAll());
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult PopularProduct()
        {
            var model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productService.GetViewCount(3));
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult TopOnSale()
        {
            var model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productService.GetHomeSale(3));
            return PartialView(model);
        }

        public JsonResult GetAll(int id)
        {
            var model = _productService.GetAllById(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSize(int id)
        {
            var model = _productService.GetListSizeByProductId(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetColor(int id)
        {
            var model = _productService.GetListColorByProductId(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}