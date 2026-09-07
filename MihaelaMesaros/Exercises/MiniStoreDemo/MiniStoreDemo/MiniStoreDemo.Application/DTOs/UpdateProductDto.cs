using System.ComponentModel.DataAnnotations;

namespace MiniStoreDemo.Application.DTOs;

public class UpdateProductDto
{
    [Required]
    public int ProductId { get; set; } = 0;

    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string ProductDescription { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal ProductPrice { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    public bool IsActive { get; set; }
}