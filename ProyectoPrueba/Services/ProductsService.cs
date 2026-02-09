using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using ProyectoPrueba.DTOs;
using ProyectoPrueba.Entity;
using ProyectoPrueba.UnitOfWork;
using ProyectoPrueba.Wrappers;

namespace ProyectoPrueba.Services
{
    public class ProductsService : IProductsService
    {
        public readonly IUnitOfWork _unitOfWork;

        public readonly IMapper _mapper;

        public readonly IMemoryCache _cache;
        public ProductsService( IMapper mapper, IUnitOfWork unitOfWork, IMemoryCache cache) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ProductDTO?> Add(ProductDTO p) {

            var entity = _mapper.Map<Product>(p);

            var newProd = _mapper.Map<ProductDTO>(await _unitOfWork.Products.Add(entity));

            await _unitOfWork.CompleteAsync();

           
            return newProd;
        }

        public async Task<bool> Delete(int id)
        {
            var deleted = await _unitOfWork.Products.Delete(id);
            if (deleted == false)
            {
                return false;
            }
            
            await _unitOfWork.CompleteAsync();
            _cache.Remove($"producto_{id}");
            return true;
        }


        public async Task<ProductDTO?> Get(int id) 
        {
            // 1. Definimos la clave ÚNICA para ESTE producto
            string cacheKey = $"producto_{id}";

            // 2. Verificamos si ya lo tenemos en memoria
            if (_cache.TryGetValue(cacheKey, out ProductDTO productCached))
            {
                // ¡Bingo! Lo tenemos en RAM
                return productCached;
            }

            // 3. No está en caché, vamos a la base de datos
            var product = await _unitOfWork.Products.GetByID(id);

            // Si no existe, retornamos null (y no guardamos nada en caché)
            if (product == null) return null;

            var productDto = _mapper.Map<ProductDTO>(product);

            // 4. Guardamos en caché
            // Aquí puedes darle un tiempo de vida distinto si quieres (ej: 10 mins)
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            _cache.Set(cacheKey, productDto, cacheOptions);

            return productDto;
        }
        

        public async Task<IEnumerable<ProductDTO>?> GetAll()
        {

            string cacheKey = $"lista_productos_todos";

                if (_cache.TryGetValue(cacheKey, out IEnumerable<ProductDTO> productsCached))
                {
                    Console.WriteLine("--> Obteniendo lista completa desde Caché (RAM)");
                    return productsCached;
            }
            var products = await _unitOfWork.Products.GetAll();

            var CacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            _cache.Set(cacheKey, _mapper.Map<IEnumerable<ProductDTO>>(products), CacheOptions);

            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<bool> Update(int id, ProductDTO product)
        {
            var entity = _mapper.Map<Product>(product);
            var updated = await _unitOfWork.Products.Update(id, entity);
            if (updated == false)
            {
                return false;
            }
           
            await _unitOfWork.CompleteAsync();
            _cache.Remove($"producto_{id}"); // Limpiamos el caché de este producto, para que la próxima vez se actualice
            return true;
        }

        public async Task<bool> SoftDelete(int id) 
        {
            var deleted = await _unitOfWork.Products.SoftDelete(id);
            if (deleted == false)
            {
                return false;
            }
            
            await _unitOfWork.CompleteAsync();
            _cache.Remove($"producto_{id}");
            return true;

        }

        public async Task<PagedResponse<ProductDTO>> GetAllPaged(int pageNumber, int pageSize)
        {
            // 1. Definimos una CLAVE ÚNICA para esta búsqueda.
            // OJO: La clave debe incluir la página y el tamaño, si no, mezclarás datos.
            string cacheKey = $"lista_productos_{pageNumber}_{pageSize}";

            // 2. Preguntamos: ¿Existe este dato en memoria?
            if (_cache.TryGetValue(cacheKey, out PagedResponse<ProductDTO> cachedResponse))
            {
                // ¡SÍ ESTÁ! Devolvemos lo que había en RAM (súper rápido) 🚀
                Console.WriteLine("--> Obteniendo datos desde Caché (RAM)");
                return cachedResponse;
            }

            // 3. NO ESTÁ EN CACHÉ. Toca ir a la Base de Datos (más lento) 🐢
            Console.WriteLine("--> Obteniendo datos desde Base de Datos");

            var productos = await _unitOfWork.Products.GetAllPaged(pageNumber, pageSize);
            var totalRecords = await _unitOfWork.Products.CountAsync();
            var productosDto = _mapper.Map<List<ProductDTO>>(productos);

            var response = new PagedResponse<ProductDTO>(productosDto, pageNumber, pageSize, totalRecords);

            // 4. Guardamos el resultado en RAM para la próxima vez
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)) // Vive 5 minutos
                .SetSlidingExpiration(TimeSpan.FromMinutes(1)); // Si no se usa en 1 min, muere

            _cache.Set(cacheKey, response, cacheOptions);

            return response;
        }
    }
}
