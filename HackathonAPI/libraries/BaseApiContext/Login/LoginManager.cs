using FleetWebApi.BusinessLogic.Login.Model;
using FleetWebApi.BusinessLogic.Login.Repository;
using WebApi.Core.BusinessLogicManager;
using WebApi.Core.Context;

namespace FleetWebApi.BusinessLogic.Login
{
    public class LoginManager : BLManager
    {
        private IContextManager contextManager;
        private ILoginRepository iLoginRepository;

        public LoginManager(IContextManager contextManager) : base(contextManager)
        {
            this.contextManager = contextManager;
            this.iLoginRepository = contextManager.resolveDI<LoginRepository>();
        }

        public async Task<string> login(LoginData loginData)
        {
            string sesKey = "";
            contextManager.dbManager.BeginTransaction(this);
            try
            {
                sesKey = await iLoginRepository.login(loginData);
                contextManager.dbManager.Commit(this);
            }
            catch (Exception ex)
            {
                contextManager.loggerManager.error(ex);
                contextManager.dbManager.Rollback(this);
            }

            return sesKey;
        }

    }
}