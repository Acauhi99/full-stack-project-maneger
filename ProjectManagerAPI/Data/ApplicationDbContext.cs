using Microsoft.EntityFrameworkCore;
using ProjectManagerAPI.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ProjectTask> ProjectTask { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder == null)
            throw new ArgumentNullException(nameof(modelBuilder));

        // Primary Keys
        modelBuilder.Entity<Project>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<ProjectTask>()
            .HasKey(t => t.Id);

        // Project -> ProjectTask relationship
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tarefas)
            .WithOne()
            .HasForeignKey(t => t.ProjetoId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // User -> ProjectTask relationship
        modelBuilder.Entity<User>()
            .HasMany(u => u.Tarefas)
            .WithOne()
            .HasForeignKey(t => t.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Indexes
        modelBuilder.Entity<ProjectTask>()
            .HasIndex(t => t.ProjetoId);

        modelBuilder.Entity<ProjectTask>()
            .HasIndex(t => t.UsuarioId);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Property configurations
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        modelBuilder.Entity<User>()
            .Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Project>()
            .Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<ProjectTask>()
            .Property(t => t.Titulo)
            .IsRequired()
            .HasMaxLength(100);
    }
}
