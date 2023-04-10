using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Model;
public class LoginUser
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public bool Active { get; set; }
    public string? LoginToken { get; set; }


}
