using Master.Model;
using Microsoft.EntityFrameworkCore;
using ServicesShared;
using System.Dynamic;

namespace Master.Services;
public partial class MasterServices
{
    public async Task<dynamic> SavePasswordAsync(SavePasswordRequest request, CancellationToken ct)
    {
        dynamic response = new ExpandoObject();
        response.Status = -1;
        response.Message = Messages.PasswordSaveFail;
        var user = services.GetCacheItem<ServicesShared.LoginUser>("LoginUser", request.AuthToken);
        if (user == null)
            return response;

        Model.LoginUser? existingUser = await context.LoginUsers
          .Where(x => x.Username == user.UserName)
          .FirstOrDefaultAsync(ct);

        if (existingUser != null)
        {
            if (string.IsNullOrEmpty(request.OldPassword))
            {
                response.Message = Messages.NoData;
                return response;

            }
            var s = existingUser.Username.ToUpper().Trim() + request.OldPassword.Trim();
            string? testloginToken = CreateHash(s);
            if (testloginToken != existingUser.LoginToken)
            {
                response.Message = Messages.InvalidOldPassword;
                return response;
            }
            string? newLoginToken = CreateHash(existingUser.Username.ToUpper() + request.NewPassword);
            existingUser.LoginToken = newLoginToken;
            var isSaved = await context.SaveChangesAsync();
            if (isSaved == 1)
            {
                response.Status = 0;
                response.Message = Messages.PasswordSaveSuccess;
            }
        }
        return response;
    }

}