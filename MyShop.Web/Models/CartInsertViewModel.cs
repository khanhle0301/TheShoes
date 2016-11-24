namespace MyShop.Web.Models
{
    public class CartInsertViewModel
    {
        public int ProductId { set; get; }
        public ProductViewModel Product { set; get; }
        public int Quantity { set; get; }
        public string Color { set; get; }
        public string Size { set; get; }
        public string Note { set; get; }
    }
}