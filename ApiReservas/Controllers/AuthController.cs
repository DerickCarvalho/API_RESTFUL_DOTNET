using System.Security.Cryptography;
using System.Text;
using ApiReservas.Data;
using ApiReservas.DTOs.AuthDTOs;
using ApiReservas.Models;
using ApiReservas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiReservas.Controllers
{
    [ApiController]
    [Route("api/autenticacao")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Register(RegisterUserDTO userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                Birthdate = userDto.Birthdate.Date,
                CreatedDate = DateTime.UtcNow
            };

            var userExist = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (userExist != null)
            {
                return Conflict(new { message = $"Email {userDto.Email} já está cadastrado no sistema!" });
            }

            using var sha256 = SHA256.Create();
            user.Password = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password)));

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuário cadastrado com sucesso!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO loginDTO)
        {
            using var sha256 = SHA256.Create();
            var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password)));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email && u.Password == hashedPassword);

            if (user == null)
            {
                return Unauthorized(new { message = "Informações de Login inválidas!" });
            }
            else
            {
                var token = _jwtService.GenerateToken(user.Email);
                return Ok(new { message = "Usuário logado com sucesso!", token });
            }
        }
    }
}
