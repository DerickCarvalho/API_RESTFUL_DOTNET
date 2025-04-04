using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.RoomsDTOs
{
    public class CreateRoomDTO
    {
        [Required]
        public string RoomName { get; set; }
        [Required]
        public int CapacityInHours { get; set; }
        [Required]
        public int PeopleCapacity { get; set; }
    }
}
