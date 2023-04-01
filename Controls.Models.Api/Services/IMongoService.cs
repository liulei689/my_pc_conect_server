using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using MongoDB.Driver;

namespace Controls.Net7.Api.Services
{
    public interface IMongoService : IDisposable
    {
        IEnumerable<T> GetMany<T>(string collName, FilterDefinition<T> filter);
    }
}
