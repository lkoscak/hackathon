using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Core.DynamicSQL.Repository
{
    public class DynamicGetRequest
    {
        public int cmd;

        public bool useNullValues;

        [JsonProperty("params")]
        public object[] param;

        [JsonProperty("param")]
        public object[] param2 { set { param = value; } }

        [JsonProperty("extraParams")]
        public IDictionary<string, object> extraParam = new Dictionary<string, object>();

    }
}
