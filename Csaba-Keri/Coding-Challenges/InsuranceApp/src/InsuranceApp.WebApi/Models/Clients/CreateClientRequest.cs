using System.ComponentModel.DataAnnotations;

namespace InsuranceApp.WebApi.Models.Clients;

public class CreateClientRequest
{
    [Required]
    public ClientTypeDto? Type { get; init; }

    [Required]
    public string? IdentificationNumber { get; init; }

    [Required]
    public string? Name { get; init; }

    [Required]
    public string? Email { get; init; }

    [Required]
    public string? Phone { get; init; }

    public string? PrimaryAddress { get; init; }
}
