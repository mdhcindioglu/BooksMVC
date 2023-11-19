using AutoMapper;
using AutoMapper.QueryableExtensions;
using Books.MVC.Data.Attributes;
using Books.MVC.Data.Entities;
using Books.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Books.MVC.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class BooksController(ILogger<BooksController> logger, MainDbContext db, IMapper mapper) : Controller
    {
        private readonly ILogger<BooksController> _logger = logger;
        private readonly MainDbContext _db = db;
        private readonly IMapper _mapper = mapper;

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(DataTableAjaxPostModel model)
        {
            var booksQry = _db.Books.Include(x => x.Auther).AsQueryable();
            var recordsTotal = await booksQry.CountAsync();

            if (!string.IsNullOrEmpty(model.search.value))
            {
                booksQry = booksQry.Where(x =>
                x.Title.Contains(model.search.value) ||
                x.Auther.FullName.Contains(model.search.value) ||
                x.Category.Name.Contains(model.search.value));
            }

            var recordsFiltered = await booksQry.CountAsync();

            var bookVMs = booksQry.ProjectTo<BooksVM>(_mapper.ConfigurationProvider);

            if (model.order != null)
                bookVMs = bookVMs.OrderBy(string.Join(",", model.order.Select(x => $"{model.columns[x.column].data} {x.dir}")));

            var books = await bookVMs.Skip(model.start).Take(model.length).ToListAsync();

            return Json(new
            {
                model.draw,
                recordsTotal,
                recordsFiltered,
                data = books,
            });
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            var model = new BookVM();
            if (id != 0)
            {
                model = await _db.Books.Include(x => x.Auther).Include(x => x.Category).ProjectTo<BookVM>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == id);
                if (model == null)
                    return RedirectToAction(nameof(Index));
            }

            model.Authers = await _db.Authers.OrderBy(x => x.FullName).ToListAsync();
            model.Categories = await _db.Categories.OrderBy(x => x.Name).ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEdit(int id, [Bind($"{nameof(BookVM.Id)},{nameof(BookVM.Title)},{nameof(BookVM.AutherId)},{nameof(BookVM.CategoryId)},{nameof(BookVM.Pages)},{nameof(BookVM.PublishDate)},{nameof(BookVM.AddedDate)},{nameof(BookVM.Image)},{nameof(BookVM.PDF)}")] BookVM model)
        {
            if (ModelState.IsValid)
            {
                if (await _db.Books.AnyAsync(x => x.Title == model.Title && x.Id != model.Id))
                {
                    ModelState.AddModelError(nameof(BookVM.Title), "This book title added before.");
                    model.Authers = await _db.Authers.OrderBy(x => x.FullName).ToListAsync();
                    model.Categories = await _db.Categories.OrderBy(x => x.Name).ToListAsync();
                    return Json(new
                    {
                        isSuccess = false,
                        html = HtmlHelpers.RenderRazorViewToString(this, "AddOrEdit", model),
                    }); ;
                }

                var book = _mapper.Map<Book>(model);

                if (model.Id == 0)
                    await _db.Books.AddAsync(book);
                else
                    _db.Books.Update(book);

                await _db.SaveChangesAsync();
                return Json(new { isSuccess = true, });
            }

            model.Authers = await _db.Authers.OrderBy(x => x.FullName).ToListAsync();
            model.Categories = await _db.Categories.OrderBy(x => x.Name).ToListAsync();
            return Json(new
            {
                isSuccess = false,
                html = HtmlHelpers.RenderRazorViewToString(this, "AddOrEdit", model),
            });
        }

        [HttpDelete()]
        [Route("/Books/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bookInDb = await _db.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (bookInDb != null)
            {
                _db.Books.Remove(bookInDb);
                await _db.SaveChangesAsync();
            }
            return Json(new { isSuccess = true, });
        }
    }
}
