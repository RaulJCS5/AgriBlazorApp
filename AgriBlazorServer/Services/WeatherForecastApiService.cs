using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using global::AgriBlazorServer.Data;
using Microsoft.Extensions.Configuration;
namespace AgriBlazorServer
{
    public class WeatherForecastApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public WeatherForecastApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"];
        }

        public async Task<WeatherForecast[]> GetWeatherForecastsAsync()
        {
            return await _httpClient.GetFromJsonAsync<WeatherForecast[]>($"{_apiBaseUrl}/weatherforecasts");
        }
    }
}
