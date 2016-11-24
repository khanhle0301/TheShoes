using MyShop.Common;
using System.Web;
using System.Web.Optimization;

namespace MyShop.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jquery").Include("~/Assets/client/hstatic.net/0/0/global/design/plugins/jquery/jquery-2.1.4.min.js"));

            bundles.Add(new ScriptBundle("~/js/plugins").Include(
                 "~/Assets/client/hstatic.net/198/1000055198/1000144003/layouta6f6.js",
                 "~/Assets/client/hstatic.net/0/0/global/option_selection.js",
                 "~/Assets/client/hstatic.net/0/0/global/api.jquery.js",
                 "~/Assets/client/hstatic.net/198/1000055198/1000144003/pluginsa6f6.js",
                 "~/Assets/client/hstatic.net/198/1000055198/1000144003/jquery.themepunch.tools.mina6f6.js",
                 "~/Assets/client/hstatic.net/198/1000055198/1000144003/jquery.themepunch.revolution.mina6f6.js",
                 "~/Assets/client/hstatic.net/198/1000055198/1000144003/scripta6f6.js",
                 "~/Assets/client/hstatic.net/198/1000055198/1000144003/jquery.lazyloadxta6f6.js",
                 "~/Assets/client/hstatic.net/198/1000055198/1000144003/fly-to-cart-aothuna6f6.js",
                 "~/Assets/client/hstatic.net/0/0/global/design/plugins/fancybox/jquery.fancybox.js",
                 "~/Assets/client/hstatic.net/198/1000055198/1000144003/pgwslideshow.mina6f6.js",
                 "~/Assets/client/hstatic.net/198/1000055198/1000144003/functionsa6f6.js",
                 "~/Assets/client/hstatic.net/198/1000055198/1000144003/footer_scriptsa6f6.js",
                 "~/Assets/client/js/controller/productSearchController.js",
                 "~/Assets/admin/libs/mustache/mustache.js",
                 "~/Assets/admin/libs/numeral/numeral.js",
                 "~/Assets/client/js/plugins/jquery.twbsPagination.min.js",
                 "~/Assets/admin/libs/jquery-validation/dist/jquery.validate.js",
                 "~/Assets/admin/libs/jquery-validation/dist/additional-methods.js"
                ));

            bundles.Add(new StyleBundle("~/css/base")
                .Include("~/Assets/client/hstatic.net/0/global/design/member/fonts/opensans.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/bootstrapa6f6.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/stylea6f6.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/darka6f6.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/font-iconsa6f6.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/animatea6f6.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/magnific-popupa6f6.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/font_defaulta6f6.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/0/0/global/design/plugins/font-awesome/4.5.0/css/font-awesome.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/responsivea6f6.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/custom_stylea6f6.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/settingsa6f6.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/0/0/global/design/plugins/fancybox/jquery.fancybox.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/simple-sidebara6f6.css", new CssRewriteUrlTransform())
                .Include("~/Assets/client/hstatic.net/198/1000055198/1000144003/pgwslideshow.mina6f6.css", new CssRewriteUrlTransform())
                );
            BundleTable.EnableOptimizations = bool.Parse(ConfigHelper.GetByKey("EnableBundles"));
        }
    }
}
