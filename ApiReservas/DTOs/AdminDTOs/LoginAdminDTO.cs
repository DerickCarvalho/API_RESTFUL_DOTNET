using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.AuthDTOs
{
    public class LoginAdminDTO
    {
        [Required]
        public string Email {  get; set; }
        [Required]
        public string Password {  get; set; }
    }
}
