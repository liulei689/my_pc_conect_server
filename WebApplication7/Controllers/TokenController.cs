
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Controls.Net7.Api.Jwt;
using Controls.Net7.Api.Model.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Controls.Net7.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [EnableCors]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IJWTManager _jWTManager;
        private readonly ILogger<TokenController> _logger;

        public TokenController(ILogger<TokenController> logger, IConfiguration configuration, IJWTManager authService)
        {
            _logger = logger;
            _configuration = configuration;
            _jWTManager = authService;

        }
      
        [AllowAnonymous]
        [HttpPost]
        public string GetToken([FromBody] UserDto request)
        {
            if (request.Password == "1")
            {
                //登录验证之后获取的用户信息
                TokenModelJwt tokenModelJwt = new TokenModelJwt
                {
                   
                    Uid = 10001,
                    User = request,
                    Role= request.Role,

                   
                };
                var jwtToken = _jWTManager.IssueJwt(tokenModelJwt);
                return jwtToken;
            }
            return "密码错误";
        }
    }
    
}
