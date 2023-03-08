using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Core.DynamicSQL.Repository
{
    public class DynamicGetSingleResponse
    {
        public int cmd;
        public List<List<IDictionary<string, object>>> inner_results;
        public int error_number;
        public string message;

        public List<IDictionary<string, object>> getObjects(int resultSetNumber = 0)
        {
            return inner_results[resultSetNumber];
        }

        public IDictionary<string, object> getObject(int resultSetNumber = 0)
        {
            return inner_results[resultSetNumber][0];
        }
    }
}
