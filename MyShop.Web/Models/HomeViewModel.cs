using System.Collections.Generic;

namespace MyShop.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<SlideViewModel> Slides { set; get; }
        public IEnumerable<BannerViewModel> Banners { set; get; }
        public IEnumerable<ProductViewModel> LastestProducts { set; get; }
        public IEnumerable<ProductViewModel> SaleProducts { set; get; }
        public IEnumerable<ProductViewModel> HotProducts { set; get; }
        public IEnumerable<ProductViewModel> HotSaleProducts { set; get; }
        public string Title { set; get; }
        public string MetaKeyword { set; get; }
        public string MetaDescription { set; get; }
    }
}