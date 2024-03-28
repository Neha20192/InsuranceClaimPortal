using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace YCompanyWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public string Data { get; set; }
        private IHttpClientFactory HttpClientFactory { get; }
        private IConfiguration _configuration;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<WeatherForecastController> logger)
        {
            HttpClientFactory = httpClientFactory;
            _configuration = configuration;
            Data = string.Empty;
            _logger = logger;
        }

        [HttpGet]
        public async Task Get()
        {

            //using var httpClient = HttpClientFactory.CreateClient("PaymentsAPI");
            using var httpClient = HttpClientFactory.CreateClient("ThirdPartyAPI");

            // get the access token from the cookie and add it to the default request headers. the save token = true is helpful here as we have the token

            //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));
            //Data = await httpClient.GetStringAsync("/WeatherForecast");

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));
            Data = await httpClient.GetStringAsync("/ThirdParty");
        }
    }
}