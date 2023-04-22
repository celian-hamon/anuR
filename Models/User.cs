namespace anuR.Models;

public class User : BaseEntity
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public String? Email { get; set; }
    public bool IsAdmin { get; set; }
    public List<Service>? Services { get; set; }
    public List<Site>? Sites { get; set; }
}