using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SafeScribeAPI.Data;
using SafeScribeAPI.DTOs;
using SafeScribeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SafeScribeAPI.Controllers
{
    [ApiController]
    [Route("api/v1/notas")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public NotesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        public async Task<IActionResult> Create([FromBody] NoteCreateDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "0");
            var note = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                UserId = userId
            };

            _db.Notes.Add(note);
            await _db.SaveChangesAsync();

            return Ok(note);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var note = await _db.Notes.FindAsync(id);
            if (note == null)
                return NotFound();

            var userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "0");
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role != UserRoles.Admin && note.UserId != userId)
                return Forbid();

            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NoteCreateDto dto)
        {
            var note = await _db.Notes.FindAsync(id);
            if (note == null)
                return NotFound();

            var userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "0");
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role != UserRoles.Admin && note.UserId != userId)
                return Forbid();

            note.Title = dto.Title;
            note.Content = dto.Content;
            note.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok(note);
        }
    }
}
