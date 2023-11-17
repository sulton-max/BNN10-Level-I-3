namespace N75_C.Models.Entities;

public class User
{
    public Guid Id { get; set; }

    public string EmailAddress { get; set; } = default!;
}