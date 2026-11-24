using Deliverable_2_StudentBookExchangeMVC.Hubs;
using Deliverable_2_StudentBookExchangeMVC.Models;
using Deliverable_2_StudentBookExchangeMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Deliverable_2_StudentBookExchangeMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _books;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHubContext<BookHub> _hub;

        public BooksController(
            IBookRepository books,
            UserManager<IdentityUser> userManager,
            IHubContext<BookHub> hub)
        {
            _books = books;
            _userManager = userManager;
            _hub = hub;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _books.GetAllAsync());
        }

        // AJAX GET endpoint used by the jQuery live-search box.
        [HttpGet]
        public async Task<IActionResult> Search(string? term)
        {
            var results = await _books.SearchAsync(term);
            return PartialView("_BookCards", results);
        }

        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> Mine()
        {
            if (User.IsInRole("Admin"))
                return View("Index", await _books.GetAllAsync());

            var userId = _userManager.GetUserId(User);
            return View("Index", userId == null
                ? Enumerable.Empty<Book>()
                : await _books.GetBySellerAsync(userId));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var book = await _books.GetByIdAsync(id.Value);
            return book == null ? NotFound() : View(book);
        }

        [Authorize(Roles = "Student,Admin")]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize(Roles = "Student,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid) return View(book);

            book.SellerId = _userManager.GetUserId(User);
            book.IsAvailable = true;
            book.Id = await _books.AddAsync(book);

            await _hub.Clients.All.SendAsync("BookChanged", "A new book listing was added.");
            TempData["Success"] = "Your book has been listed successfully.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Student,Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var book = await _books.GetByIdAsync(id.Value);
            if (book == null) return NotFound();
            if (!await CanManageBook(book)) return Forbid();
            return View(book);
        }

        [Authorize(Roles = "Student,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id) return NotFound();
            var existing = await _books.GetByIdAsync(id);
            if (existing == null) return NotFound();
            if (!await CanManageBook(existing)) return Forbid();
            if (!ModelState.IsValid) return View(book);

            book.SellerId = existing.SellerId;
            await _books.UpdateAsync(book);
            await _hub.Clients.All.SendAsync("BookChanged", "A book listing was updated.");
            TempData["Success"] = "Book listing updated.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Student,Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var book = await _books.GetByIdAsync(id.Value);
            if (book == null) return NotFound();
            if (!await CanManageBook(book)) return Forbid();
            return View(book);
        }

        [Authorize(Roles = "Student,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _books.GetByIdAsync(id);
            if (book == null) return NotFound();
            if (!await CanManageBook(book)) return Forbid();

            await _books.DeleteAsync(id);
            await _hub.Clients.All.SendAsync("BookChanged", "A book listing was removed.");
            TempData["Success"] = "Book listing deleted.";
            return RedirectToAction(nameof(Index));
        }


        // AJAX POST endpoint. Anti-forgery validation is required for this state-changing request.
        [Authorize(Roles = "Student,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var book = await _books.GetByIdAsync(id);
            if (book == null) return NotFound(new { success = false, message = "Book not found." });
            if (!await CanManageBook(book)) return Forbid();

            await _books.DeleteAsync(id);
            await _hub.Clients.All.SendAsync("BookChanged", "A book listing was removed.");
            return Json(new { success = true, message = "Book listing deleted." });
        }

        private async Task<bool> CanManageBook(Book book)
        {
            if (User.IsInRole("Admin")) return true;
            var currentUserId = _userManager.GetUserId(User);
            return !string.IsNullOrEmpty(currentUserId) && book.SellerId == currentUserId;
        }
    }
}
