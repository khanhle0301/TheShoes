using System.Web.Mvc;
using System.Web.Routing;

namespace MyShop.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "CancelOrder",
                 url: "loi-thanh-toan.html",
                 defaults: new { controller = "ShoppingCart", action = "CancelOrder", id = UrlParameter.Optional },
                 namespaces: new string[] { "MyShop.Web.Controllers" }
             );

            routes.MapRoute(
                 name: "Login",
                 url: "dang-nhap.html",
                 defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "MyShop.Web.Controllers" }
             );

            routes.MapRoute(
               name: "Contact",
               url: "trang/lien-he.html",
               defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "MyShop.Web.Controllers" }
           );

            routes.MapRoute(
                 name: "Search",
                 url: "tim-kiem.html",
                 defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                 namespaces: new string[] { "MyShop.Web.Controllers" }
             );

            routes.MapRoute(
     name: "Checkout",
     url: "thanh-toan.html",
     defaults: new { controller = "ShoppingCart", action = "Checkout", id = UrlParameter.Optional },
     namespaces: new string[] { "MyShop.Web.Controllers" }
 );

            routes.MapRoute(
          name: "Cart",
          url: "gio-hang.html",
          defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
          namespaces: new string[] { "MyShop.Web.Controllers" }
      );

            routes.MapRoute(
          name: "ViewViewCountProduct",
          url: "san-pham/viewcount.html",
          defaults: new { controller = "Product", action = "ViewViewCountProduct", id = UrlParameter.Optional },
          namespaces: new string[] { "MyShop.Web.Controllers" }
      );

            routes.MapRoute(
            name: "ViewAllProduct",
            url: "san-pham/all.html",
            defaults: new { controller = "Product", action = "ViewAllProduct", id = UrlParameter.Optional },
            namespaces: new string[] { "MyShop.Web.Controllers" }
        );

            routes.MapRoute(
                name: "ViewNewProduct",
                url: "san-pham/news.html",
                defaults: new { controller = "Product", action = "ViewNewProduct", id = UrlParameter.Optional },
                namespaces: new string[] { "MyShop.Web.Controllers" }
            );

            routes.MapRoute(
              name: "ViewOnSaleProduct",
              url: "san-pham/onsale.html",
              defaults: new { controller = "Product", action = "ViewOnSaleProduct", id = UrlParameter.Optional },
              namespaces: new string[] { "MyShop.Web.Controllers" }
          );

            routes.MapRoute(
            name: "ViewHotProduct",
            url: "san-pham/noi-bat.html",
            defaults: new { controller = "Product", action = "ViewHotProduct", id = UrlParameter.Optional },
            namespaces: new string[] { "MyShop.Web.Controllers" }
        );

            routes.MapRoute(
          name: "ViewSaleHotProduct",
          url: "san-pham/ban-chay.html",
          defaults: new { controller = "Product", action = "ViewSaleHotProduct", id = UrlParameter.Optional },
          namespaces: new string[] { "MyShop.Web.Controllers" }
      );

            routes.MapRoute(
                name: "Post",
                url: "blogs/news.html",
                defaults: new { controller = "Post", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "MyShop.Web.Controllers" }
            );

            routes.MapRoute(
             name: "Page",
             url: "trang/{alias}.html",
             defaults: new { controller = "Page", action = "Index", alias = UrlParameter.Optional },
             namespaces: new string[] { "MyShop.Web.Controllers" }
         );

            routes.MapRoute(
                name: "Post Category",
                url: "blogs/{alias}.html",
                defaults: new { controller = "Post", action = "Category", alias = UrlParameter.Optional },
                  namespaces: new string[] { "MyShop.Web.Controllers" }
            );

            routes.MapRoute(
              name: "Post Tag",
              url: "blogs/tag/{tagId}.html",
              defaults: new { controller = "Post", action = "ListByTag", tagId = UrlParameter.Optional },
              namespaces: new string[] { "MyShop.Web.Controllers" }
          );

            routes.MapRoute(
              name: "Post Detail",
              url: "blogs/{catealias}/{alias}-{id}.html",
              defaults: new { controller = "Post", action = "Detail", id = UrlParameter.Optional },
                namespaces: new string[] { "MyShop.Web.Controllers" }
          );

            routes.MapRoute(
          name: "Product Tag",
          url: "san-pham/tag/{tagId}.html",
          defaults: new { controller = "Product", action = "ListByTag", tagId = UrlParameter.Optional },
          namespaces: new string[] { "MyShop.Web.Controllers" }
      );

            routes.MapRoute(
             name: "Product Category",
             url: "san-pham/{alias}.html",
             defaults: new { controller = "Product", action = "Category", alias = UrlParameter.Optional },
               namespaces: new string[] { "MyShop.Web.Controllers" }
         );

            routes.MapRoute(
             name: "Product Detail",
             url: "san-pham/{cateAlias}/{alias}-{id}.html",
             defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
               namespaces: new string[] { "MyShop.Web.Controllers" }
         );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                  namespaces: new string[] { "MyShop.Web.Controllers" }
            );
        }
    }
}