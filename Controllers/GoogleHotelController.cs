using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/hotels")]
public class GoogleHotelController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public GoogleHotelController(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetHotels([FromQuery] string location, [FromQuery] string checkIn, [FromQuery] string checkOut)
    {
        var apiKey = _configuration["GoogleAPI:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
            return BadRequest("API Key is missing.");

        string url = $"https://travelpartner.googleapis.com/v1/hotels:search?key={apiKey}&location={location}&checkInDate={checkIn}&checkOutDate={checkOut}&currency=USD&maxResults=15";

        var response = await _httpClient.GetStringAsync(url);
        return Content(response, "application/json");
    }
}
