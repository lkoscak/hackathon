using WebApi.Core.Authentication.Repository;
using WebApi.Core.Authentication.Repository.Model;
using WebApi.Core.Caching;
using WebApi.Core.Config;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net;

namespace WebApi.Core.Authentication.Attribute
{
    public class DbAuthorize : AuthorizeAttribute
    {
        //https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-filters
        //https://stackoverflow.com/questions/40281050/jwt-authentication-for-asp-net-web-api

        //private readonly SessionRepository _sessionRepository = new SessionRepository(Common.ConnectionString);

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var identity = Thread.CurrentPrincipal.Identity;

            if (identity == null && HttpContext.Current != null)
                identity = HttpContext.Current.User.Identity;

            if (identity != null && identity.IsAuthenticated)
            {
                BearerAuthenticationIdentity bearerAuth = identity as BearerAuthenticationIdentity;

                CacheManager _cacheManager = new CacheManager();
                Session session = _cacheManager.fromCacheToken(bearerAuth.SessionKey) as Session;
                if (session == null)
                {
                    ISessionRepository _sessionRepository = (ISessionRepository)actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(ISessionRepository));
                    session = _sessionRepository.GetActiveByKey(bearerAuth.SessionKey);
                    if (session != null)
                    {
                        bearerAuth.Session = session;
                        _cacheManager.toCacheToken(session.Key, session);
                        return true;
                    }
                }
                else
                {
                    bearerAuth.Session = session;
                    return true;
                }
            }
            return false;
        }
    }

    public class DbAuthorizeB2bAndPortal : AuthorizeAttribute
    {
        //https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-filters
        //https://stackoverflow.com/questions/40281050/jwt-authentication-for-asp-net-web-api

        //private readonly SessionRepository _sessionRepository = new SessionRepository(Common.ConnectionString);

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var identity = Thread.CurrentPrincipal.Identity;

            if (identity == null && HttpContext.Current != null)
                identity = HttpContext.Current.User.Identity;

            if (identity != null && identity.IsAuthenticated)
            {
                BearerAuthenticationIdentity bearerAuth = identity as BearerAuthenticationIdentity;

                CacheManager _cacheManager = new CacheManager();
                Session session = _cacheManager.fromCacheToken(bearerAuth.SessionKey) as Session;
                if (session == null)
                {
                    ISessionRepository _sessionRepository = (ISessionRepository)actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(ISessionRepository));

                    
                    ConfigManager configManager = new ConfigManager();
                    bool isRequestFromPortal = false;
                    string allowedPortalUrls = configManager.getPortalUrls();
                    if (actionContext.Request.Headers.Referrer != null && allowedPortalUrls != null)
                    {
                        string[] allowedPortalUrlsArray = allowedPortalUrls.Split(',');
                        foreach (string allowedPortalUrl in allowedPortalUrlsArray)
                        {
                            if (actionContext.Request.Headers.Referrer.OriginalString.Contains(allowedPortalUrl.Trim().ToLower()))
                            {
                                isRequestFromPortal = true;
                                break;
                            }
                        }
                    }
                    if (isRequestFromPortal) session = _sessionRepository.GetActiveByKey(bearerAuth.SessionKey);                    
                    else session = _sessionRepository.GetActiveByB2BKey(bearerAuth.SessionKey);

                    if (session != null)
                    {
                        bearerAuth.Session = session;
                        _cacheManager.toCacheToken(session.Key, session);
                        return true;
                    }
                }
                else
                {
                    bearerAuth.Session = session;
                    return true;
                }
            }
            return false;
        }
    }

    public class DbAuthorizeB2B : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var identity = Thread.CurrentPrincipal.Identity;

            if (identity == null && HttpContext.Current != null)
                identity = HttpContext.Current.User.Identity;

            if (identity != null && identity.IsAuthenticated)
            {
                BearerAuthenticationIdentity bearerAuth = identity as BearerAuthenticationIdentity;

                CacheManager _cacheManager = new CacheManager();
                Session session = _cacheManager.fromCacheToken(bearerAuth.SessionKey) as Session;
                if (session == null)
                {
                    ISessionRepository _sessionRepository = (ISessionRepository)actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(ISessionRepository));
                    session = _sessionRepository.GetActiveByB2BKey(bearerAuth.SessionKey);
                    if (session != null)
                    {
                        bearerAuth.Session = session;
                        _cacheManager.toCacheToken(session.Key, session);
                        return true;
                    }
                }
                else
                {
                    bearerAuth.Session = session;
                    return true;
                }
            }
            return false;
        }
    }
}