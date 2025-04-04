using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.UserDTOs
{
    public class EditUserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
