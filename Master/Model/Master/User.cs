namespace Master.Model.Master;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public bool Active { get; set; }
    public string? LoginToken { get; set; }
}
