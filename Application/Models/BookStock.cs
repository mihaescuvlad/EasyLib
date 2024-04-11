namespace Application.Models
{
    public class BookStock
    {
        public int BookIsbn { get; set; }
        public Guid LibraryId { get; set; }
        public int Stock { get; set; }
    }
}
