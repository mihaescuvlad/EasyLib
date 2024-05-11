namespace Application.Pocos;

public class LibraryLocationWithAddressPoco
{
    public Guid Id { get; set; }
    public required string Address1 { get; set; }
    public string? Address2 { get; set; }
    public required DateTime OpenTime { get; set; }
    public required DateTime CloseTime { get; set; }
}
