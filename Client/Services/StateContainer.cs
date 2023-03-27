using System.Collections.Generic;
using YABA.Shared.Account;
using YABA.Shared.Master;

namespace Client.Services
{
    public class StateContainer
    {
        public LoginUser? LoginUser { get; set; }
        public CurrentUser? CurrentUser { get; set; }

    }
}
