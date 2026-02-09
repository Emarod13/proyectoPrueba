
using ProyectoPrueba.Entity;
using ProyectoPrueba.Repository;

namespace ProyectoPrueba.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // Aquí guardamos la instancia del repositorio
        public IRepository<Product,int> Products { get; private set; }

        public IUsersRepository Users { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            // Inicializamos el repositorio pasándole el contexto actual
            // (Nota: Tendrás que modificar el constructor de tu ProductRepository para que acepte el contexto)
            Products = new DbRepository(_context);
            Users = new DbUsersRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            // Aquí se ejecuta la transacción real en la BD
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
