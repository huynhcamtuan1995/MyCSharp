using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataSql.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(255), MinLength(3)]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
