using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using SafeScribeAPI.Services;
using System.Threading.Tasks;
using System.Linq;

namespace SafeScribeAPI.Middleware
{
    public class JwtBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService)
        {
            var jti = context.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (jti != null && blacklistService.IsTokenBlacklisted(jti))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}
