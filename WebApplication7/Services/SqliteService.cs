using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Microsoft.Data.Sqlite;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Controls.Net7.Api.Services
{
    public class SqliteService : ISqliteService
    {
        public string _connectString;
        public SqliteService(string ConnectString)
        {
            _connectString = ConnectString;
        }
        public async Task<int> ActionUserTable(object o, SqlAction type)
        {
            string tablename = GetTableName(new UserDto());
            using (var db = SqlLiteQueryFactory(_connectString))
            {
                if (type == SqlAction.Add)
                    return await db.Query(tablename).InsertAsync(o);
                if (type == SqlAction.Delete)
                    return await db.Query(tablename).Where("UserName", o.ToString()).DeleteAsync();
                if (type == SqlAction.UpdatePassword)
                    return await db.Query(tablename).Where("UserName", (o as User).UserName).UpdateAsync(new { (o as User).Password });
                if (type == SqlAction.Update)
                    return await db.Query(tablename).Where("UserName", (o as UserDto).UserName).UpdateAsync(o);
                return -1;
            }
        }
        public async Task<IEnumerable<UserDto>> GetUserAsync(User o)
        {
            string tablename = GetTableName(new UserDto());
            using (var db = SqlLiteQueryFactory(_connectString))
                return await db.Query(tablename).Where(o).GetAsync<UserDto>();
        }
        public async Task<IEnumerable<UserDto>> SelectUserAllAsync()
        {
            string tablename = GetTableName(new UserDto());
            using (var db = SqlLiteQueryFactory(_connectString))
                return await db.Query(tablename).GetAsync<UserDto>();
        }
        public QueryFactory SqlLiteQueryFactory(string cnn)
        {
            var compiler = new SqliteCompiler();
            var connection = new SqliteConnection(cnn);
            var db = new QueryFactory(connection, compiler);
            db.Logger = result =>
            {
                Console.WriteLine(result.ToString());
            };
            if (!File.Exists("db.db"))
            {

                string creatdb = @"CREATE TABLE ""usermanger"" ( ""UserName"" TEXT NOT NULL, ""Password"" TEXT, ""Role"" TEXT, PRIMARY KEY ( ""UserName"" ) );";
                int s = db.Statement(creatdb);
            }
            return db;
        }

        public string GetTableName(object o)
        {
            return (o.GetType().GetCustomAttributes(typeof(TableAttribute), false)[0] as TableAttribute).Name;
        }
    }
}
