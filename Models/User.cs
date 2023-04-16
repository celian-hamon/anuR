namespace anuR.Models;

public class User : BaseEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string Password { get; set; }
    public String Email { get; set; }
    public List<Service> Services { get; set; }
}