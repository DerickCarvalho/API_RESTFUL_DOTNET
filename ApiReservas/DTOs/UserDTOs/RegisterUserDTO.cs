﻿namespace ApiReservas.DTOs.UserDTOs
{
    public class RegisterUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isActive { get; set; }
    }
}
