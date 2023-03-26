using System.Collections.Generic;

namespace Client.Services
{
    public class StateContainer
    {
        required public string AuthToken { get; set; }
        public List<string>? Databases { get; set; }

    }
}
