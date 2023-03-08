using MobilisisGeocodingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiContext.Geocoding
{
    public interface IGeocodingRepository
    {
        public Task<GeocodingPlacemark> GetGeocodingAsync(GeocodingParameters parameters);
        public Task<string> GetLoactionAddress(float lat, float lon, string lang);
        public string GetLoactionAddressSync(float lat, float lon, string lang);
        public string GetLocationCity(float lat, float lon, string lang);
        public string GetLocationAddress(float lat, float lon, string lang);
    }
}
