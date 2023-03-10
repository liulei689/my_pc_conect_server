using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Model;
using Controls.Net7.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace Controls.Net7.Api.Controllers
{
    /// <summary>
    /// redis管理
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize("公共接口")]
    public class RedisPcController : ControllerBase
    {
        private readonly ILogger<RedisPcController> _logger;
        private readonly IRedisService _redisService;
        private readonly IJWTManager _iJWTManager;

        public RedisPcController(ILogger<RedisPcController> logger,IRedisService redisService,IJWTManager jWTManager)
        {
            _logger = logger;
            _redisService = redisService;
            _iJWTManager = jWTManager;
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("/GetRedisPcStatus")]
        public string? GetRedisPcStatus()=>_redisService.Database.StringGet("家-台式电脑-状态");

        
        /// <summary>
        /// 关机
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcClose")]
        public bool SetRedisPcClose()
        {
            //获取JWT
            string AuthorizationToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string JwtToken = AuthorizationToken!.Split(' ')[1];
            var u = _iJWTManager.SerializeJwt(JwtToken);
#pragma warning disable CS8602 // 解引用可能出现空引用。
            return  _redisService.Database.StringSet($"家-台式电脑-状态", "关机|"+ u.User.UserName+Request.Headers.UserAgent +" "+ Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString() + " " + DateTime.Now.ToString());
#pragma warning restore CS8602 // 解引用可能出现空引用。

        }
        /// <summary>
        /// 开机
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcOpen")]
        public bool SetRedisPcOpen()
        {
            //获取JWT
            string AuthorizationToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string JwtToken = AuthorizationToken!.Split(' ')[1];
            var u = _iJWTManager.SerializeJwt(JwtToken);
#pragma warning disable CS8602 // 解引用可能出现空引用。
            return _redisService.Database.StringSet($"家-台式电脑-状态", "开机|" + u.User.UserName + Request.Headers.UserAgent + Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString() + DateTime.Now.ToString());
#pragma warning restore CS8602 // 解引用可能出现空引用。
        }
        /// <summary>
        /// 开机指定机器
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcOpenByName")]
        public bool SetRedisPcOpenByName([FromQuery]string deviceStatus)
        {
            return _redisService.Database.StringSet($"家-台式电脑-状态", "开机|" + deviceStatus + Request.Headers.UserAgent + Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString() + DateTime.Now.ToString());

        }
    }
}