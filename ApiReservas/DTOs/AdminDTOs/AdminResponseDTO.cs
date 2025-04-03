using ApiReservas.Models;

namespace ApiReservas.DTOs.AuthDTOs
{
    public class AdminResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Birthdate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsAdmin { get; }

        public AdminResponseDTO(Admin user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Birthdate = user.Birthdate.ToString("yyyy-MM-dd");
            CreatedDate = user.CreatedDate;
            IsAdmin = user.IsAdmin;
        }
    }
}
