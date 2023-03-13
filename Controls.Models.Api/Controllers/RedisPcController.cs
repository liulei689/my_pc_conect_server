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
    /// redis管理
    /// </summary>
    [Route("[controller]")]
    [Authorize("公共接口")]
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
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("/GetRedisPcStatus")]
        public ApiResult GetRedisPcStatus()=>ApiResult.OkData(JsonSerializer.Deserialize<PcStatus>(_redisService.Database.StringGet("家-台式电脑-状态")));       
        /// <summary>
        /// 关机
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcClose")]
        public ApiResult SetRedisPcClose()=> Common.SetPcSatusReponse( HttpContext,_iJWTManager, Request,_redisService,Cmd: PcCmd.TurnOff);
        /// <summary>
        /// 开机
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SetRedisPcOpen")]
        public ApiResult SetRedisPcOpen()=>Common.SetPcSatusReponse(HttpContext, _iJWTManager, Request, _redisService,Cmd:PcCmd.TurnOn);
        /// <summary>
        /// 指令行带参数
        /// </summary>
        /// <returns></returns>
        [HttpPost("/SetRedisPcCmd")]
        public ApiResult SetRedisPcCmd([FromBody] PcStatus cmd)
        {
          var data=  cmd;
          return  Common.SetPcSatusReponse(HttpContext, _iJWTManager, Request, _redisService, PcCmd.AddTime, data);
        }
    }
}