using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.ReservationDTOs
{
    public class FilteredReservetionRequisitionDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RoomId { get; set; }
        public DateTime? ReservDate { get; set; }
        public bool? IsActive { get; set; }
    }
}