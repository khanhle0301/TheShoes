using AutoMapper;
using MyShop.Common;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IProductService _productService;
        private IOrderService _orderService;

        public ShoppingCartController(IOrderService orderService, IProductService productService)
        {
            this._productService = productService;
            this._orderService = orderService;
        }

        public ActionResult Index()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return View();
        }

        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new
            {
                data = cart,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(int productId, int quantity, string size, string color)
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            var product = _productService.GetAllById(productId);
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            if (quantity > product.Quantity)
            {
                return Json(new
                {
                    status = false,
                    message = "Số lượng không đủ"
                });
            }

            if (cart.Any(x => x.ProductId == productId && x.Color == color && x.Size == size))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productId && item.Color == color && item.Size == size)
                    {
                        item.Quantity += quantity;
                    }
                }
            }
            else if (cart.Any(x => x.ProductId == productId && x.Color == color))
            {
                bool add = false;
                foreach (var item in cart)
                {
                    if (item.ProductId == productId && item.Color == color && item.Size == size)
                    {
                        item.Quantity += quantity;
                    }
                    else
                    {
                        add = true;
                    }
                }
                if (add)
                {
                    ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                    newItem.ProductId = productId;
                    newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                    newItem.Quantity = quantity;
                    newItem.Size = size;
                    newItem.Color = color;
                    cart.Add(newItem);
                }
            }
            else if (cart.Any(x => x.ProductId == productId && x.Size == size))
            {
                bool add = false;
                foreach (var item in cart)
                {
                    if (item.ProductId == productId && item.Size == size && item.Color == color)
                    {
                        item.Quantity += quantity;
                    }
                    else
                    {
                        add = true;
                    }
                }
                if (add)
                {
                    ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                    newItem.ProductId = productId;
                    newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                    newItem.Quantity = quantity;
                    newItem.Size = size;
                    newItem.Color = color;
                    cart.Add(newItem);
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = quantity;
                newItem.Size = size;
                newItem.Color = color;
                cart.Add(newItem);
            }

            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId, string size, string color)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId && x.Size == size && x.Color == color);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];

            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.ProductId == jitem.ProductId && item.Size == jitem.Size && item.Color == jitem.Color)
                    {
                        item.Quantity = jitem.Quantity;
                        item.Size = jitem.Size;
                    }
                }
            }

            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true
            });
        }
    }
}