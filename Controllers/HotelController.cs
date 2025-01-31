using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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

    private async Task<string> GetAccessTokenAsync()
    {
        var apiKey = _configuration["AmadeusAPI:ApiKey"];
        var apiSecret = _configuration["AmadeusAPI:ApiSecret"];

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            return null;

        var requestBody = new StringContent(
            $"grant_type=client_credentials&client_id={apiKey}&client_secret={apiSecret}",
            Encoding.UTF8, "application/x-www-form-urlencoded"
        );

        var response = await _httpClient.PostAsync("https://test.api.amadeus.com/v1/security/oauth2/token", requestBody);
        string responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"❌ Failed to obtain access token. Status: {response.StatusCode}, Response: {responseString}");
            return null;
        }

        var json = JsonSerializer.Deserialize<JsonElement>(responseString);
        return json.GetProperty("access_token").GetString();
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetHotels([FromQuery] string location, [FromQuery] string checkIn, [FromQuery] string checkOut)
    {
        string accessToken = await GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken))
        {
            return BadRequest("❌ Failed to obtain access token. Check console logs for details.");
        }

        // Step 1: Fetch city information
        string citySearchUrl = $"https://test.api.amadeus.com/v1/reference-data/locations?keyword={location}&subType=CITY";

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        var cityResponse = await _httpClient.GetAsync(citySearchUrl);
        string cityJson = await cityResponse.Content.ReadAsStringAsync();

        if (!cityResponse.IsSuccessStatusCode)
        {
            return BadRequest($"❌ Failed to fetch city information. Error: {cityJson}");
        }

        // ✅ Fix: Use case-insensitive deserialization
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var cityData = JsonSerializer.Deserialize<AmadeusCityResponse>(cityJson, options);

        if (cityData?.Data == null || cityData.Data.Length == 0)
            return BadRequest($"❌ Invalid city name. Response: {cityJson}");

        string cityCode = cityData.Data[0].IataCode;
        Console.WriteLine($"✅ Extracted City Code: {cityCode}");

        // Step 2: Fetch Hotel Data with the Correct City Code
        string hotelUrl = $"https://test.api.amadeus.com/v2/shopping/hotel-offers?cityCode={cityCode}&checkInDate={checkIn}&checkOutDate={checkOut}";

        Console.WriteLine($"➡️  Making hotel request to: {hotelUrl}");

        var hotelResponse = await _httpClient.GetAsync(hotelUrl);

        Console.WriteLine($"⬅️  Hotel request completed with status: {hotelResponse.StatusCode}");

        string hotelJson = await hotelResponse.Content.ReadAsStringAsync();

        if (!hotelResponse.IsSuccessStatusCode)
        {
            return BadRequest($"❌ Failed to fetch hotel data. Error: {hotelJson}");
        }

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
