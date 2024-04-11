namespace Application.Models
{
    public class Book
    {
        public int Isbn { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public byte[]? CoverPicture { get; set; }
    }
}
