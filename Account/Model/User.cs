namespace Account.Model;
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public bool Active { get; set; }
    public string? LoginToken { get; set; }
 
}
