using System.ComponentModel.DataAnnotations;
using ApiReservas.Models;

namespace ApiReservas.DTOs.AuthDTOs
{
    public class AdminResponseDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Birthdate { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
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
