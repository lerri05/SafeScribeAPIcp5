using SafeScribeAPI.Models;

namespace SafeScribeAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user, out DateTime expiresAt);
    }
}
