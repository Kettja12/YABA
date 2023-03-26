namespace Master.Model
{
    public class MasterLoginResponse
    {
        required public string AuthToken { get; set; }
        public List<string>? Databases { get; set; }

    }
}
