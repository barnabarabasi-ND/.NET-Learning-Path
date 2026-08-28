using System.ComponentModel.DataAnnotations;

namespace MiniStoreDemo.Application.DTOs;

public class PatchProductDto
{
    [MaxLength(200)]
    public string? ProductName { get; set; }

    [MaxLength(500)]
    public string? ProductDescription { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? ProductPrice { get; set; }

    [Range(1, int.MaxValue)]
    public int? CategoryId { get; set; }

    public bool? IsActive { get; set; }
}
