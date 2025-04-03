namespace ApiReservas.DTOs.RoomsDTOs
{
    public class EditRoomDTO
    {
        public string RoomName { get; set; }
        public int CapacityInHours { get; set; }
        public bool IsActive { get; set; }
        public int PeopleCapacity { get; set; }
    }
}
