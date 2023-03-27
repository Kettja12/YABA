namespace Master.Model
{
    public class RefreshResponse
    {
        required public string AuthToken { get; set; }
        public DateTime refreshTime { get; set; }

    }
}
