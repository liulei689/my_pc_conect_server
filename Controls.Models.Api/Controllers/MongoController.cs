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
    /// 代码库管理
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
        /// 更新
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
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost("/adddata")]
        public  ApiResult Adddata(Codess cs)
        {
            // 更新集合中的文档
            try
            {
                _mongoService.InsertOne("代码库", cs);
                return ResultOk("添加成功!");
            }
            catch (Exception ex)
            {
                return ResultOk(ex);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost("/delete")]
        public ApiResult DeleteById(string id)
        {
            // 更新集合中的文档
            try
            {
              long ifs=  _mongoService.Delete<Codess>("代码库", o=>o._id==id);
                return ResultOk(ifs);
            }
            catch (Exception ex)
            {
                return ResultOk(ex);
            }
        }
    }
}