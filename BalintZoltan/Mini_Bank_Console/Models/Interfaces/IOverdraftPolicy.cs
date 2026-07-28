namespace MiniBank.Models.Interfaces;

interface IOverdraftPolicy
{
    decimal OverdraftLimit { get; }
}
