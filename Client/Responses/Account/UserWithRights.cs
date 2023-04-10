using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Responses.Account;
public class UserWithRights
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty; 
    public List<int> Functions { get; set; } = new();


}
