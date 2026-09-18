namespace InsuranceApp.Application.DTOs.Client;

public sealed record ClientSearchDto(string? Name, string? Identifier, int PageNumber = 1, int PageSize = 50);