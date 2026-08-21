namespace MiniStoreDemo.DTOs;

public class UpdateProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public decimal ProductPrice { get; set; }

    public int CategoryId { get; set; }

    public bool IsActive { get; set; }
}