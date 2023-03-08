using MobilisisGeocodingCore;
using MobilisisIpc.Udp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Core.Commons;
using WebApi.Core.Config;
using WebApi.Core.Context;
using WebApi.Core.Repository;

namespace BaseApiContext.Geocoding
{
    public class GeocodingRepository : BaseRepository, IGeocodingRepository
    {
        public GeocodingRepository(IContextManager contextManager) : base(contextManager)
        {
            //contextManager.loggerManager.addCustomRelativePath("GeocodingCore");
        }

        public async Task<GeocodingPlacemark> GetGeocodingAsync(GeocodingParameters parameters)
        {
            Stopwatch St = Stopwatch.StartNew();
            try
            {
                GeocodingPlacemark GeocodingResult = new GeocodingPlacemark(GeocodingPlacemarkStatuses.Unknown);
                GeocodingResult.Valid = false;

                    if (!String.IsNullOrWhiteSpace(parameters.lat) && !String.IsNullOrWhiteSpace(parameters.lon) && parameters.lat != "0" && parameters.lon != "0")
                    {
                        float Lat = Misc.ParseSingle(parameters.lat);
                        float Lon = Misc.ParseSingle(parameters.lon);
                        GeocodingResult.Latitude = (decimal)Lat;
                        GeocodingResult.Longitude = (decimal)Lon;
                        GeocodingResult = await this.ReverseGeocodeAsync(Lat, Lon, parameters.lang);
                    }
                    else if (!String.IsNullOrEmpty(parameters.address))
                    {
                        GeocodingResult = await this.ForwardGeocodeAsync(parameters.address, parameters.lang);
                    }
                    else
                    {
                        ContextManager.loggerManager.debug(JsonConvert.SerializeObject(parameters) + " -> could not determine geocoding direction - LAT/LON or Address incorrect");
                    }

                if (GeocodingResult.Valid && GeocodingResult.CountryName == "Kosovo") GeocodingResult.CountryNameCode = "XK";

                return GeocodingResult;
            }
            catch (Exception E)
            {
                ContextManager.loggerManager.error(E);
            }
            return null;
        }

        public async Task<string> GetLoactionAddress(float lat, float lon, string lang)
        {
            Stopwatch St = Stopwatch.StartNew();
            try
            {
                GeocodingPlacemark GeocodingResult = new GeocodingPlacemark(GeocodingPlacemarkStatuses.Unknown);
                GeocodingResult.Valid = false;

                if (lat != 0 && lon != 0)
                {
                    float Lat = lat;
                    float Lon = lon;
                    GeocodingResult.Latitude = (decimal)Lat;
                    GeocodingResult.Longitude = (decimal)Lon;
                    GeocodingResult = await this.ReverseGeocodeAsync(Lat, Lon, lang);
                }
                else
                {
                    ContextManager.loggerManager.debug("Lat lon are not correctly defined!");
                }

                if (GeocodingResult.Valid && GeocodingResult.CountryName == "Kosovo") GeocodingResult.CountryNameCode = "XK";
                string geocodeResultString = GeocodingResult.GetLocationString();
                if(!String.IsNullOrWhiteSpace(geocodeResultString) && !String.IsNullOrWhiteSpace(GeocodingResult.CountryName)) geocodeResultString = geocodeResultString + ", " + GeocodingResult.CountryName;
                return geocodeResultString;
            }
            catch (Exception E)
            {
                ContextManager.loggerManager.error(E);
            }
            return "";
        }

        public string GetLoactionAddressSync(float lat, float lon, string lang)
        {
            Stopwatch St = Stopwatch.StartNew();
            try
            {
                GeocodingPlacemark GeocodingResult = new GeocodingPlacemark(GeocodingPlacemarkStatuses.Unknown);
                GeocodingResult.Valid = false;

                if (lat != 0 && lon != 0)
                {
                    float Lat = lat;
                    float Lon = lon;
                    GeocodingResult.Latitude = (decimal)Lat;
                    GeocodingResult.Longitude = (decimal)Lon;
                    GeocodingResult = ReverseGeocode(Lat, Lon, lang);
                }
                else
                {
                    ContextManager.loggerManager.debug("Lat lon are not correctly defined!");
                }

                if (GeocodingResult.Valid && GeocodingResult.CountryName == "Kosovo") GeocodingResult.CountryNameCode = "XK";
                
                string geocodeResultString = GeocodingResult.GetLocationString();
                if (!String.IsNullOrWhiteSpace(geocodeResultString) && !String.IsNullOrWhiteSpace(GeocodingResult.CountryName)) geocodeResultString = geocodeResultString + ", " + GeocodingResult.CountryName;
                return geocodeResultString;
            }
            catch (Exception E)
            {
                ContextManager.loggerManager.error(E);
            }
            return "";
        }

        public string GetLocationCity(float lat, float lon, string lang)
        {
            try
            {
                GeocodingPlacemark GeocodingResult = getGeocodingResult(lat, lon, lang);
                return GeocodingResult.LocalityName;
            }
            catch (Exception E)
            {
                ContextManager.loggerManager.error(E);
            }
            return "";
        }
        public string GetLocationAddress(float lat, float lon, string lang)
        {
            try
            {
                GeocodingPlacemark GeocodingResult = getGeocodingResult(lat, lon, lang);
                return GeocodingResult.Address;
            }
            catch (Exception E)
            {
                ContextManager.loggerManager.error(E);
            }
            return "";
        }

        private GeocodingPlacemark getGeocodingResult(float lat, float lon, string lang)
        {
            if (String.IsNullOrWhiteSpace(lang)) lang = "hr";
            GeocodingPlacemark GeocodingResult = new GeocodingPlacemark(GeocodingPlacemarkStatuses.Unknown);
            GeocodingResult.Valid = false;

            if (lat != 0 && lon != 0)
            {
                float Lat = lat;
                float Lon = lon;
                GeocodingResult.Latitude = (decimal)Lat;
                GeocodingResult.Longitude = (decimal)Lon;
                GeocodingResult = ReverseGeocode(Lat, Lon, lang);
            }
            else
            {
                ContextManager.loggerManager.debug("Lat lon are not correctly defined!");
            }

            return GeocodingResult;
        }

        private async Task<GeocodingPlacemark> ReverseGeocodeAsync(float latitude, float longitude, string language)
        {
            using (IpcUdpConnection GeoConnection = CreateGeocodingConnection())
            {
                GeoConnection.Open();
                return await GeocodingPlacemark.GetGeocodingAsync(GeoConnection, latitude, longitude, language, "WebApiCore");
            }
        }

        private async Task<GeocodingPlacemark> ForwardGeocodeAsync(string address, string language)
        {
            using (IpcUdpConnection GeoConnection = CreateGeocodingConnection())
            {
                GeoConnection.Open();
                return await GeocodingPlacemark.GetGeocodingAsync(GeoConnection, address, language, "WebApiCore");
            }
        }

        private GeocodingPlacemark ReverseGeocode(float latitude, float longitude, string language)
        {
            using (IpcUdpConnection GeoConnection = CreateGeocodingConnection())
            {
                GeoConnection.Open();
                return GeocodingPlacemark.GetGeocodingBlocking(GeoConnection, latitude, longitude, language, "WebApiCore");
            }
        }

        private GeocodingPlacemark ForwardGeocode(string address, string language)
        {
            using (IpcUdpConnection GeoConnection = CreateGeocodingConnection())
            {
                GeoConnection.Open();
                return GeocodingPlacemark.GetGeocodingBlocking(GeoConnection, address, language, "WebApiCore");
            }
        }

        private IpcUdpConnection CreateGeocodingConnection()
        {
            // Expecting this settings in web.config
            //<add key="GeocodingServerHostIp" value="127.0.0.1" />
            //<add key="GeocodingServerHostPort" value="5386" />

            string GeocodingServerHostIp = ContextManager.configManager.getGeocodingServerHostIp();
            string GeocodingServerHostPortAsString = ContextManager.configManager.getGeocodingServerHostPort();
            int GeocodingServerHostPort = 5386;

            if (String.IsNullOrEmpty(GeocodingServerHostIp)) GeocodingServerHostIp = "127.0.0.1";
            if (!int.TryParse(GeocodingServerHostPortAsString, out GeocodingServerHostPort)) GeocodingServerHostPort = 5386;
            return IpcUdpConnection.CreateClient(GeocodingServerHostIp, GeocodingServerHostPort);
        }
    }
}
