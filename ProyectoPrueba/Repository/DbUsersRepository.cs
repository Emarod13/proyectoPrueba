
using Microsoft.EntityFrameworkCore;
using ProyectoPrueba.DTOs;
using ProyectoPrueba.Entity;

namespace ProyectoPrueba.Repository
{
    public class DbUsersRepository : IUsersRepository
    {

        public readonly ApplicationDbContext _context;

        public DbUsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User?> Add(User entity)
        {
            if (await _context.Users.FirstOrDefaultAsync(u => u.Username == entity.Username) != null) // ya hay un usuario registrado
            {
                return null;
            }
        
            
            await _context.Users.AddAsync(entity);
          
            return entity;
        }

        public async Task<User?> isValid(User entity) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == entity.Username);

            if (user == null)
            {
                return null;
            }

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(entity.Password, user.Password);

            return isPasswordCorrect ? user : null;
        }

        public async Task<bool> Delete(User entity)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == entity.Username);
            if (user != null)
            {
                _context.Users.Remove(user);
                return true;
            }
            return false;
        }



        public async Task<bool> Update(User entity)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == entity.Username && u.Password == entity.Password);
            if (user != null)
            {
                user.Username = entity.Username;
                user.Password = entity.Password;
                return true;
            }
            return false;
        }
    }
}
