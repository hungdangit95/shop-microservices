using Shared.Dtos.Customer;
using AutoMapper;
using Infrastructure.Mappings;

namespace Customer.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Entities.Customer, CustomerDto>();
        CreateMap<CreateCustomerDto, Entities.Customer>();
        CreateMap<UpdateCustomerDto, Entities.Customer>().IgnoreAllNonExisting();
    }
}