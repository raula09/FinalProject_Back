
using System.ComponentModel;

namespace FinalProject_Back.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public Category category { get; set; }
         
    }
}
