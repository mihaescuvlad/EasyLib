using Application.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Application.Data
{
    public class LibraryContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
        { }

        public required DbSet<Login> Logins { get; set; }
        public required DbSet<Address> Addresses { get; set; }
        public required DbSet<Author> Authors { get; set; }
        public required DbSet<Book> Books { get; set; }
        public required DbSet<BookAuthor> BookAuthors { get; set; }
        public required DbSet<BookStock> BookStocks { get; set; }
        public required DbSet<BorrowHistory> BorrowHistory { get; set; }
        public required DbSet<LibraryLocation> LibraryLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var standardUser = new IdentityRole("standard")
            {
                NormalizedName = "standard",
            };

            var librarian = new IdentityRole("librarian")
            {
                NormalizedName = "librarian",
            };

            builder.Entity<IdentityRole>().HasData(standardUser, librarian);
        }
    }
}
