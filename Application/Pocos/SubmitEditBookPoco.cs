namespace Application.Pocos;

public class SubmitEditBookPoco
{
    public required BookPoco BookData { get; set; }
    public required Dictionary<string, int> LibraryStocks { get; set; }
}
