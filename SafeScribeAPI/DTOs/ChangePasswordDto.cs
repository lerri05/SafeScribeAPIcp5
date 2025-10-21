using System.ComponentModel.DataAnnotations;

namespace SafeScribeAPI.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Senha atual é obrigatória")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nova senha é obrigatória")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
        public string NewPassword { get; set; } = string.Empty;
    }
}