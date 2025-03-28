namespace Controle_Gastos.Models;

public class Transaction
{
    /// <summary>
    /// Initializes a new instance of the Transaction class.
    /// </summary>
    /// <param name="description"></param>
    /// <param name="amount"></param>
    /// <param name="type"></param>
    /// <param name="userId"></param>
    public Transaction(string description, double amount, TransactionType type, int userId)
    {
        Description = description;
        Amount = amount;
        Type = type;
        UserId = userId;
    }
    
    /// <summary>
    /// Incremental integer as identifier.
    /// </summary>
    public int Id { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }
    
    /// <summary>
    /// Type of transaction. Can be either a revenue or an expense.
    /// </summary>
    public TransactionType Type { get; set; }
    
    /// <summary>
    /// The user's id.
    /// </summary>
    public int UserId { get; set; }
}