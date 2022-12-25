using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Basket;
public class BasketCheckoutDto
{
    public string Username { get; set; }
    public decimal TotalPrice { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string ShippingAddress { get; set; }
    public string _invoiceAddress;

    public string? InvoiceAddress
    {
        get => _invoiceAddress;
        set => _invoiceAddress = value ?? ShippingAddress;
    }
}
