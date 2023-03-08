using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiContext.Geocoding
{
    public class GeocodingParameters
    {
        public string lat { get; set; }
        public string lon { get; set; }
        public string lang { get; set; }
        public string query { get; set; }
        public string json { get; set; }
        public string address { get; set; }
    }
}
