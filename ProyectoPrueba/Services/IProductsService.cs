using ProyectoPrueba.DTOs;
using ProyectoPrueba.Wrappers;
using System.Collections.Generic;

namespace ProyectoPrueba.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductDTO>?> GetAll();

        Task<ProductDTO?> Get(int id);

        Task<ProductDTO?> Add(ProductDTO product);

        Task<bool> Update(int id, ProductDTO product);

        Task<bool> Delete(int id);

        Task<bool> SoftDelete(int id);

        Task<PagedResponse<ProductDTO>> GetAllPaged(int pageNumber, int pageSize);


    }
}
