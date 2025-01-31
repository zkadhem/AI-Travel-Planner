using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/hotels")]
public class HotelController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public HotelController(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetHotels([FromQuery] string location, [FromQuery] string checkIn, [FromQuery] string checkOut)
    {
        var apiKey = _configuration["AmadeusAPI:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
            return BadRequest("API Key is missing.");

        // Step 1: Get the Correct City Code
        string citySearchUrl = $"https://test.api.amadeus.com/v1/reference-data/locations?keyword={location}&subType=CITY";
        var cityResponse = await _httpClient.GetAsync(citySearchUrl);
        if (!cityResponse.IsSuccessStatusCode)
            return BadRequest("Failed to fetch city information.");

        var cityJson = await cityResponse.Content.ReadAsStringAsync();
        var cityData = JsonSerializer.Deserialize<AmadeusCityResponse>(cityJson);
        if (cityData?.Data == null || cityData.Data.Length == 0)
            return BadRequest("Invalid city name.");

        string cityCode = cityData.Data[0].IataCode; // Extract the city code

        // Step 2: Fetch Hotel Data with City Code
        string hotelUrl = $"https://test.api.amadeus.com/v2/shopping/hotel-offers?cityCode={cityCode}&checkInDate={checkIn}&checkOutDate={checkOut}";
        var hotelResponse = await _httpClient.GetAsync(hotelUrl);
        if (!hotelResponse.IsSuccessStatusCode)
            return BadRequest("Failed to fetch hotel data.");

        var hotelJson = await hotelResponse.Content.ReadAsStringAsync();
        return Content(hotelJson, "application/json");
    }
}

// Helper class to parse JSON response
public class AmadeusCityResponse
{
    public CityData[] Data { get; set; }
}

public class CityData
{
    public string IataCode { get; set; }
}
