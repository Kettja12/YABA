using Account.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using YABA.Shared;
using YABA.Shared.Account;

namespace Account.Services;

public class AccountServices
{
    private CacheServices services;
    private AccountContext context;
    public AccountServices(CacheServices services, AccountContext context)
    {
        this.services = services;
        this.context = context;

    }
    public async Task<CurrentUser> LoginAsync(LoginRequest request,string authToken)
    {

        var result = new CurrentUser() { AuthToken= "", Username = "", FirstName = "", LastName = "" };
        if (authToken==null 
            ||string.IsNullOrEmpty(request.Username)
            || string.IsNullOrEmpty(request.Password)) return result;
        User? user = await context
          .Users
          .FirstOrDefaultAsync(x => x.Username == request.Username);
        if (user == null) return result;
        string? s = request.Username.ToUpper().Trim() + request.Password.Trim();
        string? loginToken = CreateHash(s);
        if (user.LoginToken != loginToken) return result;
        result.AuthToken = authToken;
        result.FirstName = user.FirstName;
        result.LastName = user.LastName;
        result.Username = user.Username;
        var q = context
                .UserUsergroups
                .Where(x => x.UserId == user.Id)
                .Select(x => x.UsergroupId);
        result.Functions = await context.UsergroupFunctions
            .Where(x => q.Contains(x.UsergroupId))
            .Select(x => x.FunctionId)
            .ToListAsync();

        services.SetCacheItem("currentUser", user.Username, result);
        return result;
    }
    public string CreateHash(string s)
    {
        byte[]? dataArray = Encoding.Unicode.GetBytes(s);
        HashAlgorithm sha = SHA256.Create();
        byte[] result = sha.ComputeHash(dataArray);
        return Encoding.Unicode.GetString(result);
    }
}
