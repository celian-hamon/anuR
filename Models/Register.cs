using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace anuR.Models;

[BindProperties]
public class Register
{
    [Required] public string? FirstName { get; set; }

    [Required] public string? LastName { get; set; }

    [Required] [Phone] public string? PhoneNumber { get; set; }

    [Required] [Phone] public string? LandLine { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string? Email { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [Display(Name = "Email Address")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}