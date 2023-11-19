using Microsoft.AspNetCore.Identity;

namespace Books.MVC.Data.Entities;
public class User : IdentityUser<int>
{
    public string FullName { get; set; } = default!;
    public string? Image { get; set; }
   
    public ICollection<Role> Roles { get; set; } = default!;
    public ICollection<Book> BorrowingBooks { get; set; } = default!;
    public ICollection<Book> LikedBooks { get; set; } = default!;
}
