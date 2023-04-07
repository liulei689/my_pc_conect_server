using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Microsoft.Data.Sqlite;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Controls.Net7.Api.Services
{
    public interface IPostgreSQLService
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        string GetTableName(object o);
        Task<int> ActionUserTable(object o, SqlAction type);
        Task<IEnumerable<PostSql>> SelectUserAllAsync();
        Task<IEnumerable<PostSql>> GetUserAsync(PostSql o);
    }
}
