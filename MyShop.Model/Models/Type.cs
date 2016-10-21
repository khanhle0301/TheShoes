using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Model.Models
{
    [Table("Types")]
    public class Type
    {
        [Key]
        public int ID { set; get; }

        [MaxLength(50)]
        [Required]
        public string Name { set; get; }

        public virtual IEnumerable<ProductType> ProductTypes { set; get; }
    }
}
