using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Books.MVC.Data
{
    public class MainDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Auther> Authers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<LikedBook> LikedBooks { get; set; }

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Books");

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");

            builder.Entity<Book>().ToTable("Books");
            builder.Entity<Auther>().ToTable("Authers");
            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Borrowing>().ToTable("Borrowings");
            builder.Entity<LikedBook>().ToTable("LikedBooks");

            //builder.Entity<User>().HasMany(u => u.Roles).WithMany(r => r.Users).UsingEntity<UserRole>();
            builder.Entity<User>().HasMany(u => u.BorrowingBooks).WithMany(b => b.BorrowingUsers).UsingEntity<Borrowing>();
            builder.Entity<User>().HasMany(u => u.LikedBooks).WithMany(b => b.LikedUsers).UsingEntity<LikedBook>();

            builder.Entity<Book>().HasOne(b => b.Auther).WithMany(a => a.Books).IsRequired().HasForeignKey(b => b.AutherId);
            builder.Entity<Book>().HasOne(b => b.Category).WithMany(a => a.Books).IsRequired().HasForeignKey(b => b.CategoryId);
        }
    }
}
