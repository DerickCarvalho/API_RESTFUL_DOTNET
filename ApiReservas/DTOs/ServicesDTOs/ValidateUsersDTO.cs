using ApiReservas.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.ServicesDTOs
{
    public class ValidateUsersDTO
    {
        [Required]
        public bool IsValidated { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public ResponseType Type { get; set; }
    }
}
