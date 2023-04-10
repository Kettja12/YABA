using System.Collections.Generic;
using Client.Responses.Master;
using Client.Responses.Account;

namespace Client.Services
{
    public class StateContainer
    {
        public LoginUser LoginUser { get; private set; } = new();
        public UserWithRights CurrentUser { get; private set; } = new();

        public event Action? OnChange;

        public void SetLoginUser(LoginUser user)
        {
            this.LoginUser = user;
            NotifyStateChanged();
        }
        public void SetCurrentUser(UserWithRights user)
        {
            this.CurrentUser = user;
            NotifyStateChanged();
        }

        public bool IsAuthenticated { get { return LoginUser.AuthToken != "";} }
        public bool IsLogin { get { return CurrentUser.Id>0; } }

        public void ResetLogin()
        {
            this.LoginUser = new();
            this.CurrentUser = new();
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
