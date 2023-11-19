using AutoMapper;

namespace Books.MVC.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Book, BooksVM>()
            .AfterMap((book, bookVM) => bookVM.AutherFullname = book.Auther.FullName)
            .AfterMap((book, bookVM) => bookVM.CategoryName = book.Category.Name);
        CreateMap<BooksVM, Book>();
        CreateMap<Book, BookVM>();
        CreateMap<BookVM, Book>();
    }
}
