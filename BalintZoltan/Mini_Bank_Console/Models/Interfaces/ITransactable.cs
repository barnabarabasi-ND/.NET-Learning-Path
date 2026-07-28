namespace MiniBank.Models.Interfaces;


interface ITransactable
{
    void Deposit(decimal amount);
    bool Withdraw(decimal amount, out string? error);
}