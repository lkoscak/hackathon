using WebApi.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Core.DynamicSQL.Repository
{
    public interface IDynamicGetRepository
    {
        //Task<List<Dictionary<string, object>>> GetAsync(DynamicGetRequest param);
        Task<DynamicGetGroupResponse> GetAsync(List<DynamicGetRequest> param, string sessionKey);
        Task<int> SetAsync(List<DynamicGetRequest> param, string sessionKey);

        
    }
}
