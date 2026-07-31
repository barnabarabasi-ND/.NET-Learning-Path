using MiniBank.Models.Interfaces;
public abstract class BankAccount : ITransactable, IStatement  // abstract ???
{
    public int Id { get; set; } = 0;
    public string Owner { get; set; } = string.Empty;
    public decimal Balance { get; set; } = 0;
    public abstract AccountType Type { get; }
    public List<string> Transactions { get; set; } = new();

    public virtual void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Deposit amount must be positive.");
        }
        else
        {
            Balance += amount;
            Transactions.Add($"Deposit: {amount}");
        }
    }

    public virtual bool Withdraw(decimal amount, out string? error)
    {
        if (Balance - amount < 0)
        {
            error = "Credit limit reached!";
            return false;
        }
        else
        {
            error = "No error.";
            Balance -= amount;
            Transactions.Add($"Withdraw: {amount}");
            return true;
        }
    }
    public void PrintStatement()
    {
        foreach (var t in Transactions)
        {
            Console.WriteLine(t);
        }
        Console.ReadLine();
    }
}
