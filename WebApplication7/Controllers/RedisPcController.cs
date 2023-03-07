using Controls.Net7.Api.Model;
using Controls.Net7.Api.Redis;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace Controls.Net7.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisPcController : ControllerBase
    {
        private readonly ILogger<RedisPcController> _logger;
        private readonly IRedisService _redisService;

        public RedisPcController(ILogger<RedisPcController> logger,IRedisService redisService )
        {
            _logger = logger;
            _redisService = redisService;
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("/GetRedisPcStatus")]
        public string? GetRedisPcStatus()
        {

            return _redisService.Database.StringGet("家-台式电脑-状态");

        }
        /// <summary>
        /// 关机
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcClose")]
        public bool SetRedisPcClose()
        {
            return  _redisService.Database.StringSet($"家-台式电脑-状态", "关机|"+ Request.Headers.UserAgent +" "+ Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString() + " " + DateTime.Now.ToString());

        }
        /// <summary>
        /// 开机
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcOpen")]
        public bool SetRedisPcOpen()
        {
           return  _redisService.Database.StringSet($"家-台式电脑-状态", "开机|"+ Request.Headers.UserAgent + Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString() + DateTime.Now.ToString());

        }
        /// <summary>
        /// 开机指定机器
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcOpenByName")]
        public bool SetRedisPcOpenByName([FromQuery]DeviceStatus deviceStatus)
        {
            var ip = Request.Headers.UserAgent+Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
            return _redisService.Database.StringSet($"家-台式电脑-状态", ip);

        }
    }
}