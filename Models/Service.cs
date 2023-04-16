namespace anuR.Models;

public class Service : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<User>? Users { get; set; }
    public Site? Site { get; set; }
}