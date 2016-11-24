using System;
using System.Collections.Generic;

namespace MyShop.Web.Models
{
    public class ProductViewModel
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public string Alias { set; get; }

        public int CategoryID { set; get; }

        public int ProviderID { set; get; }

        public int? Quantity { set; get; }

        public int? QuantitySold { set; get; }

        public string Image { set; get; }

        public string Image2 { set; get; }

        public string MoreImages { set; get; }

        public decimal Price { set; get; }

        public decimal? PromotionPrice { set; get; }

        public int? Warranty { set; get; }

        public string Description { set; get; }

        public string Content { set; get; }

        public bool? HomeFlag { set; get; }

        public bool? HotFlag { set; get; }

        public int? ViewCount { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        public bool Status { set; get; }

        public string Tags { set; get; }

        public decimal OriginalPrice { set; get; }

        public virtual ProductCategoryViewModel ProductCategory { set; get; }

        public IEnumerable<MaterialViewModel> Materials { set; get; }

        public IEnumerable<ColorViewModel> Colors { set; get; }

        public IEnumerable<SizeViewModel> Sizes { set; get; }

        public IEnumerable<HeightViewModel> Heights { set; get; }

        public IEnumerable<TypeViewModel> Types { set; get; }

        public IEnumerable<HeelViewModel> Heels { set; get; }
    }
}