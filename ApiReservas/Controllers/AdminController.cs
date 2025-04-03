using System.Security.Cryptography;
using System.Text;
using ApiReservas.Data;
using ApiReservas.DTOs.AuthDTOs;
using ApiReservas.DTOs.UserDTOs;
using ApiReservas.Middleware;
using ApiReservas.Models;
using ApiReservas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiReservas.Controllers
{
    [ApiController]
    [Route("api/admins")]
    public class AdminController : ControllerBase
    {
        #region CONFIGURATIONS
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;
        private readonly EncPassword _encPassword;

        public AdminController(ApplicationDbContext context, JwtService jwtService, EncPassword encPassword)
        {
            _context = context;
            _jwtService = jwtService;
            _encPassword = encPassword;
        }
        #endregion


        [HttpPost("cadastrar")]
        public async Task<IActionResult> Register(RegisterAdminDTO adminDto)
        {
            var admin = new Admin
            {
                Name = adminDto.Name,
                Email = adminDto.Email,
                Password = adminDto.Password,
                Birthdate = adminDto.Birthdate.Date,
                CreatedDate = DateTime.UtcNow,
                IsAdmin = true
            };

            var adminExists = await _context.Admins.FirstOrDefaultAsync(u => u.Email == adminDto.Email);

            if (adminExists != null)
            {
                return Conflict(new { message = $"Cadastro com e-mail {adminDto.Email} já está existe no sistema!" });
            }

            admin.Password = _encPassword.EncriptPassword(adminDto.Password);

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Admin cadastrado com sucesso!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginAdminDTO loginDTO)
        {
            var hashedPassword = _encPassword.EncriptPassword(loginDTO.Password);

            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Email == loginDTO.Email && u.Password == hashedPassword);

            if (admin == null)
            {
                return Unauthorized(new { message = "Informações de Login inválidas!" });
            }
            else
            {
                var token = _jwtService.GenerateToken(admin.Email, admin.IsAdmin);
                return Ok(new { message = "Usuário logado com sucesso!", token });
            }
        }

        [HttpPost("criar_usuario")]
        [Authorize]
        [AdminOnly]
        public async Task<IActionResult> AddUser(RegisterUserDTO userDTO)
        {
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email);
            var userIsAdmin = await _context.Admins.FirstOrDefaultAsync(u => u.Email == userDTO.Email);

            if (userExists != null || userIsAdmin != null)
            {
                return Conflict(new { message = $"Cadastro com e-mail {userDTO.Email} já existe no sistema!" });
            }

            user.Password = _encPassword.EncriptPassword(userDTO.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuário cadastrado com sucesso!" });
        }

        [HttpPut("editar_usuario/{id}")]
        [Authorize]
        [AdminOnly]
        public async Task<IActionResult> EditUser(int id, EditUserDTO userDTO)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado!" });
            }

            user.Name = userDTO.Name;
            user.IsActive = userDTO.IsActive;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuário editado com sucesso!", userDTO});
        }

        [HttpDelete("inativar_usuario/{id}")]
        [Authorize]
        [AdminOnly]
        public async Task<IActionResult> InativeUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado!" });
            }

            user.IsActive = false; ;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuário inativado com sucesso!"});
        }

        [HttpGet("listar_usuarios")]
        [Authorize]
        [AdminOnly]
        public async Task<IActionResult> ListUsers()
        {
            var users = await _context.Users.Select(u => new UserResponseDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                isActive = u.IsActive
            })
            .ToListAsync();

            return Ok(users);
        }

        [HttpGet("listar_usuarios_ativos")]
        [Authorize]
        [AdminOnly]
        public async Task<IActionResult> ListActiveUsers()
        {
            var users = await _context.Users
            .Where(u => u.IsActive)
            .Select(u => new UserResponseDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                isActive = u.IsActive
            })
            .ToListAsync();

            return Ok(users);
        }
    }
}
