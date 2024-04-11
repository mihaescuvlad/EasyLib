namespace Application.Models
{
    public class BorrowHistory
    {
        public Guid UserId { get; set; }
        public int BookIsbn { get; set; }
        public TimeSpan BorrowDate { get; set; }
    }
}
