using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Product;

public class CreateProductDto : CreateOrUpdateProductDto
{
    [Required]
    public string No { get; set; }
}