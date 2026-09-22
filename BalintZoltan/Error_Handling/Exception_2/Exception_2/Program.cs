int Balance = 100;

Console.WriteLine("Hello, World!");

try
{
    Withdraw(200);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}










void Withdraw(decimal amount)
{
    if (amount > Balance)
    {
        //throw new Exception("Insufficient funds.");
        throw new InsufficientFundsException();
    }
}

public class InsufficientFundsException : Exception
{
    //public InsufficientFundsException()
    //{
    //}
    public InsufficientFundsException()
        : base("Insufficient funds.")
    {
    }
    public InsufficientFundsException(
    string message,
    Exception innerException)
    : base(message, innerException)
    {
    }
}