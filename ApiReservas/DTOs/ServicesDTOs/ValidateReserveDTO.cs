using System.ComponentModel.DataAnnotations;
using ApiReservas.Enums;

namespace ApiReservas.DTOs.ServicesDTOs
{
    public class ValidateReserveDTO
    {
        [Required]
        public bool IsDisponible { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public ResponseType Type { get; set; }

    }
}
