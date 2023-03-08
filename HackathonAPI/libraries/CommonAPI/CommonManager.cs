using BaseApiContext.ServiceResponse;
using CommonAPI.Model.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Core.BusinessLogicManager;
using WebApi.Core.Context;

namespace CommonAPI
{
    public class CommonManager: BLManager
    {
        private readonly IContextManager _contextManager;
        private readonly ICommonRepository _commonRepository;

        public CommonManager(IContextManager contextManager) : base(contextManager)
        {
            _contextManager = contextManager;
            _commonRepository = contextManager.resolveDI<ICommonRepository>();
        }

        public async Task<ServiceResponse<List<Currency>>> GetCurrencies() { 
            ServiceResponse<List<Currency>> response = await _commonRepository.GetCurrencies();
            return response;
        }

        public async Task<ServiceResponse<int?>> GetCountryIdByCountryCode(string code)
        {
            ServiceResponse<int?> response = await _commonRepository.GetCountryIdByCode(code);
            return response;
        }
        public async Task<ServiceResponse<string>> GetCountryAlpha3CodeByAlpha2Code(string code)
        {
            ServiceResponse<string> response = await _commonRepository.GetCountryAlpha3CodeByAlpha2Code(code);
            return response;
        }
    }
}
