using System.ComponentModel.DataAnnotations;

namespace MiniStoreDemo.Domain.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = null!;

    [Required]
    [MaxLength(1000)]
    public string? ProductDescription { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal ProductPrice { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }

    public virtual Category Category { get; set; } = null!;
}
