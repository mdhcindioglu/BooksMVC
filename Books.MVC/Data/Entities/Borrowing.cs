namespace Books.MVC.Data.Entities
{
    public class Borrowing
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; } = default!;

        public int UserId { get; set; }
        public User User { get; set; } = default!;

        public DateTime RentDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }

        public string? ReturnNote { get; set; }
    }
}
