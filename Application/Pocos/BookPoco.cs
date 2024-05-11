namespace Application.Pocos;

public class BookPoco
{
    public required string Isbn { get; set; }
    public required string Title { get; set; }
    public required string[] Authors { get; set; }
    public required string Description { get; set; }
    public string? Thumbnail { get; set; }
}

