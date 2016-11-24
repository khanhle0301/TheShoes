using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using MyShop.Model.Abstract;

namespace MyShop.Model.Models
{
    [Table("Products")]
    public class Product : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Required]
        [MaxLength(256)]
        public string Alias { set; get; }

        [Required]
        public int CategoryID { set; get; }

        public int? ProviderID { set; get; }

        public int? Quantity { set; get; }

        public int? QuantitySold { set; get; }

        [MaxLength(256)]
        public string Image { set; get; }

        [MaxLength(256)]
        public string Image2 { set; get; }

        [Column(TypeName = "xml")]
        public string MoreImages { set; get; }

        public decimal Price { set; get; }

        public decimal? PromotionPrice { set; get; }
        public int? Warranty { set; get; }

        [MaxLength(500)]
        public string Description { set; get; }
        public string Content { set; get; }

        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { set; get; }

        public string Tags { set; get; }

        public decimal OriginalPrice { set; get; }

        [ForeignKey("CategoryID")]
        public virtual ProductCategory ProductCategory { set; get; }

        [ForeignKey("ProviderID")]
        public virtual Provider Provider { set; get; }
    }
}