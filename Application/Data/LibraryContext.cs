using Application.Models;

using Microsoft.EntityFrameworkCore;
namespace Application.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookStock> BookStocks { get; set; }
        public DbSet<BorrowHistory> BorrowHistory { get; set; }
        public DbSet<LibraryLocation> LibraryLocations { get; set; }
    }
}
