using Microsoft.AspNetCore.Mvc;
using ProyectoPrueba.DTOs;
using ProyectoPrueba.Services;
using ProyectoPrueba.UnitOfWork;

namespace ProyectoPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        
        public AuthController(AuthService authService)
        {
            _authService = authService;
           
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login) // Crea este DTO simple con User y Pass
        {
            // 1. Validar credenciales (Aquí iría tu lógica de ir a la BD de usuarios)
            var user = await _authService.isValid(login);
            if (user != null)
           // if (login.Username == "admin" && login.Password == "1234")
            {
                // 2. Si es válido, generamos el token
                var tokenString = _authService.GenerateToken(user);
                return Ok(new { token = tokenString, role = user.Role });
            }

            return Unauthorized("Usuario o contraseña incorrectos");
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] LoginDTO user)
        {

            if(user == null)
            {
                return BadRequest("Falta el usuario o contraseña!");
            }
            var newUser = await _authService.Register(user);

            if(newUser != null)
            {
                return Ok(newUser);
            }
            return BadRequest("El usuario ya se encuentra registrado");
        }

    }
}
