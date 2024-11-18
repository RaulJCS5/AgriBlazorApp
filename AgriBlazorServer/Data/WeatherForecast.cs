using System;

namespace AgriBlazorServer.Data
{
    public class WeatherForecast
    {
        public int Id { get; set; } // Primary key
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
