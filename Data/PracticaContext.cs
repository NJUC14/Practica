using Microsoft.EntityFrameworkCore;
using Practica.Models;

namespace Practica.Data
{
    public class PracticaContext : DbContext
    {
        public PracticaContext(DbContextOptions<PracticaContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(p => p.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.Precio)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)"); // Especificamos el tipo de columna

                entity.Property(p => p.Stock)
                      .IsRequired();

                entity.Property(p => p.FechaCreacion)
                      .HasDefaultValueSql("GETDATE()");

                // Configuración de la relación con Categoria
                entity.HasOne(p => p.Categoria)
                      .WithMany(c => c.Productos)
                      .HasForeignKey(p => p.CategoriaId);
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(c => c.Nombre)
                      .IsRequired()
                      .HasMaxLength(50);

                // Aseguramos que el nombre sea único
                entity.HasIndex(c => c.Nombre)
                      .IsUnique();
            });
        }
    }
}
