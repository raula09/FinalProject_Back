namespace FinalProject_Back.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
         public string HashedPassword { get; set; } = string.Empty;
         public string Token { get; set; } = string.Empty;
         public DateTime? TokenExpireDate { get; set; }
         public string? Email { get; set; }
         public string Role { get; set; } = "User";

          
         

    }
}
