using HackathonAPI.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Unity;
using Unity.Lifetime;
using WebApi.Core.Authentication;
using WebApi.Core.Caching;
using WebApi.Core.Config;
using WebApi.Core.Context;
using WebApi.Core.DB;
using WebApi.Core.Logging;
using WebApi.Core.Resolver;

namespace HackathonAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Add authentication handler for Bearer authentication
            config.MessageHandlers.Add(new BearerAuthenticationHandler());

            // Register Dependency injection
            var container = new UnityContainer();
            registerCoreDI(container);
            registerCustomDI(container);

            IConfigManager configManager = container.Resolve<IConfigManager>();
            config.DependencyResolver = new UnityResolver(container);

            // Web API configuration and services
            var origins = configManager.getCorsDomainsString();
            var cors = new EnableCorsAttribute(origins, "*", "*");
            cors.PreflightMaxAge = 600;
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void registerCoreDI(UnityContainer container)
        {
            container.RegisterType<IConfigManager, ConfigManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICacheManager, CacheManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILoggerManager, LoggerManager>(new HierarchicalLifetimeManager());
            container.RegisterType<IContextManager, ContextManager>(new HierarchicalLifetimeManager());
            container.RegisterType<IDBManager, DBManager>(new HierarchicalLifetimeManager());

        }

        private static void registerCustomDI(UnityContainer container)
        {
            container.RegisterType<IBaseRepository, BaseRepository>(new HierarchicalLifetimeManager());
        }
    }
}
