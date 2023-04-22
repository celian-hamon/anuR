using anuR.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace anuR.Views.App;

public class Index : PageModel
{
    public List<Site> Sites { get; set; }
    public List<Service> Services { get; set; }
    public List<User> Users { get; set; }
}