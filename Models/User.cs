using System.Text.Json.Serialization;

namespace Controle_Gastos.Models;

public class User
{
    /// <summary>
    /// Initializes a new instance of the User class.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="age"></param>
    /// <param name="role"></param>
    public User(string name, int age, UserRole role)
    {
        Name = name;
        Age = age;
        Role = role;
    }

    /// <summary>
    /// Incremental integer as identifier.
    /// </summary>
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    
    /// <summary>
    /// The user's role. Can be either an adult or a minor.
    /// </summary>
    public UserRole Role { get; }
    
    /// <summary>
    /// The user's transactions.
    /// </summary>
    public List<Transaction> Transactions { get; set; } = new();
}