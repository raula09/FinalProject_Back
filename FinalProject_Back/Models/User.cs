using System.Reflection;

namespace FinalProject_Back.Models
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Zipcode { get; set; }
        public string Avatar { get; set; }
        public Gender Gender { get; set; } = Gender.MALE;
        public string Role { get; set; }
        public bool Verified { get; set; }
    }
    public enum Gender
    {
        MALE,
        FEMALE,
        OTHER
    }
}
