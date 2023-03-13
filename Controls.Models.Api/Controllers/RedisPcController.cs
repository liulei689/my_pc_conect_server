using Controls.Models;
using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Model;
using Controls.Net7.Api.Services;
using Controls.Net7.Api.Untiys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Controls.Net7.Api.Controllers
{
    /// <summary>
    /// redis����
    /// </summary>
    [Route("[controller]")]
    [Authorize("�����ӿ�")]
    public class RedisPcController : AppBaseController
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
        public ApiResult GetRedisPcStatus()=>ApiResult.OkData(JsonSerializer.Deserialize<PcStatus>(_redisService.Database.StringGet("��-̨ʽ����-״̬")));       
        /// <summary>
        /// �ػ�
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcClose")]
        public ApiResult SetRedisPcClose()=> Common.SetPcSatusReponse("�ػ�",HttpContext,_iJWTManager, Request,_redisService);
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcOpen")]
        public ApiResult SetRedisPcOpen()=>Common.SetPcSatusReponse("����", HttpContext, _iJWTManager, Request, _redisService);
        /// <summary>
        /// ����ָ������
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcCmd")]
        public ApiResult SetRedisPcCmd([FromQuery]string cmd) => Common.SetPcSatusReponse("����", HttpContext, _iJWTManager, Request, _redisService,cmd);
    }
}