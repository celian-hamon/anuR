using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace anuR.Models;

public class User : BaseEntity
{
    public Guid Id { get; set; }

    [Required] public string? FirstName { get; set; }

    [Required] public string? LastName { get; set; }

    [Required] [Phone] public string? PhoneNumber { get; set; }
    [Required] [Phone] public string? LandLine { get; set; }
    public string? Password { get; set; }
    [Required] public String? Email { get; set; }
    public bool IsAdmin { get; set; }
    public List<Service>? Services { get; set; }
    public List<Site>? Sites { get; set; }
}