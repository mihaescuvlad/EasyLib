using Application.Models;

namespace Application.Pocos;

public class EditBookPoco
{
    public required BookPoco BookData;
    public required Dictionary<LibraryLocationWithAddressPoco, BookStock> LibraryStocks { get; set; }
}