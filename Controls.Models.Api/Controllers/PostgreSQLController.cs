using Controls.Models;
using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using static MongoDB.Driver.WriteConcern;

namespace Controls.Net7.Api.Controllers
{
    /// <summary>
    /// 管理
    /// </summary>
    [Route("[controller]")]

    public class PostgreSQLController :  AppBaseController
    {
        private readonly IPostgreSQLService _postgreSQLService;
        private readonly IMongoService _iMongoService;
        public static int _count = 0;
        public PostgreSQLController(IPostgreSQLService postgreSQLService, IMongoService iMongoService)
        {
            _postgreSQLService = postgreSQLService;
            _iMongoService = iMongoService; 
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("/Addp")]
        public async Task<ApiResult> AddUser(PostSql userDto)=> ResultOk(await _postgreSQLService.ActionUserTable(userDto, SqlAction.Add));

        [HttpPost("/Updateid")]
        public async Task<ApiResult> UpdateUserPassword(PostSql userDto)=> ResultOk(await _postgreSQLService.ActionUserTable(userDto, SqlAction.UpdatePassword));
        [HttpPost("/Updateall")]
        public async Task<ApiResult> UpdateUser(PostSql userDto) => ResultOk(await _postgreSQLService.ActionUserTable(userDto, SqlAction.Update));

        [HttpPost("/Deletebyid")]
        public async Task<ApiResult> DeleteUser(string id) => ResultOk(await _postgreSQLService.ActionUserTable(id, SqlAction.Delete));
        [HttpPost("/Deleteall")]
        public async Task<ApiResult> DeleteAll() => ResultOk(await _postgreSQLService.ActionUserTable(0, SqlAction.deleteall));
        [HttpPost("/SelectAllp")]
        public async Task<ApiResult> SelectAllUser() => ResultOk(await _postgreSQLService.SelectUserAllAsync());
        [HttpPost("/backup_monogodb_to_postgresql")]
        public async Task<ApiResult> BMTP() 
        {
          var datas=  _iMongoService.GetMany("代码库", Builders<Codess>.Filter.Ne("_id", ""));

           // var pqdata = await _postgreSQLService.SelectUserAllAsync();
            if (datas.Count() != _count)
            {
                _count = datas.Count();
                int counts = await _postgreSQLService.ActionUserTable(0, SqlAction.deleteall);
                int infect = 0;
                string message = "";
                foreach (var data in datas)
                {
                    PostSql postSql = new PostSql();
                    postSql._id = data._id;
                    postSql.timeupate = data.TimeUpate;
                    postSql.createtime = data.TimeUpate;
                    postSql.froms = data.From;
                    postSql.code = data.Code;
                    postSql.languages = data.Language;
                    postSql.readtime = data.ReadTime;
                    postSql.readcount = data.ReadCount;
                    postSql.usedetail = data.UseDetail;
                    postSql.technical = data.Technical;
                    postSql.use = data.Use;
                    try
                    {
                        int c = await _postgreSQLService.ActionUserTable(postSql, SqlAction.Add);
                        infect = infect + c;
                    }
                    catch (Exception ex) { message = ex.Message; }
                }
                return ResultOk("删除条数：" + counts + ",查询到芒果条数：" + datas.Count() + ",postgresql插入数：" + infect + ",提示" + message + "," + DateTime.Now.ToString() + "\r\n");
            }return ResultOk("未检出到新增数据" + DateTime.Now.ToString()+"\r\n");

        }
    }
}