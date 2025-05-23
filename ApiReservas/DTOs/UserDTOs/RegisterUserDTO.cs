﻿using System.ComponentModel.DataAnnotations;

namespace ApiReservas.DTOs.UserDTOs
{
    public class RegisterUserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool isActive { get; set; }
    }
}
