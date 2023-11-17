using Microsoft.AspNetCore.Identity;

namespace Books.MVC.Data.Entities;
public class User : IdentityUser<int>
{
    public string FullName { get; set; } = default!;
    public string? Image { get; set; }
}
