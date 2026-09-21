using InsuranceApp.Application.Common;

namespace InsuranceApp.Application.Exceptions;

/// <summary>
/// Exception thrown when attempting to create an entity that violates a uniqueness constraint.
/// </summary>
public class DuplicateEntityException(string entityName) : Exception(ExceptionMessages.DuplicateEntity(entityName))
{
    public string EntityName { get; } = entityName;
}
