using ApiReservas.Data;
using ApiReservas.DTOs.ServicesDTOs;
using Microsoft.EntityFrameworkCore;

namespace ApiReservas.Services
{
    public class ValidateUser
    {
        #region CONFIGURATIONS
        private readonly ApplicationDbContext _context;

        public ValidateUser
        (
            ApplicationDbContext context
        )
        {
            _context = context;
        }
        #endregion

        public async Task<ValidateUsersDTO> Validate (int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id && u.IsActive)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new ValidateUsersDTO
                {
                    IsValidated = false,
                    Message = "Usuário inativo!",
                    Type = Enums.ResponseType.BadRequest
                };
            }

            return new ValidateUsersDTO
            {
                IsValidated = true,
                Message = "Usuário ativo!",
                Type = Enums.ResponseType.Ok
            };
        }
    }
}
