using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ProyectoPrueba.Controllers;
using ProyectoPrueba.DTOs; // <--- IMPORTANTE: Usamos DTOs
using ProyectoPrueba.Services; // <--- IMPORTANTE: Usamos el Servicio
using System.Threading.Tasks;

namespace ProyectoPrueba.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductsService> _mockService;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockService = new Mock<IProductsService>();
            _controller = new ProductController(_mockService.Object);
        }

        [Fact]
        public async Task Get_SiExiste_DevuelveOkConDTO()
        {
            // --- ARRANGE ---
            int idPrueba = 1;

            // 1. CLAVE: Creamos un DTO, no una Entidad, porque eso devuelve tu Service
            var dtoEsperado = new ProductDTO
            {
                Id = 1,
                Name = "Coca Cola",
                Price = 1500
            };

            // 2. Configuramos el mock para devolver ese DTO
            _mockService.Setup(s => s.Get(idPrueba))
                        .ReturnsAsync(dtoEsperado);

            // --- ACT ---
            // Recibimos un ActionResult<ProductDTO>
            var resultado = await _controller.Get(idPrueba);

            // --- ASSERT ---

            // 3. CLAVE: Al usar ActionResult<T>, el OkObjectResult está en la propiedad .Result
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);

            // 4. CLAVE: Verificamos que el valor dentro sea ProductDTO
            var dtoDevuelto = Assert.IsType<ProductDTO>(okResult.Value);

            Assert.Equal("Coca Cola", dtoDevuelto.Name);
        }

        [Fact]
        public async Task Get_SiNoExiste_DevuelveNotFound()
        {
            // --- ARRANGE ---
            int idMalo = 99;
            _mockService.Setup(s => s.Get(idMalo))
                        .ReturnsAsync((ProductDTO)null); // Simulamos que retorna null

            // --- ACT ---
            var resultado = await _controller.Get(idMalo);

            // --- ASSERT ---
            // Cuando es NotFound, suele venir en .Result también
            Assert.IsType<NotFoundObjectResult>(resultado.Result);
            // Nota: Usamos NotFoundObjectResult porque tu código devuelve NotFound("mensaje")
            // Si fuera NotFound() solo, sería NotFoundResult.
        }
    }
}