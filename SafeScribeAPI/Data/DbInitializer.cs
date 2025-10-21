using SafeScribeAPI.Data;
using SafeScribeAPI.Models;

namespace SafeScribeAPI
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            
            context.Database.EnsureCreated();

            
            if (context.Users.Any())
                return;

           
            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = UserRoles.Admin
                },
                new User
                {
                    Username = "editor",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("editor123"),
                    Role = UserRoles.Editor
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            
            var notes = new List<Note>
            {
                new Note
                {
                    Title = "Nota Administrativa",
                    Content = "Esta nota pertence ao administrador do sistema.",
                    UserId = users[0].Id
                },
                new Note
                {
                    Title = "Nota do Editor",
                    Content = "Esta é uma nota de teste criada para o editor.",
                    UserId = users[1].Id
                }
            };

            context.Notes.AddRange(notes);
            context.SaveChanges();
        }
    }
}
