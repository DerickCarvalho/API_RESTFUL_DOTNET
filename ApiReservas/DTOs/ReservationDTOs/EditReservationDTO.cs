using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.ReservationDTOs
{
    public class EditReservationDTO
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public int QtdPeoples { get; set; }
        [Required]
        public DateTime StartAt { get; set; }
        [Required]
        public DateTime EndAt { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
