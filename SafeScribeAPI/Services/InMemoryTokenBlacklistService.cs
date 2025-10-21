public class InMemoryTokenBlacklistService : ITokenBlacklistService
{
    private readonly HashSet<string> _blacklist = new();


    public Task AddToBlacklistAsync(string jti)
    {
        _blacklist.Add(jti);
        return Task.CompletedTask;
    }


    public Task<bool> IsBlacklistedAsync(string jti)
    {
        return Task.FromResult(_blacklist.Contains(jti));
    }
}