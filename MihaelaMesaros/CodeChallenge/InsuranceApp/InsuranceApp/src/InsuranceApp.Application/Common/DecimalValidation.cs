namespace InsuranceApp.Application.Common;

public static class DecimalValidation
{
    public static bool HasValidScale(decimal value, int scale)
    {
        return decimal.Round(value, scale) == value;
    }
}
