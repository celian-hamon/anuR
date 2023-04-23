using anuR.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace anuR.Views.Admin;

public class Index : PageModel
{
    public void OnGet()
    {
        
    }

    public List<Site>? Sites { get; set; }
    public List<User>? Users { get; set; }
    public List<Service>? Services { get; set; }
}