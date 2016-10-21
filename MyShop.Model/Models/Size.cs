using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Model.Models
{
    [Table("Sizes")]
    public class Size
    {
        [Key]        
        public int ID { set; get; }

        [MaxLength(50)]
        [Required]
        public string Name { set; get; }

        public virtual IEnumerable<ProductSize> ProductSizes { set; get; }
    }
}