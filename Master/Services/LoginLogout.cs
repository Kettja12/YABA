using Master.Model;
using Microsoft.EntityFrameworkCore;
using NUlid;
using ServicesShared;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;

namespace Master.Services
{
    public partial class MasterServices
    {
        public async Task<dynamic> LoginMasterAsync(LoginRequest request)
        {
            dynamic result = new ExpandoObject();
            result.Status = -1;
            result.Message = Messages.InvalidParameters;
            result.LoginUser = new ServicesShared.LoginUser();

            if (string.IsNullOrEmpty(request.Username)
            || string.IsNullOrEmpty(request.Password))
                return result;

            Model.LoginUser? user = await context
              .LoginUsers
              .FirstOrDefaultAsync(x => x.Username == request.Username);
            if (user == null) return result;

            string? s = request.Username.ToUpper().Trim() + request.Password.Trim();
            string? loginToken = CreateHash(s);
            if (user.LoginToken != loginToken) return result;
     
            result.LoginUser.UserName = request.Username;
            IQueryable<int> databaseIds = context.DatabaseUsers.Where(x => x.UsersId == user.Id).Select(x => x.DatabasesId);
            List<string> databasenames = context.Databases.Where(x => databaseIds.Contains(x.Id)).Select(x => x.Name).ToList();
            if (databasenames.Any())
            {
                result.LoginUser.Databases = databasenames;
            }
            var authToken = Ulid.NewUlid().ToString();
            result.LoginUser.RefreshTime = DateTime.Now;
            result.LoginUser.AuthToken = authToken;
            services.SetCacheItem(LoginUser, authToken, result.LoginUser);
            result.Status = 0;
            result.Message = Messages.OK;
            return result;
        }
        public dynamic RefreshToken(ServicesShared.AuthTokenRequest request)
        {
            dynamic result = new ExpandoObject();
            result.Status = -1;
            result.Message = Messages.InvalidParameters;
            var user = services.GetCacheItem<ServicesShared.LoginUser>(LoginUser, request.AuthToken);
            if (user == null)
                return result;
            if (DateTime.Now.Ticks < user.RefreshTime.AddMinutes(10).Ticks)
            {
                user.RefreshTime = DateTime.Now;
                services.SetCacheItem(LoginUser, request.AuthToken, user);
                result.AuthToken = request.AuthToken;
                result. refreshTime = user.RefreshTime;
                result.Status = 0;
                result.Message = Messages.Success;
                return result;
            }
            return result;
        }
        public dynamic Logout(ServicesShared.AuthTokenRequest request)
        {
            dynamic result = new ExpandoObject();
            result.Status = -1;
            result.Message = Messages.InvalidParameters;
            var logout = services.DeleteCacheItem(LoginUser, request.AuthToken);
            if (logout)
            {
                result.Status = 0;
                result.Message = Messages.Success;
            }
            return result;
        }
        private string CreateHash(string s)
        {
            byte[]? dataArray = Encoding.UTF8.GetBytes(s);
            HashAlgorithm sha = SHA256.Create();
            byte[] result = sha.ComputeHash(dataArray);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sBuilder.Append(result[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
        public dynamic GetLoginUser(ServicesShared.AuthTokenRequest request)
        {
            dynamic response = new ExpandoObject();
            response.Status = -1;
            response.Message = Messages.InvalidParameters;

            var user = services.GetCacheItem<ServicesShared.LoginUser>(LoginUser, request.AuthToken);
            if (user != null)
            {
                response.Status = 0;
                response.Message = Messages.OK;
                response.LoginUser = user;
            }
            return response;
        }
        public dynamic SetLoginUserDatabase(SeUserDatabaseRequest request)
        {
            dynamic response = new ExpandoObject();
            response.Status = -1;
            response.Message = Messages.Operationfailed;

            var user = services.GetCacheItem<ServicesShared.LoginUser>(LoginUser, request.AuthToken);
            if (user != null && user.Databases != null)
            {
                user.CurrentDatabase = request.DatabaseName;
                if (user.Databases.Contains(request.DatabaseName))
                {
                    services.SetCacheItem(LoginUser, request.AuthToken, user);
                    response.Status = 0;
                    response.Message = Messages.OK;
                }
            }
            return response;
        }
    }
}
