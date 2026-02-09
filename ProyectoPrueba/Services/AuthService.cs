using Microsoft.IdentityModel.Tokens;
using ProyectoPrueba.DTOs;
using ProyectoPrueba.Entity;
using ProyectoPrueba.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProyectoPrueba.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;

        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _config = config;
            _unitOfWork = unitOfWork;
        }

        // Este método genera el string del token
        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Los "Claims" son datos que van dentro del token (ej: id, rol, email)
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Role, user.Role) 
        };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60), // Expira en 1 hora
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> isValid(LoginDTO user)
        {
            var entity = new User();
            entity.Username = user.Username;
            entity.Password = user.Password;

            return await _unitOfWork.Users.isValid(entity);
        }

        public async Task<User?> Register(LoginDTO user)
        {
            var entity = new User();
            entity.Username = user.Username;
            entity.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            entity.Role = "User";
            var newUser = await _unitOfWork.Users.Add(entity);
            if(newUser != null)
            {
                await _unitOfWork.CompleteAsync();
                return newUser;
            }
            return null;
        }
    }
}
