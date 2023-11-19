using Microsoft.AspNetCore.Identity;

namespace Books.MVC.Data.Entities;

public class Role : IdentityRole<int>
{
    public ICollection<User> Users { get; set; } = default!;
}

public class Roles
{
    public const string Admin = nameof(Admin);
}
