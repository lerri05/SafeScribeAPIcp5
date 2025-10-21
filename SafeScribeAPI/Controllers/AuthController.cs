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
        if (_db.Users.Any(u => u.Username == dto.Username)) return BadRequest("Username já existe");


        var user = new User
        {
            Username = dto.Username,
            Role = dto.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Ok();
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var user = _db.Users.SingleOrDefault(u => u.Username == dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return Unauthorized();


        var token = _tokenService.GenerateToken(user, out var expiresAt);
        return Ok(new { accessToken = token, expiresAt });
    }


    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var jti = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        if (jti != null) await _blacklist.AddToBlacklistAsync(jti);
        return Ok();
    }
}