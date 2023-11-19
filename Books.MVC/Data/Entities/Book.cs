namespace Books.MVC.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = default!;
        public int Pages { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public string? Image { get; set; }
        public string? PDF { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        
        public int AutherId { get; set; }
        public Auther Auther { get; set; } = default!;

        public ICollection<User> LikedUsers { get; set; } = default!;
        public ICollection<User> BorrowingUsers { get; set; } = default!;
    }
}
