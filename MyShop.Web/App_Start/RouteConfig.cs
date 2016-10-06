using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                 name: "Login",
                 url: "dang-nhap.html",
                 defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                 namespaces: new string[] { "MyShop.Web.Controllers" }
             );

            routes.MapRoute(
               name: "Contact",
               url: "trang/lien-he.html",
               defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "MyShop.Web.Controllers" }
           );

            routes.MapRoute(
          name: "Cart",
          url: "gio-hang.html",
          defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
          namespaces: new string[] { "MyShop.Web.Controllers" }
      );

            routes.MapRoute(
            name: "ViewAllProduct",
            url: "san-pham/tat-ca.html",
            defaults: new { controller = "Product", action = "ViewAllProduct", id = UrlParameter.Optional },
            namespaces: new string[] { "MyShop.Web.Controllers" }
        );

            routes.MapRoute(
                name: "ViewNewProduct",
                url: "san-pham/hang-moi-ve.html",
                defaults: new { controller = "Product", action = "ViewNewProduct", id = UrlParameter.Optional },
                namespaces: new string[] { "MyShop.Web.Controllers" }
            );

            routes.MapRoute(
              name: "ViewOnSaleProduct",
              url: "san-pham/san-pham-khuyen-mai.html",
              defaults: new { controller = "Product", action = "ViewOnSaleProduct", id = UrlParameter.Optional },
              namespaces: new string[] { "MyShop.Web.Controllers" }
          );

            routes.MapRoute(
            name: "ViewHotProduct",
            url: "san-pham/san-pham-noi-bat.html",
            defaults: new { controller = "Product", action = "ViewHotProduct", id = UrlParameter.Optional },
            namespaces: new string[] { "MyShop.Web.Controllers" }
        );

            routes.MapRoute(
          name: "ViewSaleHotProduct",
          url: "san-pham/san-pham-ban-chay.html",
          defaults: new { controller = "Product", action = "ViewSaleHotProduct", id = UrlParameter.Optional },
          namespaces: new string[] { "MyShop.Web.Controllers" }
      );          

            routes.MapRoute(
                name: "Post",
                url: "bai-viet.html",
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
                url: "bai-viet/{alias}.html",
                defaults: new { controller = "Post", action = "Category", alias = UrlParameter.Optional },
                  namespaces: new string[] { "MyShop.Web.Controllers" }
            );

            routes.MapRoute(
              name: "Post Tag",
              url: "bai-viet/tag/{tagId}.html",
              defaults: new { controller = "Post", action = "ListByTag", tagId = UrlParameter.Optional },
              namespaces: new string[] { "MyShop.Web.Controllers" }
          );

            routes.MapRoute(
              name: "Post Detail",
              url: "bai-viet/{catealias}/{alias}-{id}.html",
              defaults: new { controller = "Post", action = "Detail", id = UrlParameter.Optional },
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
