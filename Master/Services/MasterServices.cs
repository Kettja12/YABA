using Master.Model;
using Master.Model.Master;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace Master.Services
{
    public class MasterServices
    {
        private CacheServices services;
        private MasterContext context;
        public MasterServices(CacheServices services, MasterContext context)
        {
            this.services = services;
            this.context = context;

        }
        public async Task<MasterLoginResponse> LoginMasterAsync(LoginRequest request)
        {
            var result = new MasterLoginResponse() { AuthToken = "" };
            if (string.IsNullOrEmpty(request.Username)
          || string.IsNullOrEmpty(request.Password)) return result;
            User? user = await context
              .Users
              .FirstOrDefaultAsync(x => x.Username == request.Username);
            if (user == null) return result;
            string? s = request.Username.ToUpper().Trim() + request.Password.Trim();
            string? loginToken = CreateHash(s);
            if (user.LoginToken != loginToken) return result;

            IQueryable<int> databaseIds = context.DatabaseUsers.Where(x => x.UsersId == user.Id).Select(x => x.DatabasesId);
            List<string> databasenames = context.Databases.Where(x => databaseIds.Contains(x.Id)).Select(x => x.Name).ToList();
            if (databasenames.Any())
            {
                result.Databases = databasenames;
                user.Databases = databasenames;
            }
            var authToken = DateTime.Now.Ticks.ToString();
            user.RefreshTime = DateTime.Now;
            services.SetCacheItem("user", authToken, user);
            result.AuthToken = authToken;
            return result;
        }
        public RefreshResponse RefreshToken(String authToken)
        {
            var user = services.GetCacheItem<User>("user", authToken);
            if (user != null)
            {
                if (DateTime.Now.Ticks < user.RefreshTime.AddMinutes(10).Ticks)
                {
                    user.RefreshTime = DateTime.Now;
                    services.SetCacheItem("user", authToken, user);
                    var response = new RefreshResponse() { AuthToken = authToken, refreshTime = user.RefreshTime };
                    return response;
                }
            }
            return new RefreshResponse() { AuthToken = "", refreshTime = new DateTime() };
        }
        public string CreateHash(string s)
        {
            byte[]? dataArray = Encoding.Unicode.GetBytes(s);
            HashAlgorithm sha = SHA256.Create();
            byte[] result = sha.ComputeHash(dataArray);
            return Encoding.Unicode.GetString(result);
        }

        public async Task<CurrentUser> GetCurrentUserAsync(string authToken)
        {
            var user = services.GetCacheItem<User>("user", authToken);
            if (user != null)
            {
                return new CurrentUser()
                {
                    UserName = user.Username,
                    AuthToken = authToken,
                    Database = user.CurrentDatabase
                };
            }
            return new CurrentUser() { UserName = "", AuthToken = "" };

        }
        public async Task<CurrentUser> SetUserDatabaseAsync(string authToken, SeUserDatabaseRequest request)
        {
            var user = services.GetCacheItem<User>("user", authToken);
            if (user != null)
            {
                user.CurrentDatabase = request.DatabaseName;
                if (user.Databases.Contains(request.DatabaseName))
                {
                    services.SetCacheItem("user", authToken, user);
                    return new CurrentUser()
                    {
                        UserName = user.Username,
                        AuthToken = authToken,
                        Database = user.CurrentDatabase
                    };
                }
            }
            return new CurrentUser() { UserName = "", AuthToken = "" };

        }
    }
}
