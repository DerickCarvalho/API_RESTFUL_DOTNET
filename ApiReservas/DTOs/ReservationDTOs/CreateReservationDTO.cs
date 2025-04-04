using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.ReservationDTOs
{
    public class CreateReservationDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RoomId { get; set; }
        [Required]
        public int QtdPeoples { get; set; }
        [Required]
        public DateTime StartAt { get; set; }
        [Required]
        public DateTime EndAt { get; set; }

    }
}
