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
    /// redis����
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize("�����ӿ�")]
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
        /// ��ȡ״̬
        /// </summary>
        /// <returns></returns>
        [HttpGet("/GetRedisPcStatus")]
        public string? GetRedisPcStatus()=>_redisService.Database.StringGet("��-̨ʽ����-״̬");

        
        /// <summary>
        /// �ػ�
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcClose")]
        public bool SetRedisPcClose()
        {
            //��ȡJWT
            string AuthorizationToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string JwtToken = AuthorizationToken!.Split(' ')[1];
            var u = _iJWTManager.SerializeJwt(JwtToken);
#pragma warning disable CS8602 // �����ÿ��ܳ��ֿ����á�
            return  _redisService.Database.StringSet($"��-̨ʽ����-״̬", "�ػ�|"+ u.User.UserName+Request.Headers.UserAgent +" "+ Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString() + " " + DateTime.Now.ToString());
#pragma warning restore CS8602 // �����ÿ��ܳ��ֿ����á�

        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcOpen")]
        public bool SetRedisPcOpen()
        {
            //��ȡJWT
            string AuthorizationToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string JwtToken = AuthorizationToken!.Split(' ')[1];
            var u = _iJWTManager.SerializeJwt(JwtToken);
#pragma warning disable CS8602 // �����ÿ��ܳ��ֿ����á�
            return _redisService.Database.StringSet($"��-̨ʽ����-״̬", "����|" + u.User.UserName + Request.Headers.UserAgent + Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString() + DateTime.Now.ToString());
#pragma warning restore CS8602 // �����ÿ��ܳ��ֿ����á�
        }
        /// <summary>
        /// ����ָ������
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcOpenByName")]
        public bool SetRedisPcOpenByName([FromQuery]string deviceStatus)
        {
            return _redisService.Database.StringSet($"��-̨ʽ����-״̬", "����|" + deviceStatus + Request.Headers.UserAgent + Request.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString() + DateTime.Now.ToString());

        }
    }
}