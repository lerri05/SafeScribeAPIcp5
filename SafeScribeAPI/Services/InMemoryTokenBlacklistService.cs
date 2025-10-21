using System.Collections.Concurrent;

namespace SafeScribeAPI.Services
{
    public class InMemoryTokenBlacklistService : ITokenBlacklistService
    {
        private readonly ConcurrentDictionary<string, DateTime> _blacklist = new();

        public Task AddToBlacklistAsync(string jti)
        {
            _blacklist[jti] = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public bool IsTokenBlacklisted(string jti)
        {
            return _blacklist.ContainsKey(jti);
        }
    }
}
