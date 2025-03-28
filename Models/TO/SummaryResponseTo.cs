using Controle_Gastos.Models.TO;

namespace Controle_Gastos.Models;

public record SummaryResponseTo(
    List<UserSummaryTo> UserSummaries,
    double TotalFamilyExpense,
    double TotalFamilyRevenue,
    double TotalFamilyNetBalance);