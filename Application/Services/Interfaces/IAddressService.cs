using Application.Models;

namespace Application.Services.Interfaces;

public interface IAddressService
{
    void CreateAddress(Address address);
    void UpdateAddress(Address address);
}
