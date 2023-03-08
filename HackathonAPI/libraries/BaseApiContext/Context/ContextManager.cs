using WebApi.Core.Authentication;
using WebApi.Core.Caching;
using WebApi.Core.Config;
using WebApi.Core.Logging;
using WebApi.Core.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http.Dependencies;
using System.Data;
using System.Data.SqlClient;
using WebApi.Core.DB;
using WebApi.Core.Authentication.Repository.Model;
using WebApi.Core.Authentication.Repository;

namespace WebApi.Core.Context
{
    public class ContextManager : IContextManager
    {
        private Lazy<ILoggerManager> _loggerManagerLazy => new Lazy<ILoggerManager>(() => (ILoggerManager)_dependencyResolver.GetService(typeof(ILoggerManager)));
        public ILoggerManager loggerManager {
            get
            {
                return _loggerManagerLazy.Value;
            }
        }

        private Lazy<IConfigManager> _configManagerLazy => new Lazy<IConfigManager>(() => (IConfigManager)_dependencyResolver.GetService(typeof(IConfigManager)));
        public IConfigManager configManager {
            get
            {
                return _configManagerLazy.Value;
            }
        }

        private Lazy<ICacheManager> _cacheManagerLazy => new Lazy<ICacheManager>(() => (ICacheManager)_dependencyResolver.GetService(typeof(ICacheManager)));
        public ICacheManager cacheManager
        {
            get
            {
                return _cacheManagerLazy.Value;
            }
        }

        private Lazy<IDBManager> _dbManagerLazy => new Lazy<IDBManager>(() => (IDBManager)_dependencyResolver.GetService(typeof(IDBManager)));
        public IDBManager dbManager
        {
            get
            {
                return _dbManagerLazy.Value;
            }
        }

        public IDependencyResolver _dependencyResolver { private get; set; }

        private Session _session = null;
        private string _GUID = Guid.NewGuid().ToString();

        public Session session
        {
            get
            {
                try
                {
                    if (_session == null)
                    {
                        var identity = Thread.CurrentPrincipal.Identity;
                        if (identity == null && HttpContext.Current != null)
                            identity = HttpContext.Current.User.Identity;

                        if (identity != null && identity.IsAuthenticated)
                        {
                            BearerAuthenticationIdentity bearerAuth = identity as BearerAuthenticationIdentity;
                            _session = bearerAuth.Session;
                            return _session;
                        }
                    }
                    else
                    {
                        return _session;
                    }
                }
                catch (Exception)
                {
                    //Ne logirati zbog rekurzivnog poziva
                    //loggerManager.error(e, "Unable to get session object Thread.CurrentPrincipal.Identity");
                }
                return null;
            }
        }

        public string sessionKey
        {
            get
            {
                try
                {
                    return session.Key;
                }
                catch (Exception)
                {
                    //Ne logirati zbog rekurzivnog poziva
                    //loggerManager.error(e, "Unable to get session key");
                }
                return null;
            }
            set
            {
                ISessionRepository _sessionRepository = (ISessionRepository)_dependencyResolver.GetService(typeof(ISessionRepository));
                _session = _sessionRepository.GetActiveByKey(value);
            }
        }

        public string GUID
        {
            get {
                return _GUID;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string requestHost
        {
            get
            {
                return HttpContext.Current.Request.UrlReferrer.GetLeftPart(UriPartial.Authority);
            }
        }

        /// <summary>
        /// Returns current user agent from currrent request.
        /// </summary>
        /// <returns></returns>
        public string userAgent
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
                }
                catch (Exception)
                {
                }
                return "";
            }
        }

        /* Ne smije se staviti DI u constructor zbog cicle dependency-a (stackoverflow error)
        */
        public ContextManager()
        {
        }

        public T resolveDI<T>()
        {
            return (T)_dependencyResolver.GetService(typeof(T));
        }

        //public object resolveDI(Type type)
        //{
        //    return _dependencyResolver.GetService(type);
        //}

        public void Dispose()
        {
            //Makar ga dispose-a i Unity jer se dohvaća preko dependency injection-a
            if (_dbManagerLazy.IsValueCreated)
            {
                this.dbManager.Dispose();
            }
        }

        //public T getDI<T>()
        //{
        //    object returnObj = _dependencyResolver.GetService(typeof(T));
        //    BaseDI baseDI = (BaseDI)returnObj;
        //    baseDI.ContextManager = this;
        //    return (T)returnObj;
        //}

    }
}