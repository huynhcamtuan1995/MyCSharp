using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Category Category { get; set; }
    }
}
