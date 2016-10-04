using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Model.Models
{
    [Table("ProductColors")]
    public class ProductColor
    {
        [Key]
        [Column(Order = 1)]
        public int ProductID { set; get; }

        [Key]
        [Column(TypeName = "varchar", Order = 2)]
        [MaxLength(50)]
        public string ColorID { set; get; }     

        [ForeignKey("ProductID")]
        public virtual Product Product { set; get; }

        [ForeignKey("ColorID")]
        public virtual Color Color { set; get; }
    }
}
