namespace anuR.Models;

public class Site : BaseEntity
{
   public int Id { get; set; }
   public String City { get; set; }
   public List<User>? Users { get; set; }
}