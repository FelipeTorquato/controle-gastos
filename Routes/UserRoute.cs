using Controle_Gastos.Data;
using Controle_Gastos.Models;
using Controle_Gastos.Models.TO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controle_Gastos.Routes;

/// <summary>
/// User routes.
/// </summary>
public static class UserRoute
{
    public static void UserRoutes(this WebApplication app)
    {
        // Group of routes for the user resource.
        var route = app.MapGroup("user");

        // Paginated GetAll users.
        route.MapGet("", async (
            AppDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10) =>
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var totalItems = await context.Users.CountAsync();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var users = await context.Users
                .Include(u => u.Transactions)
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Results.Ok(new PageResponseTo<User>(
                users,
                page,
                totalPages,
                totalItems,
                pageSize
            ));
        });

        // Route to create a new user.
        route.MapPost("", async (UserTo req, AppDbContext context) =>
        {
            // Verify if the username already exists.
            var user = context.Users.FirstOrDefault(x => x.Name == req.Name);
            if (user != null)
                return Results.BadRequest("The name" + user.Name + " already exists.");

            // Instantiate a new user. Based on the user's age, the user role will be set.
            var newUser = new User(req.Name, req.Age,
                req.Age >= 18 ? UserRole.Adult : UserRole.Minor);
            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();
            return Results.Created();
        });

        // Route to get a user by id. If the user doesn't exist, a 404 will be returned.
        route.MapGet("{id:int}", async (int id, AppDbContext context) =>
        {
            var user = await context.Users.Include(u => u.Transactions)
                .FirstOrDefaultAsync(x => x.Id == id);
            return user == null ? Results.NotFound() : Results.Ok(user);
        });

        // Route to delete a user by id. If the user doesn't exist, a 404 will be returned.
        route.MapDelete("{id:int}", async (int id, AppDbContext context) =>
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return Results.NotFound();
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return Results.NoContent();
        });

        // Route to get the summary of all users. It calculates the total expense,
        // total revenue and net balance of each user. At the end, it returns the overall summary.
        route.MapGet("summary", (AppDbContext context) =>
        {
            var userSummaries = context.Users.Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Age,
                    TotalExpense = u.Transactions.Where(t => t.Type == TransactionType.Expense)
                        .Sum(t => t.Amount),
                    TotalRevenue = u.Transactions.Where(t => t.Type == TransactionType.Revenue)
                        .Sum(t => t.Amount)
                }).AsEnumerable()
                .Select(u => new UserSummaryTo(
                    u.Id,
                    u.Name,
                    u.Age,
                    u.TotalExpense,
                    u.TotalRevenue,
                    u.TotalRevenue - u.TotalExpense
                )).ToList();

            var totalExpense = userSummaries.Sum(u => u.TotalExpense);
            var totalRevenue = userSummaries.Sum(u => u.TotalRevenue);
            var totalNetRevenue = totalRevenue - totalExpense;

            return Results.Ok(new SummaryResponseTo(
                userSummaries,
                totalExpense,
                totalRevenue,
                totalNetRevenue));
        });
    }
}