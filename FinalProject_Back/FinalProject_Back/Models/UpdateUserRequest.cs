﻿namespace FinalProject_Back.Models
{

    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
  
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Zipcode { get; set; }
        public string Avatar { get; set; }
        public string Gender { get; set; }
    }
}
