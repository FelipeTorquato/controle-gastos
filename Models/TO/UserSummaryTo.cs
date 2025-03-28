namespace Controle_Gastos.Models.TO;

public record UserSummaryTo(
    int UserId,
    string Name,
    int Age,
    double TotalExpense,
    double TotalRevenue,
    double NetBalance);