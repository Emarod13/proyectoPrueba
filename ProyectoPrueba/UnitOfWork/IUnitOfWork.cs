using ProyectoPrueba.Entity;
using ProyectoPrueba.Repository;

namespace ProyectoPrueba.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product, int> Products { get; }
        IUsersRepository Users { get; }
        Task<int> CompleteAsync();
    }
}
