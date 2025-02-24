using System.ComponentModel.DataAnnotations;

namespace Practica.DTOs
{
    public class CategoriaDTOs
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
    }
}
