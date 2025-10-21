using Microsoft.EntityFrameworkCore;
using SafeScribeAPI.Models;

namespace SafeScribeAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tabelas do banco
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.PasswordHash)
                      .IsRequired();

                entity.Property(u => u.Role)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasMany(u => u.Notes)
                      .WithOne(n => n.User)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração da entidade Note
            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.Property(n => n.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(n => n.Content)
                      .IsRequired();

                entity.Property(n => n.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(n => n.UpdatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Dados iniciais (seeding)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = UserRoles.Admin
                },
                new User
                {
                    Id = 2,
                    Username = "editor",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("editor123"),
                    Role = UserRoles.Editor
                }
            );

            modelBuilder.Entity<Note>().HasData(
                new Note
                {
                    Id = 1,
                    Title = "Primeira Nota",
                    Content = "Esta é uma nota criada automaticamente no seeding.",
                    UserId = 1
                },
                new Note
                {
                    Id = 2,
                    Title = "Nota de Teste",
                    Content = "Exemplo de nota pertencente ao editor.",
                    UserId = 2
                }
            );
        }
    }
}
