
using Microsoft.EntityFrameworkCore;
using ProyectoPrueba.Entity;

namespace ProyectoPrueba.Repository
{
    public class DbRepository : IRepository<Product,int>
    {
        public readonly ApplicationDbContext _context;
   
        public DbRepository(ApplicationDbContext context) {
            _context = context;
     
        }    

        public async Task<Product?> Add(Product entity)
        {
            if(entity != null)
            {
                entity.IsDeleted = false;
         
                await _context.Products.AddAsync(entity);
            }
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var product = await GetByID(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                return true;

            }
            return false;
        }

        public async Task<bool> SoftDelete(int id)
        {
            var product = await GetByID(id);
            if (product != null)
            {
                product.IsDeleted = true;
                product.DeleteTime = DateTime.Now;
                return true;

            }
            return false;
        }

        public async Task<IEnumerable<Product>?> GetAll()
        {
            var products = await _context.Products
                                 .Where(p => p.IsDeleted == false) // o simplemente (!p.IsDeleted)
                                 .ToListAsync();

            return products;
        }

        public async Task<Product?> GetByID(int id)
        {
            
            var product= await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);

            if (product != null)
            {
                return product;
            }

            return null;
        }
        public async Task<bool> Update(int id, Product entity)
        {
            var oldEntity = await GetByID(id);
            if (oldEntity != null)
            {
                oldEntity.Price = entity.Price;
                oldEntity.Name = entity.Name;
                oldEntity.Stock = entity.Stock;
                oldEntity.Description = entity.Description;
                return true;
            }
            return false;
        }

        public async Task<List<Product>>GetAllPaged(int pageNumber, int pageSize)
        {
            return await _context.Products
            .Skip((pageNumber - 1) * pageSize) // La fórmula mágica
            .Take(pageSize)
            .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Products.CountAsync();
        }

        
    }
}
