﻿using AutoMapper;
using MyShop.Common;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private IPostService _postService;
        private IPageService _pageService;
        private IProductCategoryService _productCategoryService;
        private IProductService _productService;
        private ICommonService _commonService;

        public HomeController(IProductCategoryService productCategoryService,
                                IPostService postService, IPageService pageService,
                                IProductService productService, ICommonService commonService)
        {
            this._productCategoryService = productCategoryService;
            this._postService = postService;
            this._pageService = pageService;
            this._commonService = commonService;
            this._productService = productService;
        }

        public ActionResult Index()
        {
            var slideModel = _commonService.GetSlides();
            var slideView = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(slideModel);
            var bannerModel = _commonService.GetBanners();
            var bannerView = Mapper.Map<IEnumerable<Banner>, IEnumerable<BannerViewModel>>(bannerModel);
            var homeViewModel = new HomeViewModel();
            homeViewModel.Slides = slideView;
            homeViewModel.Banners = bannerView;

            var lastestProductModel = _productService.GetHomeLastest(4);
            var hotSaleProductModel = _productService.GetHomeHotSaleProduct(8);
            var saleProductModel = _productService.GetHomeSale(4);
            var hotProductModel = _productService.GetHomeHotProduct();

            var lastestProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            var hotSaleProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(hotSaleProductModel);
            var saleProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(saleProductModel);
            var hotProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(hotProductModel);

            homeViewModel.LastestProducts = lastestProductViewModel;
            homeViewModel.HotSaleProducts = hotSaleProductViewModel;
            homeViewModel.SaleProducts = saleProductViewModel;
            homeViewModel.HotProducts = hotProductViewModel;

            ViewBag.Title = ConfigHelper.GetByKey("HomeTitle");
            ViewBag.Keywords = ConfigHelper.GetByKey("HomeKeyword");
            ViewBag.Descriptions = ConfigHelper.GetByKey("HomeDescription");

            return View(homeViewModel);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            var footerViewModel = Mapper.Map<Footer, FooterViewModel>(_commonService.GetFooter());
            return PartialView(footerViewModel);
        }        

        [ChildActionOnly]
        public ActionResult HeaderCart()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return PartialView(cart);
        }

        [ChildActionOnly]
        public ActionResult TopBar()
        {
            var contactViewModel = Mapper.Map<ContactDetail, ContactDetailViewModel>(_commonService.GetContactDetail());
            return PartialView(contactViewModel);
        }

        [ChildActionOnly]
        public ActionResult Page()
        {
            var model = Mapper.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(_pageService.GetListPages());
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult PostNew()
        {
            var model = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(_postService.GetNewPost(3));
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Category()
        {
            var model = _productCategoryService.GetAllHome();
            var listProductCategoryViewModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryViewModel);
        }

        [ChildActionOnly]
        public ActionResult SearchCategory()
        {
            var model = _productCategoryService.GetAllHome();
            var listProductCategoryViewModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryViewModel);
        }

        [ChildActionOnly]
        public ActionResult SearchCategoryMobile()
        {
            var model = _productCategoryService.GetAllHome();
            var listProductCategoryViewModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryViewModel);
        }
    }
}