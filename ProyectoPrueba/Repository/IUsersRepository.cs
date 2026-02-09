using ProyectoPrueba.Entity;

namespace ProyectoPrueba.Repository
{
    public interface IUsersRepository
    {
        public Task<User> Add(User entity);
        public Task<User?> isValid(User entity);

        public Task<bool> Delete(User entity);

        public Task<bool> Update(User entity);

    }
}
