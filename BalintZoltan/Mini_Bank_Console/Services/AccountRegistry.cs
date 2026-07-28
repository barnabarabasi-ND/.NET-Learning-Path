using MiniBank.Models.Interfaces;
using MiniBankConsole.Models;
namespace MiniBankConsole.Services;

public class AccountRegistry
{
    private List<BankAccount> _accountList = new List<BankAccount>();

    public void ListAccount()
    {
        if (!Empty_accountList())
        {
            Console.WriteLine("Listing accounts...");
            foreach (var account in _accountList)
            {
                Console.WriteLine($"ID: {account.Id}, Owner: {account.Owner}, Type: {account.Type}, Balance: {account.Balance}");
            }
            Console.ReadLine();
        }
    }

    public void CreateAccount(Menu AccountMenu)
    {
        //Create account (choose type + owner name + initial deposit or loan amount).
        int acountChoice = 0;
        while (AccountMenuOptions.BackToMainMenu != (AccountMenuOptions)acountChoice)
        {
            AccountMenu.DisplayMenu();
            acountChoice = AccountMenu.ReadChoice();
            AccountMenuOptions accountMenuSelected = (AccountMenuOptions)acountChoice;
            switch (accountMenuSelected)
            {
                case AccountMenuOptions.CheckingAccount:
                    AddNewAccount(new CheckingAccount());
                    break;

                case AccountMenuOptions.SavingsAccount:
                    AddNewAccount(new SavingsAccount());
                    break;

                case AccountMenuOptions.LoanAccount:
                    AddNewAccount(new LoanAccount());
                    break;

                case AccountMenuOptions.BackToMainMenu:
                    Console.WriteLine("Returning to main menu...");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    public void Deposit()
    {
        if (!Empty_accountList())
        {
            Console.Write("Enter the account number:");
            int accountId = int.TryParse(Console.ReadLine(), out int result) ? result : 0;
            var account = _accountList.FirstOrDefault(d => d.Id == accountId);
            if (account != null)
            {
                Console.Write("Enter the amount:");
                account.Deposit(ReadAmount());
            }
            else
            {
                Console.WriteLine("Account not found.");
                Console.ReadLine();
            }
        }
    }

    public void Withdraw()
    {
        if (!Empty_accountList())
        {
            Console.Write("Enter the account number:");
            int accountId = int.TryParse(Console.ReadLine(), out int result) ? result : 0;
            var account = _accountList.FirstOrDefault(d => d.Id == accountId);
            if (account != null)
            {
                Console.Write("Enter the amount:");
                string error = String.Empty;
                if (account.Withdraw(ReadAmount(), out error) == false)
                {
                    Console.WriteLine(error);
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Account not found.");
                Console.ReadLine();
            }
        }
    }

    public void ViewStatement()
    {
        if (!Empty_accountList())
        {
            Console.Write("Enter the account number:");
            int accountId = int.TryParse(Console.ReadLine(), out int result) ? result : 0;
            var account = _accountList.FirstOrDefault(d => d.Id == accountId);
            if (account != null)
            {
                Console.WriteLine("Account statement:");
                account.PrintStatement();
            }
            else
            {
                Console.WriteLine("Account not found.");
                Console.ReadLine();
            }
        }
    }

    public void RunMonthEnd()
    {
        if (!Empty_accountList())
        {
            foreach (var account in _accountList)
            {
                if (account is IInterestBearing interestBearing)
                {
                    interestBearing.ApplyMonthlyInterest();
                }
            }
        }
    }

    private void AddNewAccount(BankAccount account)
    {
        account.Id = _accountList.Count + 1;        // GUID
        account.Owner = ReadOwnerName();
        switch (account.Type)
        {
            case AccountType.Checking:
                Console.Write("Enter the initial deposit for checking account: ");
                break;
            case AccountType.Savings:
                Console.Write("Enter the initial deposit for savings account: ");
                break;
            case AccountType.Loan:
                Console.Write("Enter the initial loan amount: ");
                break;
        }
        var amount = ReadAmount();
        if (account.Type == AccountType.Loan)
        {
            account.Balance = amount * (-1);
            account.Transactions.Add($"Loan: {account.Balance}");
        }
        else
        {
            account.Deposit(amount);
        }

        _accountList.Add(account);
    }

    private decimal ReadAmount()
    {
        decimal amount = 0m;
        while (true)
        {
            if (decimal.TryParse(Console.ReadLine(), out amount) && (amount >= 0))
                return amount;
            Console.Write("Invalid amount. Please enter a number:");
        }
    }

    private string ReadOwnerName()
    {
        string? ownerName;

        do
        {
            Console.Write("Enter the account owner's name: ");
            ownerName = Console.ReadLine();
        }
        while (string.IsNullOrWhiteSpace(ownerName));

        return ownerName;
    }

    private bool Empty_accountList()
    {
        if (_accountList.Count == 0)
        {
            Console.WriteLine("No accounts found.");
            Console.ReadLine();
            return true;
        }
        return false;
    }
}