using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiReservas.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string RoomName { get; set; }
        [Required]
        public int PeopleCapacity { get; set; }
        [Required]
        public bool isActive { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
