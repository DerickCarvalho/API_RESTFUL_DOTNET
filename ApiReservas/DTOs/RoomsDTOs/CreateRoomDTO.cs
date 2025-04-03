using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.RoomsDTOs
{
    public class CreateRoomDTO
    {
        public string RoomName { get; set; }
        public int CapacityInHours { get; set; }
        public int PeopleCapacity { get; set; }
    }
}
