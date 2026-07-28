using MiniBank.Models.Interfaces;

public class SavingsAccount : BankAccount, IInterestBearing
{
    decimal percent = 1;
    public override AccountType Type => AccountType.Savings;
    public void ApplyMonthlyInterest()
    {
        // percent % of balance if balance > 0).
        if (base.Balance > 0)
        {
            decimal interest = Balance * percent / 100;
            Balance += interest;
            Transactions.Add($"Interest: {interest}");
        }
    }
}