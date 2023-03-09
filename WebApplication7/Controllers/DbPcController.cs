using Controls.Models;
using Controls.Net7.Api.Db;
using Controls.Net7.Api.Jwt;
using Controls.Net7.Api.Model;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.Api.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace Controls.Net7.Api.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DbPcController : ControllerBase
    {
        private readonly ILogger<DbPcController> _logger;
        private readonly ISqliteService _sqliteService;
        private readonly IJWTManager _iJWTManager;

        public DbPcController(ILogger<DbPcController> logger, ISqliteService sqliteService, IJWTManager jWTManager)
        {
            _logger = logger;
            _sqliteService = sqliteService;
            _iJWTManager = jWTManager;
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("/AddUser")]
        public Task<int> AddUser(UserDto userDto)=> _sqliteService.ActionTable(userDto, SqlAction.Add);

        [HttpPost("/UpdateUser")]
        public Task<int> UpdateUser(UserDto userDto) => _sqliteService.ActionTable(userDto, SqlAction.Update);

        [HttpPost("/DeleteUser")]
        public Task<int> DeleteUser(string username) => _sqliteService.DeleteUser(username);
    }
}