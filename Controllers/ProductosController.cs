using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Practica.Models;
using Practica.Data;
using Practica.DTOs;

namespace TiendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly PracticaContext _context;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(PracticaContext context, ILogger<ProductosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTOs>>> GetProductos()
        {
            var productos = await _context.Productos.ToListAsync();
            var productosDTO = productos.Select(p => new ProductoDTOs
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock,
                FechaCreacion = p.FechaCreacion,
                CategoriaId = p.CategoriaId
            });

            return Ok(productosDTO);
        }

        // GET: api/productos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDTOs>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                _logger.LogWarning("Producto con id {Id} no encontrado", id);
                return NotFound();
            }

            var productoDTO = new ProductoDTOs
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock,
                FechaCreacion = producto.FechaCreacion,
                CategoriaId = producto.CategoriaId
            };

            return Ok(productoDTO);
        }

        // POST: api/productos
        [HttpPost]
        public async Task<ActionResult<ProductoDTOs>> CreateProducto(ProductoDTOs productoDTO)
        {
            if (productoDTO.Precio <= 0)
            {
                ModelState.AddModelError("Precio", "El precio debe ser mayor a 0.");
            }
            if (productoDTO.Stock < 0)
            {
                ModelState.AddModelError("Stock", "El stock debe ser mayor o igual a 0.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = new Producto
            {
                Nombre = productoDTO.Nombre,
                Precio = productoDTO.Precio,
                Stock = productoDTO.Stock,
                CategoriaId = productoDTO.CategoriaId,
                FechaCreacion = DateTime.Now
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            productoDTO.Id = producto.Id;
            productoDTO.FechaCreacion = producto.FechaCreacion;

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, productoDTO);
        }

        // PUT: api/productos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, ProductoDTOs productoDTO)
        {
            if (id != productoDTO.Id)
            {
                return BadRequest();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            producto.Nombre = productoDTO.Nombre;
            producto.Precio = productoDTO.Precio;
            producto.Stock = productoDTO.Stock;
            producto.CategoriaId = productoDTO.CategoriaId;

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error al actualizar el producto con id {Id}", id);
                throw;
            }

            return NoContent();
        }

        // DELETE: api/productos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
