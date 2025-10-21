using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.EntityFrameworkCore;
using SafeScribeAPI.Data;
using SafeScribeAPI.Models;
using SafeScribeAPI.DTOs;
using SafeScribeAPI.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SafeScribeAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ITokenService _tokenService;
        private readonly ITokenBlacklistService _blacklist;

        public AuthController(ApplicationDbContext db, ITokenService tokenService, ITokenBlacklistService blacklist)
        {
            _db = db;
            _tokenService = tokenService;
            _blacklist = blacklist;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { message = "Username já existe" });

            
            if (dto.Password.Length < 8)
                return BadRequest(new { message = "A senha deve ter no mínimo 8 caracteres" });

            var user = new User
            {
                Username = dto.Username,
                Role = string.IsNullOrEmpty(dto.Role) ? "User" : dto.Role, // Valor padrão
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Usuário registrado com sucesso",
                userId = user.Id,
                username = user.Username
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);

            
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Credenciais inválidas" });

            
            var token = _tokenService.GenerateToken(user, out var expiresAt);

            return Ok(new
            {
                accessToken = token,
                expiresAt,
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    role = user.Role
                }
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var jti = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (string.IsNullOrEmpty(jti))
                return BadRequest(new { message = "Token inválido" });

            await _blacklist.AddToBlacklistAsync(jti);

            return Ok(new { message = "Logout realizado com sucesso" });
        }

        
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? "0");
            var user = await _db.Users.FindAsync(userId);

            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                role = user.Role
            });
        }

        
        [HttpPost("trocar-senha")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? "0");
            var user = await _db.Users.FindAsync(userId);

            if (user == null)
                return NotFound(new { message = "Usuário não encontrado" });

            
            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
                return BadRequest(new { message = "Senha atual incorreta" });

            
            if (dto.NewPassword.Length < 8)
                return BadRequest(new { message = "A nova senha deve ter no mínimo 8 caracteres" });

            
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Senha alterada com sucesso" });
        }
    }
}