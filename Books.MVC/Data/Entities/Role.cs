using Microsoft.AspNetCore.Identity;

namespace Books.MVC.Data.Entities;

public class Role : IdentityRole<int>
{
}

public class Roles
{
    public const string Admin = nameof(Admin);
}
