using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Basket;
public class CartItemDto
{
    [Required]
    [Range(1, double.PositiveInfinity, ErrorMessage = "The field {0} must be >= {1}.")]
    public int Quantity { get; set; }
    [Required]
    [Range(0.1, double.PositiveInfinity, ErrorMessage = "The field {0} must be >= {1}.")]
    public decimal ItemPrice { get; set; }
    [Required]
    public string ItemNo { get; set; }
    [Required]
    public string ItemName { get; set; }

    public int AvailableQuantity { get; set; }

    public void SetAvailableQuantity(int quantity)
    {
        AvailableQuantity = quantity;
    }
}
