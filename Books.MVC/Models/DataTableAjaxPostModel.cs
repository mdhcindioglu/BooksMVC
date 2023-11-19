namespace Books.MVC.Models;

public class DataTableAjaxPostModel
{
    public int draw { get; set; }
    public int start { get; set; }
    public int length { get; set; }
    public List<Column> columns { get; set; } = default!;
    public Search search { get; set; } = default!;
    public List<Order> order { get; set; } = default!;
}

public class Column
{
    public string data { get; set; } = default!;
    public string name { get; set; } = default!;
    public bool searchable { get; set; }
    public bool orderable { get; set; }
    public Search search { get; set; } = default!;
}

public class Search
{
    public string value { get; set; } = default!;
    public string regex { get; set; } = default!;
}

public class Order
{
    public int column { get; set; }
    public string dir { get; set; } = default!;
}
