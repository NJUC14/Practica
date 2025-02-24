using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Practica.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        // Relación: Una Categoria puede tener muchos Productos
        public ICollection<Producto> Productos { get; set; }
    }
}
