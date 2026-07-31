using MiniBankConsole.Models;
using MiniBankConsole.Services;

var MainMenu = new Menu();
var AccountMenu = new Menu();
var mainChoice = 0;

MainMenu.AddCaption("MiniBank Console");
MainMenu.AddOption("List accounts");
MainMenu.AddOption("Create account");
MainMenu.AddOption("Deposit");
MainMenu.AddOption("Withdraw");
MainMenu.AddOption("View statement");
MainMenu.AddOption("Run month-end");
MainMenu.AddOption("Exit");

AccountMenu.AddCaption("ADD ACCOUNT");
AccountMenu.AddOption("Checking Account");
AccountMenu.AddOption("Savings Account");
AccountMenu.AddOption("Loan Account");
AccountMenu.AddOption("Back to main menu");

AccountRegistry accountRegistry = new AccountRegistry();

while (MainMenuOptions.Exit != (MainMenuOptions)mainChoice)
{
    MainMenu.DisplayMenu();
    mainChoice = MainMenu.ReadChoice();
    MainMenuOptions MainMenuSelected = (MainMenuOptions)mainChoice;
    switch (MainMenuSelected)
    {
        case MainMenuOptions.ListAccounts:
            accountRegistry.ListAccount();
            break;

        case MainMenuOptions.CreateAccount:

            accountRegistry.CreateAccount(AccountMenu);
            break;

        case MainMenuOptions.Deposit:
            accountRegistry.Deposit();
            break;

        case MainMenuOptions.Withdraw:
            accountRegistry.Withdraw();
            break;

        case MainMenuOptions.ViewStatement:
            accountRegistry.ViewStatement();
            break;

        case MainMenuOptions.RunMonthEnd:
            accountRegistry.RunMonthEnd();
            break;

        case MainMenuOptions.Exit:
            Console.WriteLine("Exiting...");
            break;

        default:
            Console.WriteLine("Invalid choice. Please try again.");
            Console.ReadLine();
            break;
    }
}

