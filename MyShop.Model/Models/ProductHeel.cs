using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyShop.Model.Models
{
    [Table("ProductHeels")]
    public class ProductHeel
    {
        [Key]
        [Column(Order = 1)]
        public int ProductId { set; get; }

        [Key]
        [Column(Order = 2)]
        public int HeelId { set; get; }

        [ForeignKey("ProductId")]
        public virtual Product Product { set; get; }

        [ForeignKey("HeelId")]
        public virtual Heel Heel { set; get; }
    }
}
