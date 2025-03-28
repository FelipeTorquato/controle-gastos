namespace Controle_Gastos.Models;

public record TransactionTo(
    string Description,
    double Amount,
    TransactionType Type,
    int UserId);