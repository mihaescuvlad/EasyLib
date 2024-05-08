using Application.Models;

namespace Application.Pocos;

public class EditBookPoco
{
    public required BookPoco BookData { get; set; }
    public required Dictionary<LibraryLocationWithAddressPoco, int> LibraryStocks { get; set; }
}