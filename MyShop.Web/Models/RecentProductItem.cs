using MyShop.Model.Models;
using System;

namespace MyShop.Web.Models
{
    [Serializable]
    public class RecentProductItem
    {
        public int ProductId { set; get; }
        public Product Product { set; get; }
        public DateTime CreateDate { set; get; }       
    }
}