using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.ComponentModel.DataAnnotations.Schema;

namespace Controls.Net7.Api.Services
{
    public class PostgreSQLService : IPostgreSQLService
    {
        public string _connectString;
        public PostgreSQLService(string ConnectString)
        {
            _connectString = ConnectString;
        }
        public async Task<int> ActionUserTable(object o, SqlAction type)
        {
            string tablename = GetTableName(new PostSql());
            using (var db = PostgresQueryFactory(_connectString))
            { 
                if(type==SqlAction.deleteall)
                    return await db.Query(tablename).DeleteAsync();
                if (type == SqlAction.Add)
                    return await db.Query(tablename).InsertAsync(o);
                if (type == SqlAction.Delete)
                    return await db.Query(tablename).Where("_id", o.ToString()).DeleteAsync();
                if (type == SqlAction.UpdatePassword)
                    return await db.Query(tablename).Where("_id", (o as PostSql)._id).UpdateAsync(new { (o as PostSql)._id });
                if (type == SqlAction.Update)
                    return await db.Query(tablename).Where("_id", (o as PostSql)._id).UpdateAsync(o);
                return -1;
            }
        }
        public async Task<IEnumerable<PostSql>> GetUserAsync(PostSql o)
        {
            string tablename = GetTableName(new PostSql());
            using (var db = PostgresQueryFactory(_connectString))
                return await db.Query(tablename).Where(o).GetAsync<PostSql>();
        }
        public async Task<IEnumerable<PostSql>> SelectUserAllAsync()
        {
            string tablename = GetTableName(new PostSql());
            using (var db = PostgresQueryFactory(_connectString))
                return await db.Query(tablename).GetAsync<PostSql>();
        }
        public QueryFactory PostgresQueryFactory(string cnn)
        {
            var compiler = new PostgresCompiler();
            var connection = new NpgsqlConnection(cnn);
            var db = new QueryFactory(connection, compiler);
            db.Logger = result =>
            {
                Console.WriteLine(result.ToString());
            };
            return db;
        }

        public string GetTableName(object o)
        {
            return (o.GetType().GetCustomAttributes(typeof(TableAttribute), false)[0] as TableAttribute).Name;
        }
    }
}
