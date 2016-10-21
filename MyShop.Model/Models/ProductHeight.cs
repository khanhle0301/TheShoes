using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Model.Models
{
    [Table("ProductHeights")]
    public class ProductHeight
    {
        [Key]
        [Column(Order = 1)]
        public int ProductId { set; get; }

        [Key]
        [Column(Order = 2)]
        public int HeightId { set; get; }

        [ForeignKey("ProductId")]
        public virtual Product Product { set; get; }

        [ForeignKey("HeightId")]
        public virtual Height Height { set; get; }
    }
}