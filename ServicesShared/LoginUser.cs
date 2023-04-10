namespace ServicesShared;
public class LoginUser
{
    public string AuthToken { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime RefreshTime { get; set; }
    public string CurrentDatabase { get; set; } = string.Empty;
    public List<string> Databases { get; set; } = new();

}
