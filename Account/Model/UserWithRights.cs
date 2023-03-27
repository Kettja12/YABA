namespace Account.Model
{
    public class UserWithRights
    {
        required public string Username { get; set; }
        required public string Firstname { get; set; }
        required public string Lastname { get; set; }
        public List<int> Functions { get; set; } = new();

    }
}
