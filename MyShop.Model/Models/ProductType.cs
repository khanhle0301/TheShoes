using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Model.Models
{
    [Table("ProductTypes")]
    public class ProductType
    {
        [Key]
        [Column(Order = 1)]
        public int ProductId { set; get; }

        [Key]
        [Column(Order = 2)]
        public int TypeId { set; get; }

        [ForeignKey("ProductId")]
        public virtual Product Product { set; get; }

        [ForeignKey("TypeId")]
        public virtual Type Type { set; get; }
    }
}
