using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Model.Master;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public bool Active { get; set; }
    public string? LoginToken { get; set; }

    [NotMapped]
    public DateTime RefreshTime { get; set; }
    [NotMapped]
    public string? CurrentDatabase { get; set; }
    
    [NotMapped]
    public List<string>? Databases { get; set; }

}
