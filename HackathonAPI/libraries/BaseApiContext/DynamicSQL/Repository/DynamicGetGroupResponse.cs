using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Core.DynamicSQL.Repository
{
    public class DynamicGetGroupResponse
    {
        public List<DynamicGetSingleResponse> requests = new List<DynamicGetSingleResponse>();

        public List<IDictionary<string, object>> getObjects(int requestNumber = 0, int resultSetNumber = 0)
        {
            return requests[requestNumber].inner_results[resultSetNumber];
        }

        public List<IDictionary<string, object>> getObjectsByCmd(int cmdNumber, int resultSetNumber = 0)
        {
            return requests.FirstOrDefault(r => r.cmd == cmdNumber)?.inner_results[resultSetNumber];
        }

        public IDictionary<string, object> getObject(int requestNumber = 0, int resultSetNumber = 0)
        {
            return requests[requestNumber].inner_results[resultSetNumber][0];
        }

        public IDictionary<string, object> getObjectByCmd(int cmdNumber, int resultSetNumber = 0)
        {
            return requests.FirstOrDefault(r => r.cmd == cmdNumber)?.inner_results[resultSetNumber][0];
        }
    }
}