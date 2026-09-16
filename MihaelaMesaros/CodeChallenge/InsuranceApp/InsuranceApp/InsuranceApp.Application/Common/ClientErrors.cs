namespace InsuranceApp.Application.Common;

public static class ClientErrors
{
    public static readonly Error NameRequired = new(
        "Client.NameRequired",
        "Client name is required.",
        ErrorType.Validation);

    public static readonly Error IdentificationNumberRequired = new(
        "Client.IdentificationNumberRequired",
        "Identification number is required.",
        ErrorType.Validation);
    
    public static readonly Error InvalidClientType = new(
        "Client.InvalidClientType",
        "Client type is invalid.",
        ErrorType.Validation);

    public static readonly Error InvalidEmail = new(
        "Client.InvalidEmail",
        "Email address is invalid.",
        ErrorType.Validation);

    public static readonly Error DuplicateIdentificationNumber = new(
        "Client.DuplicateIdentificationNumber",
        "A client with this identification number already exists.",
        ErrorType.Conflict);

    public static Error NotFound(int clientId) => new(
        "Client.NotFound",
        $"Client with ID {clientId} was not found.",
        ErrorType.NotFound);

}