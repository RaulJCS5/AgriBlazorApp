using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgriBlazorServer.Data
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecast>> GetForecastsAsync();
    }

}
