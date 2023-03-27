using System.ComponentModel.DataAnnotations.Schema;

namespace YABA.Shared.Master
{
    public class LoginUser
    {
        required public string AuthToken { get; set; }
        required public string UserName { get; set; }
        public DateTime RefreshTime { get; set; }
        public string? CurrentDatabase { get; set; }
        public List<string>? Databases { get; set; }

    }
}
