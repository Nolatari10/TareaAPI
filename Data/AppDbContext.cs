using Microsoft.EntityFrameworkCore;
using TareaAPI.Models;

namespace TareaAPI.Data;

public class AppDbContext : DbContext
{
    public DbSet<Tarea> Tareas => Set<Tarea>();
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(200);
        });
    }
}
