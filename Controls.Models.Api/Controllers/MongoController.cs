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
    /// �û�����
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
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        [HttpPost("/alldata")]
        public  ApiResult Alldata() => ResultOk( _mongoService.GetMany("�����", Builders<Codess>.Filter.Ne("_id", "")));
    }
}