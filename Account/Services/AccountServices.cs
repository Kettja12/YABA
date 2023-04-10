using Account.Model;
using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServicesShared;
using System.Dynamic;

namespace Account.Services;
public partial class AccountServices
{
    private const string CurrentUser = "CurrentUser";

    protected ILogger<AccountServices> logger;
    protected CacheServices services;
    protected AccountContext context;
    protected ApiService apiService;
    public AccountServices(
        ILogger<AccountServices> logger,
        CacheServices services,
        ApiService apiService,
        AccountContext context)
    {
        this.logger = logger;
        this.services = services;
        this.context = context;
        this.apiService = apiService;

    }

    protected string authToken = "";
    protected bool IsLoggedIn(string? authToken)
    {
        if (authToken == null || authToken == "") return false;
        this.authToken = authToken;
        if (currentUser == null) return false;
        return true;
    }
    protected UserWithRights? currentUser
    { 
        get{ return services.GetCacheItem<UserWithRights>(CurrentUser, authToken); } 
    }



}
