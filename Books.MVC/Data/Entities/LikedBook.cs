namespace Books.MVC.Data.Entities
{
    public class LikedBook
    {
        public int BookId { get; set; }
        public Book Book { get; set; } = default!;
        
        public int UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
