namespace InsuranceApp.Application.DTOs.Client;

public sealed record UpdateClientDto(
    string Name,
    string? Email,
    string? Phone,
    string? Address
);