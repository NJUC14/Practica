using Microsoft.EntityFrameworkCore;
using Practica.Models;

namespace Practica.Data
{
    public class TiendaContext : DbContext
    {
        public TiendaContext(DbContextOptions<TiendaContext> options) : base(options) { }

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
                      .HasColumnType("decimal(18,2)"); 

                entity.Property(p => p.Stock)
                      .IsRequired();

                entity.Property(p => p.FechaCreacion)
                      .HasDefaultValueSql("GETDATE()");


                entity.HasOne(p => p.Categoria)
                      .WithMany(c => c.Productos)
                      .HasForeignKey(p => p.CategoriaId);
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(c => c.Nombre)
                      .IsRequired()
                      .HasMaxLength(50);


                entity.HasIndex(c => c.Nombre)
                      .IsUnique();
            });
        }
    }
}
