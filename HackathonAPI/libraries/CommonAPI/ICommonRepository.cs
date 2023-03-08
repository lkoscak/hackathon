using BaseApiContext.ServiceResponse;
using CommonAPI.Model.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPI
{
    public interface ICommonRepository
    {
        Task<ServiceResponse<List<Currency>>> GetCurrencies();
        Task<ServiceResponse<int?>> GetCountryIdByCode(string countryCode);
        Task<ServiceResponse<string>> GetCountryAlpha3CodeByAlpha2Code(string alpha2Code);
    }
}
