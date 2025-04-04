using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.RoomsDTOs
{
    public class EditRoomDTO
    {
        [Required]
        public string RoomName { get; set; }
        [Required]
        public int CapacityInHours { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public int PeopleCapacity { get; set; }
    }
}
