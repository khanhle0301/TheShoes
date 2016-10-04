namespace MyShop.Web.Models
{
    public class BannerViewModel
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public string Content { set; get; }

        public string Image { set; get; }

        public string Url { set; get; }

        public int? DisplayOrder { set; get; }

        public bool Status { set; get; }
    }
}