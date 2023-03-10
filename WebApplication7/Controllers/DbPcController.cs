using Controls.Models;
using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.Api.Services;
using Microsoft.AspNetCore.Mvc;

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
        public Task<int> AddUser(UserDto userDto)=> _sqliteService.ActionUserTable(userDto, SqlAction.Add);

        [HttpPost("/UpdateUserPassword")]
        public Task<int> UpdateUserPassword(User userDto) => _sqliteService.ActionUserTable(userDto, SqlAction.UpdatePassword);
        [HttpPost("/UpdateUser")]
        public Task<int> UpdateUser(UserDto userDto) => _sqliteService.ActionUserTable(userDto, SqlAction.Update);

        [HttpPost("/DeleteUser")]
        public Task<int> DeleteUser(string username) => _sqliteService.ActionUserTable(username, SqlAction.Delete);
        [HttpPost("/SelectAllUser")]
        public Task<IEnumerable<UserDto>> SelectAllUser() => _sqliteService.SelectUserAllAsync();
    }
}