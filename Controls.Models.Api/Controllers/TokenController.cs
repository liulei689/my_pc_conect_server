
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Controls.Models;
using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using static Humanizer.In;

namespace Controls.Net7.Api.Controllers
{
    /// <summary>
    /// token相关
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors]
    public class TokenController : AppBaseController
    {
        private readonly IJWTManager _jWTManager;
        private readonly ISqliteService _sqliteService;

        public TokenController(IJWTManager authService,ISqliteService sqliteService)
        {
            _jWTManager = authService;
            _sqliteService =sqliteService;

        }
      
        [AllowAnonymous]
        [HttpPost]
        public async Task<ApiResult> GetToken([FromBody] User request)
        {
           var users= await _sqliteService.GetUserAsync(request);
            if (users.Count() < 1) return ResultWarn("用户不存在");
            if (users.Count() > 1) return ResultWarn("用户重复，请联系管理员");
            var usersl =users.ToList();
                //登录验证之后获取的用户信息
                TokenModelJwt tokenModelJwt = new TokenModelJwt
                {                  
                    Uid = 10001,
                    User = usersl[0],
                    Role= usersl[0].Role,                   
                };
                var jwtToken = "Bearer "+_jWTManager.IssueJwt(tokenModelJwt);
            return await Task.FromResult(this.ResultOk(
                "token",
                jwtToken
            ));         
        }
    }
    
}
