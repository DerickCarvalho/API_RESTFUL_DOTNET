using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.UserDTOs
{
    public class LoginUserDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
