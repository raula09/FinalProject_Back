using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace FinalProject_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public QRController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateQRCode([FromQuery] string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return BadRequest("Text cannot be null or empty.");
            }

            string apiUrl = $"https://api.everrest.educata.dev/qrcode?text={HttpUtility.UrlEncode(text)}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); 

                var qrCodeBase64 = await response.Content.ReadAsStringAsync();

             
                return Ok(new { qrCode = qrCodeBase64 });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", error = ex.Message });
            }
        }
    }
}
