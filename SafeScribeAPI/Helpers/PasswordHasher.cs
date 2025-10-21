using BCrypt.Net;

namespace SafeScribeAPI.Helpers
{
    public static class PasswordHasher
    {
        /// <summary>
        /// Gera um hash seguro a partir de uma senha em texto puro.
        /// </summary>
        /// <param name="password">Senha em texto puro.</param>
        /// <returns>Hash criptografado da senha.</returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verifica se a senha informada corresponde ao hash armazenado.
        /// </summary>
        /// <param name="password">Senha em texto puro.</param>
        /// <param name="hashedPassword">Hash armazenado no banco de dados.</param>
        /// <returns>Verdadeiro se corresponder, falso caso contrário.</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
