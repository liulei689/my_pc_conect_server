using StackExchange.Redis;

namespace Controls.Net7.Api.Services
{
    public interface IRedisService : IDisposable
    {
        IDatabase Database { get; }
        IConnectionMultiplexer Multiplexer { get; }
    }
}
