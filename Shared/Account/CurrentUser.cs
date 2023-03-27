using System.ComponentModel.DataAnnotations.Schema;

namespace YABA.Shared.Account;
public class CurrentUser
{
    required public string AuthToken { get; set; }
    public int Id { get; set; }
    required public string FirstName { get; set; }
    required public string LastName { get; set; }
    required public string Username { get; set; } 
    public bool Active { get; set; }
    public string? DashboardItems { get; set; }
    public List<int> Functions { get; set; } = new();


}
