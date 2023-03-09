

using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Microsoft.Data.Sqlite;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.ComponentModel.DataAnnotations.Schema;

namespace Controls.Net7.Api.Db
{
  public  class SqliteService : ISqliteService
    {
        public string _connectString;
        public SqliteService(string ConnectString)
        {
            _connectString = ConnectString;   
        }
        public  async Task<int> ActionTable(object o, SqlAction type)
        {
            string tablename= GetTableName(o);
           var d= ((dynamic)o).UserName;
            using (var db = SqlLiteQueryFactory(_connectString))
            {
                if(type==SqlAction.Add)
                    return await db.Query(tablename).InsertAsync(o);
                if (type == SqlAction.Delete)
                    return await db.Query(tablename).Where("UserName", d).DeleteAsync();
                if (type==SqlAction.Update)
                    return await db.Query(tablename).UpdateAsync(o);
                return -1;
            }
        }
        public async Task<int> DeleteUser(string username)
        {
            using (var db = SqlLiteQueryFactory(_connectString))
            { 
                    return await db.Query(GetTableName(new UserDto())).Where("UserName", username).DeleteAsync();    
            }
        }
        public  async Task<IEnumerable<dynamic>> SelectAllAsync(object o)
        {
            string tablename = GetTableName(o);
            using (var db = SqlLiteQueryFactory(_connectString))
            {
                return await db.Query(tablename).GetAsync();   

            }
        }
        public  QueryFactory SqlLiteQueryFactory(string cnn)
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
