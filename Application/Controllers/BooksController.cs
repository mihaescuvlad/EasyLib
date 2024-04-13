using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.Data;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Application.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        [HttpGet("{Isbn}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Book>> GetBookByIsbn(string Isbn)
        {
            var book = await _context.Books.FindAsync(Isbn);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isbn,Title,Description")] Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Book");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. ");
            }

            return View(book);
        }

        [HttpPut("{Isbn}")]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Isbn, [Bind("Isbn,Title,Description")] Book book)
        {
            if (Isbn != book.Isbn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Book");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. ");
                }
            }

            return View(book);
        }

        [HttpDelete("{Isbn}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string Isbn)
        {
            var book = await _context.Books.FindAsync(Isbn);
            if (book == null)
            {
                return RedirectToAction("Book");
            }

            try
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("Book");
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { Isbn = Isbn, saveChangesError = true });
            }
        }
    }
}
