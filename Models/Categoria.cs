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


        public ICollection<Producto> Productos { get; set; }
    }
}
