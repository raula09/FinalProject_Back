namespace FinalProject_Back.Models
{

    public class UserResponse
    {
        public string _id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Zipcode { get; set; }
        public string Avatar { get; set; }
        public string Gender { get; set; }
        public string CartID { get; set; }
        public bool Verified { get; set; }
        public List<string> ChatIds { get; set; }
        public int __v { get; set; }
    }
}
