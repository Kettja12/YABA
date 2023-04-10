using Account.Model;
using Microsoft.EntityFrameworkCore;
using ServicesShared;
using System.Dynamic;

namespace Account.Services;
public partial class AccountServices
{
    public async Task<dynamic> LoginAsync(AuthTokenRequest request)
    {
        dynamic result = new ExpandoObject();
        result.Status = -1;
        result.Message = Messages.LoginToUserDbFailed;
        result.User = new UserWithRights();
        
        LoginUserResponse? userResponse = await apiService
            .PostAsync<LoginUserResponse>(
            request.AuthToken, "Master/GetLoginUser", request);

        if (userResponse != null)
        {
            if (userResponse.Status != 0)
            {
                result.Status = userResponse.Status;
                return result;
            }
            User? user = await context
              .Users
              .FirstOrDefaultAsync(x => x.Username == userResponse.LoginUser.UserName);
            if (user == null) return result;
            result.User.Id = user.Id;
            result.User.FirstName = user.FirstName;
            result.User.LastName = user.LastName;
            result.User.Username = user.Username;
            var q = context
                    .UserUsergroups
                    .Where(x => x.UserId == user.Id)
                    .Select(x => x.UsergroupId);
            result.User.Rights = await context.UsergroupRights
                .Where(x => q.Contains(x.UsergroupId))
                .Select(x => x.RightId)
                .ToListAsync();
            services.SetCacheItem(CurrentUser, request.AuthToken, result.User);
            result.Status = 0;
            result.Message = Messages.OK;
        }
        return result;
    }
    public dynamic Logout(ServicesShared.AuthTokenRequest request)
    {
        var response = new ExpandoObject();
        var logout = services.DeleteCacheItem(CurrentUser, request.AuthToken);
        return response;
    }
}
