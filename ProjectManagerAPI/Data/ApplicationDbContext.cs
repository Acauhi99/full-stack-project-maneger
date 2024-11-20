using Microsoft.EntityFrameworkCore;
using ProjectManagerAPI.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurar chaves primárias
        modelBuilder.Entity<Project>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<ProjectTask>()
            .HasKey(t => t.Id);

        // Configurar relações
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tarefas)
            .WithOne(t => t.Projeto)
            .HasForeignKey(t => t.ProjetoId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Tarefas)
            .WithOne(t => t.Usuario)
            .HasForeignKey(t => t.UsuarioId);
    }
}
