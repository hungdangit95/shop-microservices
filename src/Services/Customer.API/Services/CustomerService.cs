using AutoMapper;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Shared.Dtos.Customer;

namespace Customer.API.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;
    public CustomerService(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CustomerDto>> GetCustomers()
    {
        return _mapper.Map<IEnumerable<CustomerDto>>(await _repository.GetCustomers());
    }

    public async Task<CustomerDto> GetCustomer(int id)
    {
        return _mapper.Map<CustomerDto>(await _repository.GetCustomer(id));
    }

    //public async Task CreateCustomer(CreateCustomerDto customerDto)
    //{
    //    var customer = _mapper.Map<Entities.Customer>(customerDto);
    //    await _repository.CreateCustomer(customer);
    //    await _repository.SaveChangesAsync();
    //}

    //public async Task UpdateCustomer(int id, UpdateCustomerDto customerDto)
    //{
    //    var customer = await _repository.GetByIdAsync(id);
    //    if (customer == null)
    //        throw new Exception("Customer Not Found !");
    //    await _repository.UpdateCustomer(_mapper.Map(customerDto, customer));
    //    await _repository.SaveChangesAsync();
    //}

    //public async Task DeleteCustomer(int id)
    //{
    //    await _repository.DeleteCustomer(id);
    //    await _repository.SaveChangesAsync();
    //}
}