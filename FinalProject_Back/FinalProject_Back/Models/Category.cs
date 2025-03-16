namespace FinalProject_Back.Models
{
    public class Category
    {
         public int Id { get; set; }
         public int Name { get; set; }
         public string Image { get; set; }
        public ICollection<Product> products { get; set; }
    }
}
