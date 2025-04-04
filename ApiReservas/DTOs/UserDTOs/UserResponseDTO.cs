using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.UserDTOs
{
    public class UserResponseDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool isActive { get; set; }
    }
}
