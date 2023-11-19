namespace Books.MVC.Data.Entities
{
    public class Auther
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
   
        public ICollection<Book> Books { get; set; } = default!;
    }
}
