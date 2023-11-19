namespace Books.MVC.Models
{
    public class ItemViewModel<T> where T : class
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; } = default!;
    }
}
