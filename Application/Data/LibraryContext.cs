using Application.Models;

using Microsoft.EntityFrameworkCore;
namespace Application.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
        { }

        public required DbSet<User> Users { get; set; }
        public required DbSet<Login> Logins { get; set; }
        public required DbSet<Role> Roles { get; set; }
        public required DbSet<Address> Addresses { get; set; }
        public required DbSet<Author> Authors { get; set; }
        public required DbSet<Book> Books { get; set; }
        public required DbSet<BookAuthor> BookAuthors { get; set; }
        public required DbSet<BookStock> BookStocks { get; set; }
        public required DbSet<BorrowHistory> BorrowHistory { get; set; }
        public required DbSet<LibraryLocation> LibraryLocations { get; set; }
    }
}
