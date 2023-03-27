namespace Master.Model
{
    public class CurrentUser
    {
        required public string AuthToken { get; set; }
        required public string UserName { get; set; }
        public string? Database { get; set; }
    }
}
