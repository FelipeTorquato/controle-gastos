using Controle_Gastos.Data;
using Controle_Gastos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controle_Gastos.Routes;

/// <summary>
/// Transaction routes.
/// </summary>
public static class TransactionRoute
{
    public static void TransactionRoutes(this WebApplication app)
    {
        // Group of routes for the transaction resource.
        var route = app.MapGroup("transaction");

        // Paginated GetAll transactions.
        route.MapGet("", async (
            AppDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10) =>
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var totalItems = await context.Transactions.CountAsync();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var transactions = await context.Transactions
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Results.Ok(new PageResponseTo<Transaction>(
                transactions,
                page,
                totalPages,
                totalItems,
                pageSize
            ));
        });

        // Route to create a new transaction. It verifies if the user exists and
        // if the user can create a revenue transaction.
        route.MapPost("", async (TransactionTo req, AppDbContext context) =>
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == req.UserId);
            if (user == null)
                return Results.BadRequest("User not found.");

            VerifyIfUserCanRevenue(user.Role.ToString(), req.Type.ToString());

            var transaction = new Transaction(req.Description, req.Amount, req.Type, user.Id);
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            return Results.Created();
        });

        // Route to get a transaction by id. If the transaction doesn't exist, a 404 will be returned.
        route.MapGet("{id:int}", async (int id, AppDbContext context) =>
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            return transaction == null ? Results.NotFound() : Results.Ok(transaction);
        });
    }

    // Method that verifies if the user can create a revenue transaction based on the user's role.
    private static void VerifyIfUserCanRevenue(string userRole, string transactionType)
    {
        if (userRole.Equals("Minor") && transactionType.Equals("Revenue"))
        {
            throw new BadHttpRequestException(
                "You can't create a revenue transaction for a minor.");
        }
    }
}