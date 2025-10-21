using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SafeScribeAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty; 

        [Required]
        public string Role { get; set; } = "User";

        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}