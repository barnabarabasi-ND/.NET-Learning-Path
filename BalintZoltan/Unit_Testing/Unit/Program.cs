
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
        Balance = balance;
    }

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }
    public void Withdraw(decimal amount)
    {
        if (Balance >= amount)
        {  
            Balance -= amount;
        }
    }
}

