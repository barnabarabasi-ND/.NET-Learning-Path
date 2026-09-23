namespace MiniStoreDemo.Application.Exceptions;

/// <summary>
/// Exception thrown when attempting to create an entity that violates a uniqueness constraint.
/// </summary>
public class DuplicateEntityException(string entityName) : Exception($"A duplicate {entityName} was detected.")
{
    public string EntityName { get; } = entityName;
}
