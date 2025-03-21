using System.Text.Json.Serialization;

namespace FinalProject_Back.Models
{
    public class TokenResponse
    {
      
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }

            [JsonPropertyName("refresh_token")]
            public string RefreshToken { get; set; }
        

    }
}
