using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace Books.MVC.Data.ViewModels;

public class BooksVM
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;
    public int Pages { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    public string? Image { get; set; }
    public string? PDF { get; set; }

    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public Category Category { get; set; } = default!;

    public int AutherId { get; set; }
    public string? AutherFullname { get; set; }
    public Auther Auther { get; set; } = default!;

    public List<User> LikedUsers { get; set; } = default!;
    public List<User> BorrowingUsers { get; set; } = default!;
}

public class BookVM
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = default!;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The {0} is required.")]
    public int Pages { get; set; }
    
    [Required]
    public DateTime? PublishDate { get; set; }
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "Book Image")]
    public string? Image { get; set; }
    
    [Display(Name = "Book PDF")]
    public string? PDF { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public Category? Category { get; set; } 

    [Required]
    public int AutherId { get; set; }
    public string? AutherFullname { get; set; }
    public Auther? Auther { get; set; } 

    public List<Auther>? Authers { get; set; } 
    public List<Category>? Categories { get; set; } 
    public List<User>? LikedUsers { get; set; } 
    public List<User>? BorrowingUsers { get; set; } 
}
