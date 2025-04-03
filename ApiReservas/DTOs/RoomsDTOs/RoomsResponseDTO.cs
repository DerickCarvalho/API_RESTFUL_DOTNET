namespace ApiReservas.DTOs.RoomsDTOs
{
    public class RoomsResponseDTO
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int CapacityInHours { get; set; }
        public bool IsActive { get; set; }
        public int PeopleCapacity { get; set; }
    }
}
