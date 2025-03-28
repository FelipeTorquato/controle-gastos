namespace Controle_Gastos.Models;

public enum TransactionType
{
    /// <summary>
    /// Only Adult users can create revenue transactions.
    /// </summary>
    Revenue,
    
    /// <summary>
    /// All users can create expense transactions.
    /// </summary>
    Expense
}