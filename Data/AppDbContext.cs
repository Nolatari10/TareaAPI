using Microsoft.EntityFrameworkCore;
using TareaAPI.Models;

namespace TareaAPI.Data;

public class AppDbContext : DbContext
{
    public DbSet<Tarea> Tareas => Set<Tarea>();

    public DbSet<Categoria> Categorias => Set<Categoria>();

    //created for user authentication
    public DbSet<User> Users => Set<User>();
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(200);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.categoriaId);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
        });
    }
}
