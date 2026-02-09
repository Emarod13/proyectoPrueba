namespace ProyectoPrueba.Repository
{
    public interface IRepository<T,TI>
    {
        public Task<IEnumerable<T>?> GetAll();

        public Task<T?> GetByID(TI id);

        public Task<T?> Add(T entity);

        public Task<bool> Update(TI id, T entity);

        public Task<bool> Delete(TI id);

        public Task<bool> SoftDelete(TI id);

        public Task<List<T>> GetAllPaged(int pageNumber, int pageSize);

        public Task<int> CountAsync();


    }
}
