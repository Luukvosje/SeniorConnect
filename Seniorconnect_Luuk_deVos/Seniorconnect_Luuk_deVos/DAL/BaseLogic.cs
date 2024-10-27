using Seniorconnect_Luuk_deVos.Model;

namespace Seniorconnect_Luuk_deVos.DAL
{
    public abstract class BaseLogic
    {

        protected readonly MySqlDbContext _dbContext;
        public readonly IConfiguration _configuration;
        private IConfiguration configuration;

        public BaseLogic(MySqlDbContext dbContext, IConfiguration configuration)
        {
            this._dbContext = dbContext;
            this._configuration = configuration;
        }
    }
}
