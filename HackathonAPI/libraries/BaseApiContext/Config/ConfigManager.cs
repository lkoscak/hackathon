using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WebApi.Core.Config
{
    public class ConfigManager : IConfigManager
    {
        private string _connectionString;
        private string _corsDomainsString;
        private string _HPConnectionString;
        private string _HPConnectionStringTest;

        private string _loggerDefaultPath;

        private string _filesApiURL;
        private string _documentFolder;

        private string _disableGeocoding;
        private string _geocodingServerHostIp;
        private string _getGeocodingRequestId;
        private string _getZoneRequestId;
        private string _geocodingServerHostPort;
        private string _storageBasePath;
        private string _documentAprovementBasePath;
        private string _fileStorageRelativePath;
        private string _importLogRelativePath;
        private string _mobilisisMsmqInjectorUrl;
        private string _mobilisisMsmqInjectorToken;
        private string _environmentId;
        private string _fleetWebBaseURL;
        private string _documentImagesPath;
        private string _execQueueName;
        private string _innerFleetWebBaseURL;
        private string _schmitzProcessingURL;
        private string _roothPath;


        private string _hereAppId;
        private string _hereAppCode;
        private string _hereApiKey;
        private string _tachoAoiUrl;
        private string _rootPath;

        private string _webApiUrl;
        private string _reportingApiUrl;

        private string _locationMapIconsPath;
        private string _locationReportIconsPath;
        private string _portalUrls;
        private string _hereMapImageUrl;
        string _infobipUrl;
        string _infobipAppKey;
        string _staticMapApiUrl;
        private string _HPComapnyID;      
        
        private string[] _queueNames;
        private string[] _machineNames;

        private string _statusPageID;

        private string _MQTTUsername;
        private string _MQTTPassword;
        private string _MQTTHost;

        private string _picProductionPath;

        public ConfigManager()
        {
            _portalUrls = ConfigurationManager.AppSettings["portalUrls"];
            _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
             if (ConfigurationManager.ConnectionStrings["HPConnectionString"] != null) _HPConnectionString = ConfigurationManager.ConnectionStrings["HPConnectionString"].ConnectionString;
            if (ConfigurationManager.ConnectionStrings["HPConnectionStringTest"] != null) _HPConnectionStringTest = ConfigurationManager.ConnectionStrings["HPConnectionStringTest"].ConnectionString;
            _corsDomainsString = ConfigurationManager.AppSettings["CORS_ORIGINS"];

            _filesApiURL = ConfigurationManager.AppSettings["filesApiURL"];
            _webApiUrl = ConfigurationManager.AppSettings["webApiURL"];
            _reportingApiUrl = ConfigurationManager.AppSettings["reportingApiURL"];
            _documentFolder = ConfigurationManager.AppSettings["DocumentFolder"];

            _loggerDefaultPath = ConfigurationManager.AppSettings["LogDirDefault"];
            _fleetWebBaseURL = ConfigurationManager.AppSettings["FleetWebBaseURL"];

            _disableGeocoding = ConfigurationManager.AppSettings["DisableGeocoding"];
            _geocodingServerHostIp = ConfigurationManager.AppSettings["GeocodingServerHostIp"];
            _getGeocodingRequestId = ConfigurationManager.AppSettings["GeocodingRequestId"];
            _getZoneRequestId = ConfigurationManager.AppSettings["GetZoneRequestId"];
            _geocodingServerHostPort = ConfigurationManager.AppSettings["GeocodingServerHostPort"];

            _storageBasePath = ConfigurationManager.AppSettings["StorageBasePath"];

            _documentAprovementBasePath = ConfigurationManager.AppSettings["DocumentHistoryPath"];
            _fileStorageRelativePath = ConfigurationManager.AppSettings["FileStorageRelativePath"];
            _importLogRelativePath = ConfigurationManager.AppSettings["ImportLogRelativePath"];
            _mobilisisMsmqInjectorUrl = ConfigurationManager.AppSettings["MobilisisMsmqInjectorUrl"];
            _mobilisisMsmqInjectorToken = ConfigurationManager.AppSettings["MobilisisMsmqInjectorToken"];
            _environmentId = ConfigurationManager.AppSettings["EnvironmentId"];
            _documentImagesPath = ConfigurationManager.AppSettings["DocumentImagesPath"];
            _execQueueName = ConfigurationManager.AppSettings["ExecQueueName"];

            _hereAppId = ConfigurationManager.AppSettings["HereAppId"];
            _hereAppCode = ConfigurationManager.AppSettings["HereAppCode"];
            _hereApiKey = ConfigurationManager.AppSettings["HereApiKey"];
            _tachoAoiUrl = ConfigurationManager.AppSettings["TachoApiUrl"];
            _innerFleetWebBaseURL = ConfigurationManager.AppSettings["InnerFleetWebBaseURL"];
            _schmitzProcessingURL = ConfigurationManager.AppSettings["SchmitzProcessingURL"];

            _locationMapIconsPath = ConfigurationManager.AppSettings["LocationMapIconsPath"];
            _locationReportIconsPath = ConfigurationManager.AppSettings["LocationReportIconsPath"];

            _hereMapImageUrl = ConfigurationManager.AppSettings["HereMapImageUrl"];
            _infobipUrl = ConfigurationManager.AppSettings["InfobipUrl"];
            _infobipAppKey = ConfigurationManager.AppSettings["InfobipAppKey"];
            _staticMapApiUrl = ConfigurationManager.AppSettings["StaticMapApiUrl"];
            _HPComapnyID = ConfigurationManager.AppSettings["HPCompanyID"];
            _roothPath = ConfigurationManager.AppSettings["roothPath"];          
            if(!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["MSMQPoint"])) _queueNames = ConfigurationManager.AppSettings["MSMQPoint"].Split(';');
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["StatusPageID"])) _statusPageID = ConfigurationManager.AppSettings["StatusPageID"];
            _MQTTUsername = ConfigurationManager.AppSettings["MqttUsername"];
            _MQTTPassword = ConfigurationManager.AppSettings["MqttPassword"];
            _MQTTHost = ConfigurationManager.AppSettings["MqttHost"];
            if(!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["MSMQPoint"])) _queueNames = ConfigurationManager.AppSettings["MSMQPoint"].Split(';');
            _picProductionPath = ConfigurationManager.AppSettings["PicProductionPath"];
        }
        public string[] getQueuNames()
        {
            return _queueNames;
        }

        public string getConnstionString()
        {
            return _connectionString;
        }

        public string getHPConnectionString()
        {
            return _HPConnectionString;
        }

        public string getHPConnectionStringTest()
        {
            return _HPConnectionStringTest;
        }


        public string getLoggerDefaultPath()
        {
            return _loggerDefaultPath;
        }

        public string getFleetWebBaseURL()
        {
            return _fleetWebBaseURL;
        }

        public string getCorsDomainsString()
        {
            return _corsDomainsString;
        }

        public string getDisableGeocoding()
        {
            return _disableGeocoding;
        }
        public string getGeocodingServerHostIp()
        {
            return _geocodingServerHostIp;
        }
        public string getGeocodingRequestId()
        {
            return _getGeocodingRequestId;
        }
        public string getZoneRequestId()
        {
            return _getZoneRequestId;
        }
        public string getGeocodingServerHostPort()
        {
            return _geocodingServerHostPort;
        }

        public string getStorageBasePath()
        {
            return _storageBasePath;
        }

        public string getDocumentAprovementBasePath()
        {
            return _documentAprovementBasePath;
        }
        public string getFileStorageRelativePath()
        {
            return _fileStorageRelativePath;
        }
        public string getImportLogRelativePath()
        {
            return _importLogRelativePath;
        }

        public string getMobilisisMsmqInjectorUrl()
        {
            return _mobilisisMsmqInjectorUrl;
        }

        public string getMobilisisMsmqInjectorToken()
        {
            return _mobilisisMsmqInjectorToken;
        }
        public string getEnvironmentId()
        {
            return _environmentId;
        }

        public string getDocumentImagesPath() 
        {
            return _documentImagesPath;
        }
        public string getLocationMapIconsPath()
        {
            return _locationMapIconsPath;
        }
        public string getLocationReportIconsPath()
        {
            return _locationReportIconsPath;
        }
        public string getExecQueueName()
        {
            return _execQueueName;
        }

        public string getHereAppId()
        {
            return _hereAppId;
        }

        public string getHereAppCode()
        {
            return _hereAppCode;
        }
        public string getHereApiKey()
        {
            return _hereApiKey;
        }
        public string getTachoAoiUrl()
        {
            return _tachoAoiUrl;
        }

        public string getFilesApiURL()
        {
            return _filesApiURL;
        }

        public string getDocumentFolder()
        {
            return _documentFolder;
        }

        public string getInnerFleetWebBaseURL()
        {
            return _innerFleetWebBaseURL;
        }

        public string getShmitzProcessingUrl()
        {
            return _schmitzProcessingURL;
        }

        public string getWebPortalApiURL()
        {
            return _webApiUrl;
        }
        public string getReportingApiUrl()
        {
            return _reportingApiUrl;
        }
        public string getPortalUrls(){
            return _portalUrls;
        }

        public string getHereMapImageUrl()
        {
            return _hereMapImageUrl;
        }
        public string getInfobipUrl()
        {
            return _infobipUrl;
        }
        public string getInfobipPassword()
        {
            return _infobipAppKey;
        }
        public string getStaticMapUrl()
        {
            return _staticMapApiUrl;
        }
        public string getHPCompanyID()
        {
            return _HPComapnyID;
        }

        public string getRoothPath()
        {
            return _roothPath;
        }

        public string[] getPrivateQueueNames()
        {
            return _queueNames;
        }

        public string getRootPath()
        {
            throw new NotImplementedException();
        }

        public string getStatusPageID()
        {
            return _statusPageID;
        }

        public string getMQTTUsername()
        {
            return _MQTTUsername;
        }

        public string getMQTTPassword()
        {
            return _MQTTPassword;
        }

        public string getMQTTHost()
        {
            return _MQTTHost;
        }

        public string getPicProductionPath()
        {
            return _picProductionPath;
        }
    }
}