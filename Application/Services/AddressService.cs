using Application.Models;
using Application.Repositories;
using Application.Repositories.Interfaces;
using Application.Services;
using Application.Services.Interfaces;

namespace Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public AddressService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public void CreateAddress(Address address)
        {
            _repositoryWrapper.AddressRepository.Create(address);
        }
    }
}