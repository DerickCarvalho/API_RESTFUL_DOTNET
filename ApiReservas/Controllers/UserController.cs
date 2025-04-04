using ApiReservas.Data;
using ApiReservas.DTOs.UserDTOs;
using ApiReservas.Middlewares;
using ApiReservas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiReservas.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UserController : ControllerBase
    {
        #region CONFIGURATIONS
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;
        private readonly EncPassword _encPassword;

        public UserController(ApplicationDbContext context, JwtService jwtService, EncPassword encPassword)
        {
            _context = context;
            _jwtService = jwtService;
            _encPassword = encPassword;
        }
        #endregion

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO loginDTO)
        {
            var hashedPassword = _encPassword.EncriptPassword(loginDTO.Password);

            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email && u.Password == hashedPassword);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Informações de login inválidas!" });
            }
            else
            {
                var token = _jwtService.GenerateToken(loginDTO.Email, false);
                return Ok(new { message = "Usuário logado com sucesso!", token });
            }
        }

        [HttpPut("mudar_senha")]
        [UserOnly]
        public async Task<IActionResult> ModifyPassword(EditUserPasswordDTO loginDTO)
        {
            var hashedPassword = _encPassword.EncriptPassword(loginDTO.Password);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email && u.Password == hashedPassword);

            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado!" });
            }

            user.Password = _encPassword.EncriptPassword(loginDTO.NewPassword);

            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Senha alterada com sucesso!" });
        }
    }
}
