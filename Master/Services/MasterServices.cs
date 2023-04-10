using ServicesShared;

namespace Master.Services
{
    public partial class MasterServices
    {
        private const string LoginUser = "LoginUser";

        private CacheServices services;
        private MasterContext context;
        public MasterServices(CacheServices services, MasterContext context)
        {
            this.services = services;
            this.context = context;

        }
    }
}
