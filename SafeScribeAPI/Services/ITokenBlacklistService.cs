namespace SafeScribeAPI.Services
{
    public interface ITokenBlacklistService
    {
        Task AddToBlacklistAsync(string jti);
        bool IsTokenBlacklisted(string jti);
    }
}
