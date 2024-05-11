namespace Application.Models;

public class LibraryLocation
{
    public Guid Id { get; set; }
    public required Guid AddressId { get; set; }
    public required DateTime OpenTime { get; set; }
    public required DateTime CloseTime { get; set; }
}
