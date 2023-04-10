using Account.Model;
using Microsoft.EntityFrameworkCore;
using ServicesShared;
using System.Dynamic;

namespace Account.Services;
public partial class AccountServices
{
    public async Task<dynamic> SaveUserAsync(SaveUserRequest request, CancellationToken ct)
    {
        dynamic response = new ExpandoObject();
        response.Status = -1;
        response.Message = Messages.UserSaveFailed;
        if (IsLoggedIn(request.AuthToken) == false) return response;
        if (request.User == null) return response;

        if (request.User.Id != 0)
        {
            response = await ModifyUserAsync(response, request, ct);
        }
        else
        {
            response = await InsertUserAsync(response, request, ct);
        }
        return response;

    }
    private async Task<dynamic> ModifyUserAsync(dynamic response, SaveUserRequest request, CancellationToken ct)
    {
        User? existingUser = await context.Users
          .Where(x => x.Id == request.User.Id)
          .FirstOrDefaultAsync(ct);

        if (existingUser == null)
        {
            response.Message = Messages.UserNotFoundDB;
            return response;
        }
        bool modifyCurrentUser = existingUser.Id == currentUser?.Id;

        if (modifyCurrentUser==false
            && currentUser?.Functions.Contains(AppCodes.AddModifyUser) == false)
        {
            response.Message = Messages.NoPermissionToModify;
            return response;
        }

        existingUser.FirstName = request.User.FirstName;
        existingUser.LastName = request.User.LastName;
        if (context.ChangeTracker.HasChanges())
        {
            var result = await context.SaveChangesAsync();
            if (result == 1)
            {

                response.User = request.User;
                if (modifyCurrentUser)
                {
                    services.SetCacheItem(CurrentUser, request.AuthToken, response.User);
                }
                response.Message = Messages.UserSaveSuccess;
                response.Status = 0;
            }
            else
            {
                response.Message = "No Fields To Save";
            }
        }
        return response;
    }

    private async Task<dynamic> InsertUserAsync(dynamic response, SaveUserRequest request, CancellationToken ct)
    {
        if (currentUser?.Functions.Contains(AppCodes.AddModifyUser) == false)
        {
            response.Message = Messages.NoPermissionToInsert;
            return response;
        }

        if (string.IsNullOrEmpty(request.User.Username))
        {
            response.Message = Messages.UsernameEmpty;
            return response;
        }
        User? user = await context.Users
            .Where(x => x.Username == request.User.Username)
            .FirstOrDefaultAsync();
        if (user != null)
        {
            response.Message = Messages.UsernameAlreadyInUse;
            return response;
        }
        user = new()
        {
            Username = request.User.Username,
            FirstName = request.User.FirstName,
            LastName = request.User.LastName,
        };
        context.Add(user);
        var result = await context.SaveChangesAsync();
        if (result == 1)
        {
            request.User.Id = user.Id;
            response.User = request.User;
            response.Message = Messages.SaveSuccess;
            response.Status = 0;
        }
        return response;

    }

    public class SaveUserRequest : AuthTokenRequest
    {
        public UserWithRights User { get; set; } = new();
    }

}
