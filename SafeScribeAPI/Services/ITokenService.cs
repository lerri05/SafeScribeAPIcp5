using SafeScribe.Api.Models;
public interface ITokenService
{
    string GenerateToken(User user, out DateTime expiresAt);
}