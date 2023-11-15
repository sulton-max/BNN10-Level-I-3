namespace N74_C.Models;

public class User
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } = default;
    
    public string LastName { get; set; } = default;

    // public string PasswordHash { get; set; } = default;
}