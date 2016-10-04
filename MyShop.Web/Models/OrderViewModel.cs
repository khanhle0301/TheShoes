using System;
using System.Collections.Generic;

namespace MyShop.Web.Models
{
    public class OrderViewModel
    {
        public int ID { set; get; }

        public string CustomerName { set; get; }

        public string CustomerAddress { set; get; }

        public string CustomerEmail { set; get; }

        public string CustomerMobile { set; get; }

        public string CustomerMessage { set; get; }

        public string PaymentMethod { set; get; }

        public DateTime? CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public string PaymentStatus { set; get; }
        public bool Status { set; get; }

        public virtual IEnumerable<OrderDetailViewModel> OrderDetails { set; get; }
    }
}