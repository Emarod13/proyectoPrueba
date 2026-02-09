using ProyectoPrueba.DTOs;
using ProyectoPrueba.Entity;

namespace ProyectoPrueba.Services
{
    public interface IAuthService
    {
        public string GenerateToken(User user);

        public Task<User?> isValid(LoginDTO user);

        public Task<User> Register(LoginDTO user);
    }
}
