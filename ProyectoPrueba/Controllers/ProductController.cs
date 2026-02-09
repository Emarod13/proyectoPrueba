using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProyectoPrueba.DTOs;
using ProyectoPrueba.Services;
using ProyectoPrueba.Wrappers;

namespace ProyectoPrueba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productsService; // Mejor private readonly

        public ProductController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        [Authorize]
        // CAMBIO: async Task<ActionResult...>
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {

           
            // CAMBIO: await
            var products = await _productsService.GetAll();

            if(products?.Any() == false)
            {
                return NotFound("No hay productos disponibles");
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var product = await _productsService.Get(id);
            if (product == null) return NotFound("Producto no encontrado"); // Mejor NotFound que BadRequest
            return Ok(product);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id) // No hace falta devolver el DTO al borrar
        {
            var deleted = await _productsService.Delete(id);
            if (deleted == false) return NotFound("Producto no encontrado");

            return Content("Producto eliminado exitosamente"); // O NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDTO>> Add(ProductDTO product)
        {
            if (product == null) return BadRequest("El producto enviado no es correcto");

            // Guardamos el resultado que ahora sí trae el ID nuevo
            var newProduct = await _productsService.Add(product);

            return Ok(newProduct);
        }

        [HttpDelete("soft-delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            var deleted= await _productsService.SoftDelete(id);
            if (deleted == false) return NotFound("Producto no encontrado");

            return Content("Producto eliminado exitosamente");
        }

        [HttpPut("update/{id}")] // <--- Aquí usamos PUT, y esperamos el ID en la URL
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, ProductDTO productDto)
        {
     
            if (id != productDto.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del producto.");
            }

 
            var exists= await _productsService.Update(id, productDto);
            if (exists == false)
            {
                return NotFound($"No se encontró el producto con id {id}");
            }

          
            return Content("Producto actualizado exitosamente"); ;
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PagedResponse<ProductDTO>>> Get(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            // Pequeña validación defensiva
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 50) pageSize = 50; // Evitar que alguien pida 1 millón de registros

            var response = await _productsService.GetAllPaged(pageNumber, pageSize);
            return Ok(response);
        }
    }
}
