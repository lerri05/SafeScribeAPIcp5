using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SafeScribe.Api.Models;


public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expireMinutes;


    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = _config["Jwt:Key"];
        _issuer = _config["Jwt:Issuer"];
        _audience = _config["Jwt:Audience"];
        _expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"] ?? "60");
    }


    public string GenerateToken(User user, out DateTime expiresAt)
    {
        var claims = new List<Claim>
{
new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
new Claim(ClaimTypes.Role, user.Role),
new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        expiresAt = DateTime.UtcNow.AddMinutes(_expireMinutes);


        var token = new JwtSecurityToken(
        issuer: _issuer,
        audience: _audience,
        claims: claims,
        expires: expiresAt,
        signingCredentials: creds
        );


        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}