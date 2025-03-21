using System.Reflection;

namespace FinalProject_Back.Models
{
    public enum Gender
    {
        MALE,
        FEMALE,
        OTHER
    }

    public class SignUpRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Zipcode { get; set; }
        public string Avatar { get; set; }
        public Gender Gender { get; set; }
    }

}
