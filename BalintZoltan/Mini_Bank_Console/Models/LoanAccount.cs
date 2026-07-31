using MiniBank.Models.Interfaces;

public class LoanAccount : BankAccount, IInterestBearing
{
    decimal percent = 1;
    public override AccountType Type => AccountType.Loan;
    public void ApplyMonthlyInterest()
    {
        if (Balance < 0)
        {
            decimal interest = Balance * percent / 100;
            Balance += interest;
            Transactions.Add($"Interest: {interest}");
        }
    }
}