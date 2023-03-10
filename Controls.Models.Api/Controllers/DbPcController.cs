using Controls.Models;
using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Controls.Net7.Api.Controllers
{
    /// <summary>
    /// �û�����
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DbPcController : ControllerBase
    {
        private readonly ISqliteService _sqliteService;

        public DbPcController(ISqliteService sqliteService)
        {
            _sqliteService = sqliteService;
        }
        /// <summary>
        /// ��ȡ״̬
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