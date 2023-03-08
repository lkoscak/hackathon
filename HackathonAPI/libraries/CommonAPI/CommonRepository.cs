using BaseApiContext.ServiceResponse;
using CommonAPI.Model.Currency;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Core.Config;
using WebApi.Core.Context;
using WebApi.Core.Repository;

namespace CommonAPI
{
    public class CommonRepository : BaseRepository, ICommonRepository
    {
        private readonly IConfigManager _configManager;
        private readonly IContextManager _contextManager;
        public CommonRepository(IContextManager contextManager, IConfigManager configManager) : base(contextManager)
        {
            _configManager = configManager;
            _contextManager = contextManager;
        }

        public async Task<ServiceResponse<List<Currency>>> GetCurrencies()
        {
            List<Currency> currencies = new List<Currency>();

            using (SqlDataReader data = await DynamicQuery.GetDataAsync(Connection, Transaction, ContextManager.sessionKey, 3506))
            {
                while (data.Read())
                {
                    try
                    {
                        Currency currency = new Currency
                        {
                            id = int.Parse(data["cur_id"].ToString())
                        };
                        if (data["cur_name"] != DBNull.Value) currency.name = data["cur_name"].ToString();
                        currencies.Add(currency);
                    }
                    catch (Exception ex)
                    {
                        ContextManager.loggerManager.error(ex);
                    }

                }
            }

            return new ServiceResponse<List<Currency>>
            {
                IsSuccess = true,
                Data = currencies
            };
        }

        // returns country id by alpha-2 or alpha-3 code
        public async Task<ServiceResponse<int?>> GetCountryIdByCode(string countryCode)
        {
            int? countryId = null;
            using (SqlDataReader data = await DynamicQuery.GetDataAsync(Connection, Transaction, ContextManager.sessionKey, 5639, countryCode))
            {
                if (data.HasRows)
                {
                    try
                    {
                        data.Read();
                        if (data["objId"] != DBNull.Value) countryId = int.Parse(data["objId"].ToString());
                    }
                    catch (Exception ex)
                    {
                        ContextManager.loggerManager.error(ex);
                    }
                }

            }

            return new ServiceResponse<int?>
            {
                IsSuccess = countryId != null,
                Data = countryId
            };
        }

        public async Task<ServiceResponse<string>> GetCountryAlpha3CodeByAlpha2Code(string alpha2Code)
        {
            string alpha3Code = null;
            using (SqlDataReader data = await DynamicQuery.GetDataAsync(Connection, Transaction, ContextManager.sessionKey, 5656, alpha2Code))
            {
                if (data.HasRows)
                {
                    try
                    {
                        data.Read();
                        if (data["ISO3_alpha3_valid"] != DBNull.Value) alpha3Code = data["ISO3_alpha3_valid"].ToString();
                    }
                    catch (Exception ex)
                    {
                        ContextManager.loggerManager.error(ex);
                    }
                }

            }

            return new ServiceResponse<string>
            {
                IsSuccess = alpha3Code != null,
                Data = alpha3Code
            };
        }
    }
}
