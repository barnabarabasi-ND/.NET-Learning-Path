using System.ComponentModel.DataAnnotations;

namespace MiniStoreDemo.Application.DTOs;

public class CreateProductDto
{
    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = null!;

    [Required]
    [MaxLength(1000)]
    public string? ProductDescription { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal ProductPrice { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    public bool IsActive { get; set; }
}