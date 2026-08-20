Console.WriteLine("Hello, World!");

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}

public class BankAccount
{
    public decimal Balance { get; private set; }

    public BankAccount(decimal balance)
    {
        if (balance < 0)
        {
            balance = 0;
        }
        else
        {
            Balance = balance;
        }
    }

    public void Deposit(decimal amount)
    {
        if (amount >= 0)
        {
            Balance += amount;
        }
    }
    public void Withdraw(decimal amount)
    {
        if ((Balance >= amount) && (amount > 0))
        {
            Balance -= amount;
        }
    }
}