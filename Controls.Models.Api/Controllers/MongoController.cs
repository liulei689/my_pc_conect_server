using Controls.Models;
using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Controls.Net7.Api.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("[controller]")]

    public class MongoController :  AppBaseController
    {
        private readonly IMongoService _mongoService;

        public MongoController(IMongoService mongoService)
        {
            _mongoService = mongoService;
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        [HttpPost("/alldata")]
        public  ApiResult Alldata() => ResultOk( _mongoService.GetMany("代码库", Builders<Codess>.Filter.Ne("_id", "")));
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        [HttpPost("/updatedata")]
        public async Task<ApiResult> updatedata(Codess cs)
        {
            UpdateDefinition<Codess> update = Builders<Codess>.Update
         .Set(o => o.Use, cs.Use)
         .Set(o => o.Technical, cs.Technical)
         .Set(o => o.UseDetail, cs.UseDetail)
         .Set(o => o.From, cs.From)
         .Set(o => o.Language, cs.Language)
         .Set(o => o.TimeUpate, cs.TimeUpate)
         .Set(o => o.CreateTime, cs.CreateTime)
         .Set(o => o.Code, cs.Code)
         .Set(o => o.ReadCount, cs.ReadCount)
         .Set(o => o._id, cs._id);
            // 更新集合中的文档
          var codes= await _mongoService.UpdateOneAsync("代码库", o => o._id == cs._id, update);
           return ResultOk(codes);
        }
    }
}