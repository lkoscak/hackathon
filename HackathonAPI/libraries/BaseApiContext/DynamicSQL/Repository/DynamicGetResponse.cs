using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Core.DynamicSQL.Repository
{
    public class DynamicGetResponse
    {
        public List<DynamicGetSingleResponse> cmd_results;
        public bool has_error;
        //public bool error;
        public string message;
    }
}
