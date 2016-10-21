using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Model.Models
{
    [Table("Heights")]
    public class Height
    {
        [Key]
        public int ID { set; get; }

        [MaxLength(50)]
        [Required]
        public string Name { set; get; }

        public virtual IEnumerable<ProductHeight> ProductHeights { set; get; }
    }
}
