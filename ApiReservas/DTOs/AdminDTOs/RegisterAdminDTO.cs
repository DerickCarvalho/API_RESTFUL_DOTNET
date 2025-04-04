using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.AuthDTOs
{
    public class RegisterAdminDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
    }
}
