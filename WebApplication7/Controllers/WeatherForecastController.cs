
using Controls.Net7.Api.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controls.Net7.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IJWTManager _iJWTManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IJWTManager iJWTManager
            )
        {
            _logger = logger;
            _iJWTManager = iJWTManager;
        }

        [HttpGet]
        public int Get()
        {
            //获取JWT
            string AuthorizationToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string JwtToken = AuthorizationToken.Split(' ')[1];

            //解析JWT中的用户信息
            var u = _iJWTManager.SerializeJwt(JwtToken);

            var rng = new Random();
            return 1;
        }
    }
}
