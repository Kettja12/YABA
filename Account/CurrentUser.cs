namespace Account;
public class UserWithRights
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty; 
    public List<int> Rights { get; set; } = new();


}
