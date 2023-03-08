using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Core.Config
{
    public interface IConfigManager
    {
        string getConnstionString();
        string getHPConnectionString();
        string[] getQueuNames();
        string getHPConnectionStringTest();
        string getLoggerDefaultPath();
        string getCorsDomainsString();
        string getDisableGeocoding();
        string getGeocodingServerHostIp();
        string getZoneRequestId();
        string getGeocodingRequestId();
        string getGeocodingServerHostPort();
        string getStorageBasePath();
        string getDocumentAprovementBasePath();
        string getFileStorageRelativePath();
        string getImportLogRelativePath();
        string getMobilisisMsmqInjectorUrl();
        string getMobilisisMsmqInjectorToken();
        string getFleetWebBaseURL();

        string getEnvironmentId();

        string getDocumentImagesPath();
        string getLocationMapIconsPath();
        string getLocationReportIconsPath();
        string getExecQueueName();
        string[] getPrivateQueueNames();
        string getHereAppId();
        string getHereAppCode();
        string getHereApiKey();
        string getTachoAoiUrl();
        string getFilesApiURL();
        string getWebPortalApiURL();
        string getReportingApiUrl();
        string getDocumentFolder();
        string getInnerFleetWebBaseURL();
        string getShmitzProcessingUrl();
        string getPortalUrls();
        string getHereMapImageUrl();
        string getInfobipUrl();
        string getInfobipPassword();
        string getStaticMapUrl();
        string getHPCompanyID();
        string getRootPath();
        string getRoothPath();

        string getMQTTUsername();

        string getMQTTPassword();

        string getMQTTHost();
        string getStatusPageID();
        string getPicProductionPath();
    }
}
