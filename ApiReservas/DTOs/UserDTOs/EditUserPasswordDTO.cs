using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.UserDTOs
{
    public class EditUserPasswordDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
