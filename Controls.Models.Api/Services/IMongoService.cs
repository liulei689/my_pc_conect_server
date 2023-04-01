using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Controls.Net7.Api.Services
{
    public interface IMongoService : IDisposable
    {
        IEnumerable<T> GetMany<T>(string collName, FilterDefinition<T> filter);
        Task<T> UpdateOneAsync<T>(string collName, Expression<Func<T, bool>> filter, UpdateDefinition<T> update);
    }
}
