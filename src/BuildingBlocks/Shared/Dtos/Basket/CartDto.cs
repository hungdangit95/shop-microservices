using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Basket;
public class CartDto
{
    public string Username { get; set; }
    public string EmailAddress { get; set; }

    public List<CartItemDto> Items { get; set; } = new();

    public CartDto()
    {

    }

    public CartDto(string username)
    {
        Username = username;
    }

    public decimal TotalPrice => Items.Sum(item => item.ItemPrice * item.Quantity);
}
