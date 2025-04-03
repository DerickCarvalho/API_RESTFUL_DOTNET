namespace ApiReservas.DTOs.AuthDTOs
{
    public class RegisterUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
