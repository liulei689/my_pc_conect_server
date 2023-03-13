using Controls.Models;
using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controls.Net7.Api.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("[controller]")]

    public class DbPcController :  AppBaseController
    {
        private readonly ISqliteService _sqliteService;

        public DbPcController(ISqliteService sqliteService)
        {
            _sqliteService = sqliteService;
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("/AddUser")]
        public async Task<ApiResult> AddUser(UserDto userDto)=> ResultOk(await _sqliteService.ActionUserTable(userDto, SqlAction.Add));

        [HttpPost("/UpdateUserPassword")]
        public async Task<ApiResult> UpdateUserPassword(User userDto)=> ResultOk(await _sqliteService.ActionUserTable(userDto, SqlAction.UpdatePassword));
        [HttpPost("/UpdateUser")]
        public async Task<ApiResult> UpdateUser(UserDto userDto) => ResultOk(await _sqliteService.ActionUserTable(userDto, SqlAction.Update));

        [HttpPost("/DeleteUser")]
        public async Task<ApiResult> DeleteUser(string username) => ResultOk(await _sqliteService.ActionUserTable(username, SqlAction.Delete));
        [HttpPost("/SelectAllUser")]
        public async Task<ApiResult> SelectAllUser() => ResultOk(await _sqliteService.SelectUserAllAsync());
    }
}