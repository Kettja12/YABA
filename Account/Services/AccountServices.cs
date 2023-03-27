using Account.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

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
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var result = new LoginResponse() { Username = "", Firstname = "", Lastname = "" };
        if (string.IsNullOrEmpty(request.Username)
      || string.IsNullOrEmpty(request.Password)) return result;
        User? user = await context
          .Users
          .FirstOrDefaultAsync(x => x.Username == request.Username);
        if (user == null) return result;
        string? s = request.Username.ToUpper().Trim() + request.Password.Trim();
        string? loginToken = CreateHash(s);
        if (user.LoginToken != loginToken) return result;
        UserWithRights currentUser = new()
        {
            Firstname = user.FirstName,
            Lastname = user.LastName,
            Username = user.Username
        };
        var q = context
                .UserUsergroups
                .Where(x => x.UserId == user.Id)
                .Select(x => x.UsergroupId);
        currentUser.Functions = await context.UsergroupFunctions
            .Where(x => q.Contains(x.UsergroupId))
            .Select(x => x.FunctionId)
            .ToListAsync();

        var authToken = DateTime.Now.Ticks.ToString();
        services.SetCacheItem("currentUser", user.Username, currentUser);
        result.Username = user.Username;
        result.Firstname = user.FirstName;
        result.Lastname = user.LastName;
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
