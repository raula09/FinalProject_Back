namespace FinalProject_Back.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public List<string> Images { get; set; }
        public int PCategory { get; set; }
        public int Warranty { get; set; }
        public string IssueDate { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
    }

}
