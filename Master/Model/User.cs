using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Model;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public bool Active { get; set; }
    public string? LoginToken { get; set; }


}
