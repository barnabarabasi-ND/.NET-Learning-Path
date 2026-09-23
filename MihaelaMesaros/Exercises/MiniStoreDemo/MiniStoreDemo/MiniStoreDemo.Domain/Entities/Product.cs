namespace MiniStoreDemo.Domain.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string? ProductDescription { get; set; }

    public decimal ProductPrice { get; set; }

    public int CategoryId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Category Category { get; set; } = null!;
}
