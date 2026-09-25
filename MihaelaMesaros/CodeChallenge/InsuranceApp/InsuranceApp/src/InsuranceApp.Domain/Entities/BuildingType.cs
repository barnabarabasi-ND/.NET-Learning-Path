namespace InsuranceApp.Domain.Entities;

public sealed class BuildingType
{
    public Guid BuildingTypeId { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Building> Buildings { get; set; } = [];
}
