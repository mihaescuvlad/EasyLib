namespace Application.Models
{
    public class LibraryLocation
    {
        public Guid Id { get; set; }
        public Guid AddressId { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
    }
}
