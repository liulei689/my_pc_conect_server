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
        /// ��ȡ״̬
        /// </summary>
        /// <returns></returns>
        [HttpGet("/GetRedisPcStatus")]
        public string? GetRedisPcStatus()
        {

            return _redisService.Database.StringGet("��-̨ʽ����-״̬");

        }
        /// <summary>
        /// �ػ�
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcClose")]
        public bool SetRedisPcClose()
        {
            return  _redisService.Database.StringSet($"��-̨ʽ����-״̬", "�ػ�|"+ Request.Headers.UserAgent +" "+ Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString() + " " + DateTime.Now.ToString());

        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcOpen")]
        public bool SetRedisPcOpen()
        {
           return  _redisService.Database.StringSet($"��-̨ʽ����-״̬", "����|"+ Request.Headers.UserAgent + Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString() + DateTime.Now.ToString());

        }
        /// <summary>
        /// ����ָ������
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcOpenByName")]
        public bool SetRedisPcOpenByName([FromQuery]DeviceStatus deviceStatus)
        {
            var ip = Request.Headers.UserAgent+Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
            return _redisService.Database.StringSet($"��-̨ʽ����-״̬", ip);

        }
    }
}