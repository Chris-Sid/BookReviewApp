﻿using System.ComponentModel.DataAnnotations;
namespace BookReviewApp.WebUI.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Register as Admin")]
        public bool IsAdmin { get; set; }
    }
}
