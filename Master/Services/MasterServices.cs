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
        public MasterServices(CacheServices services,MasterContext context) {
            this.services = services;
            this.context = context;

        }
        public async Task<MasterLoginResponse> LoginMasterAsync(LoginRequest request)
        {
            var result = new MasterLoginResponse() { AuthToken = ""};
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
            }
            var authToken = DateTime.Now.Ticks.ToString();
            services.SetCacheItem("user", authToken, user);
            result.AuthToken= authToken;
            return result;
        }
        public MasterLoginResponse RefreshToken(RefreshRequest request)
        {
            var user = services.GetCacheItem<User>("user", request.AuthToken);
            services.DeleteCacheItem("user",request.AuthToken);
            if (long.TryParse(request.AuthToken, out long tics))
            {
                DateTime dt = new DateTime(tics).AddMinutes(10);
                if (DateTime.Now.Ticks<dt.Ticks)
                {
                    var response = new MasterLoginResponse() { AuthToken = DateTime.Now.Ticks.ToString()};
                    services.SetCacheItem("user", response.AuthToken, user);
                    return response;
                }
            };
            return new MasterLoginResponse() { AuthToken = ""};
        }
        public string CreateHash(string s)
        {
            byte[]? dataArray = Encoding.Unicode.GetBytes(s);
            HashAlgorithm sha = SHA256.Create();
            byte[] result = sha.ComputeHash(dataArray);
            return Encoding.Unicode.GetString(result);
        }
    }
}
