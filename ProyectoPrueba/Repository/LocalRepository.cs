using ProyectoPrueba.DTOs;

namespace ProyectoPrueba.Repository
{
    public class LocalRepository : IRepository<ProductDTO, int>
    {
        public List<ProductDTO> products { get; set; }

        public LocalRepository() =>
  
            products = CargarProductosDePrueba();
        

        private List<ProductDTO> CargarProductosDePrueba()
            {
                return new List<ProductDTO>
        {
            new ProductDTO(
                1,
                "Laptop Gamer X1",
                "Notebook de alto rendimiento con 16GB RAM y tarjeta gráfica dedicada.",
                15,
                1200.99m, ""
            ),
            new ProductDTO(
                2,
                "Teclado Mecánico RGB",
                "Teclado con switches azules y retroiluminación personalizable.",
                50,
                85.50m, ""
            ),
            new ProductDTO(
                3,
                "Monitor 24' 144Hz",
                "Monitor ideal para esports con tasa de refresco ultra rápida.",
                20,
                250.00m, ""
            ),
            new ProductDTO(
                4,
                "Mouse Inalámbrico Pro",
                "Mouse ergonómico con batería de larga duración y sensor óptico.",
                100,
                45.99m, ""
            ),
            new ProductDTO(
                5,
                "Auriculares Noise Cancelling",
                "Auriculares over-ear con cancelación de ruido activa.",
                30,
                150.75m, ""
            ),
            new ProductDTO(
                6,
                "Silla de Escritorio Ergonomica",
                "Silla con soporte lumbar ajustable y materiales transpirables.",
                10,
                320.00m, ""
            )
        };
            }
        public async Task<ProductDTO?> Add(ProductDTO entity)
        {
                products.Add(entity);
                return entity;
        }

        public async Task<bool> Delete(int id)
        {
            foreach (ProductDTO p in products)
            {
                if (p.Id == id)
                {
                    products.Remove(p);
                    return true;
                }
            }
            return false;
           
        }

        public async Task<IEnumerable<ProductDTO>?> GetAll()
        {
            return products;
        }

        public async Task<ProductDTO?> GetByID(int id)
        {
            foreach(ProductDTO p in products)
            {
                if(p.Id == id)
                {
                    return p;
                }
            }
            return null;
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(int id, ProductDTO entity)
        {
            foreach (ProductDTO p in products)
            {
                if (p.Id == id)
                {
                   p.Name = entity.Name;
                   p.Price = entity.Price;
                   p.Stock = entity.Stock;
                   p.Description = entity.Description;
                    return true;
                }
            }
            return false;

        }

        public async Task<bool> SoftDelete(int id)
        {
            return true;
        }

        Task<List<ProductDTO>> IRepository<ProductDTO, int>.GetAllPaged(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        Task<int> IRepository<ProductDTO, int>.CountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
