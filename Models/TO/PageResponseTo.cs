namespace Controle_Gastos.Models;

/// <summary>
/// Represents a page of data.
/// </summary>
/// <param name="Data"></param>
/// <param name="CurrentPage"></param>
/// <param name="TotalPages"></param>
/// <param name="TotalItems"></param>
/// <param name="PageSize"></param>
/// <typeparam name="T"></typeparam>
public record PageResponseTo<T>(
    List<T> Data,
    int CurrentPage,
    int TotalPages,
    int TotalItems,
    int PageSize
    );