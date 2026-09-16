namespace InsuranceApp.Application.Common;

public static class ClientErrors
{
    public static readonly Error InvalidClientId = new(
        "Client.InvalidClientId",
        "Client ID must be greater than zero.",
        ErrorType.Validation);

    public static readonly Error NameRequired = new(
        "Client.NameRequired",
        "Client name is required.",
        ErrorType.Validation);

    public static readonly Error InvalidNameLength = new(
        "Client.InvalidNameLength",
        "Client name must be between 3 and 200 characters.",
        ErrorType.Validation);

    public static readonly Error IdentificationNumberRequired = new(
        "Client.IdentificationNumberRequired",
        "Identification number is required.",
        ErrorType.Validation);

    public static readonly Error InvalidIdentificationNumberLength = new(
        "Client.InvalidIdentificationNumberLength",
        "Identification number must be between 3 and 50 characters.",
        ErrorType.Validation);

    public static readonly Error InvalidPhoneLength = new(
        "Client.InvalidPhoneLength",
        "Phone number must be less than 50 characters.",
        ErrorType.Validation);

    public static readonly Error InvalidAddressLength = new(
        "Client.InvalidAddressLength",
        "Address must be less than 300 characters.",
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


    public static readonly Error InvalidPageNumber = new(
        "Client.InvalidPageNumber",
        "Page number must be greater than zero.",
        ErrorType.Validation);

    public static readonly Error InvalidPageSize = new(
        "Client.InvalidPageSize",
        "Page size must be between 1 and 100.",
        ErrorType.Validation);
}