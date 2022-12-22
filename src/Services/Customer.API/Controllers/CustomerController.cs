using AutoMapper;
using Customer.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Customer;

namespace Customer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
   private readonly ICustomerService _service;
   
   public CustomerController(ICustomerService service)
   {
      _service = service;
   }
   
   [HttpGet]
   public async Task<IActionResult> Get()
   {
      return Ok(await _service.GetCustomers());
   }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.GetCustomer(id));
    }

    //[HttpPost]
    //public async Task<IActionResult> Create(CreateCustomerDto customerDto)
    //{
    //    await _service.CreateCustomer(customerDto);
    //    return Ok();
    //}

    //[HttpPut("{id}")]
    //public async Task<IActionResult> Update(int id, UpdateCustomerDto customerDto)
    //{
    //    await _service.UpdateCustomer(id, customerDto);
    //    return Ok();
    //}

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    await _service.DeleteCustomer(id);
    //    return Ok();
    //}
}