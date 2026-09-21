namespace InsuranceApp.Application.Common;

public static class ExceptionMessages
{
    public static string DuplicateEntity(string entityName) =>
        $"A duplicate {entityName} has been detected.";
}