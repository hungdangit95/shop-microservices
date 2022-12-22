using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Customer;

public abstract class CreateOrUpdateCustomerDto
{
    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }
}