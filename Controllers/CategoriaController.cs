using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica.Data;
using Practica.Models;
using Practica.DTOs;

namespace TiendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly TiendaContext _context;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(TiendaContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
            _logger = logger;
        }

 
        [HttpPost]
        public async Task<ActionResult<CategoriaDTOs>> CreateCategoria(CategoriaDTOs categoriaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar que no exista una categoría con el mismo nombre (ya que debe ser única)
            var categoriaExistente = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Nombre == categoriaDTO.Nombre);
            if (categoriaExistente != null)
            {
                return Conflict("Ya existe una categoría con el mismo nombre.");
            }

            var categoria = new Categoria
            {
                Nombre = categoriaDTO.Nombre
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            categoriaDTO.Id = categoria.Id;

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoriaDTO);
        }

 
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTOs>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                _logger.LogWarning("Categoría con id {Id} no encontrada", id);
                return NotFound();
            }

            var categoriaDTO = new CategoriaDTOs
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };

            return Ok(categoriaDTO);
        }
    }
}
