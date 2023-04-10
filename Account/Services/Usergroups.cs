using Account.Model;
using Microsoft.EntityFrameworkCore;
using ServicesShared;
using System.Dynamic;

namespace Account.Services;
public partial class AccountServices
{
    public async Task<dynamic> LoadUsergroupsAsync(ServicesShared.AuthTokenRequest request, CancellationToken ct)
    {
        dynamic response = new ExpandoObject();
        response.Status = -1;
        response.Message = Messages.LoginToUserDbFailed;
        if (IsLoggedIn(request.AuthToken) == false) return response;
        if (currentUser == null) return response;

        response.IdValuePairs = await context
        .Usergroups
        .Select(x => new { Id = x.Id, Value = x.Name })
        .ToListAsync(ct);
        response.Status = 0;
        response.Message = Messages.OK;
        return response;
    }
    public async Task<dynamic> LoadFunctionsAsync(ServicesShared.AuthTokenRequest request, CancellationToken ct)
    {
        dynamic response = new ExpandoObject();
        response.Status = -1;
        response.Message = Messages.LoginToUserDbFailed;
        if (IsLoggedIn(request.AuthToken) == false) return response;
        response.CheckListItems = await context
        .Functions
        .Select(x => new { Id = x.Id, Value = x.Name })
        .ToListAsync(ct);
        response.Status = 0;
        response.Message = Messages.OK;
        return response;
    }
    public async Task<dynamic> LoadUsergroupFunctionsAsync(SimpleRequest request, CancellationToken ct)
    {
        dynamic response = new ExpandoObject();
        response.Status = -1;
        response.Message = Messages.LoginToUserDbFailed;

        try
        {
            if (IsLoggedIn(request.AuthToken) == false) return response;
            var q1 = await context.Functions.ToListAsync(ct);

            var q2 = await context.UsergroupFunctions
                    .Where(x => x.UsergroupId == request.IntField)
                    .Select(x => x.FunctionId)
                    .ToListAsync(ct);
            response.CheckListItems = q1
                .Select(x => new { id = x.Id, Value = x.Name, isChecked = q2.Contains(x.Id) })
                .ToList();
            response.Status = 0;
            response.Message = Messages.OK;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.StackTrace);
        }
        return response;

    }
    public async Task<dynamic> LoadUsergroupUsersAsync(SimpleRequest request, CancellationToken ct)
    {
        dynamic response = new ExpandoObject();
        response.Status = -1;
        response.Message = Messages.LoginToUserDbFailed;
        try
        {
            if (IsLoggedIn(request.AuthToken) == false) return response;
            var q2 = context.UserUsergroups
                .Where(x => x.UsergroupId == request.IntField)
                .Select(x => x.UserId);
            response.CheckListItems = await context.Users
            .Where(x => q2.Contains(x.Id))
            .Select(x => new { Id = x.Id, Value = x.LastName + " " + x.FirstName })
            .OrderBy(x => x.Value)
            .ToListAsync();

            response.Status = 0;
            response.Message = Messages.OK;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.StackTrace);
        }
        return response;
    }
    public async Task<dynamic> LoadUsersNotInUsergroupAsync(SimpleRequest request, CancellationToken ct)
    {
        dynamic response = new ExpandoObject();
        response.Status = -1;
        response.Message = Messages.LoginToUserDbFailed;
        try
        {
            if (IsLoggedIn(request.AuthToken) == false) return response;
            if (request.StringField==null 
                || request.StringField.Length<3
                ) return response;

            var q2 = context.UserUsergroups
                .Where(x => x.UsergroupId == request.IntField)
                .Select(x => x.UserId);

            response.CheckListItems= await context.Users
                .Where(x => x.LastName.StartsWith(request.StringField)
                && q2.Contains(x.Id) == false)
                .Select(x => new { Id = x.Id, Value = x.LastName + " " + x.FirstName })
                .OrderBy(x => x.Value)
                .ToListAsync();
            response.Status = 0;
            response.Message = Messages.OK;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.StackTrace);
        }
        return response;
    }

}
