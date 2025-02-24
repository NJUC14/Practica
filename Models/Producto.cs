using System;
using System.ComponentModel.DataAnnotations;

namespace Practica.Models
{

    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0.")]
        public int Stock { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relación con Categoria (Producto pertenece a una sola Categoria)
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
    
