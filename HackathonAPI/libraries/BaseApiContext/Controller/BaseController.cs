using WebApi.Core.Authentication;
using WebApi.Core.Commons;
using WebApi.Core.Config;
using WebApi.Core.Context;
using WebApi.Core.Logging;
using WebApi.Core.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Unity;

namespace WebApi.Core.Controller
{
    public abstract class BaseApiController: ApiController
    {
        protected IContextManager ContextManager { get; }
        //public IConfigManager _configManager {
        //    get
        //    {
        //        return (IConfigManager)ControllerContext.Configuration.DependencyResolver.GetService(typeof(IConfigManager));
        //    }
        //}

        //public ILoggerManager _loggerManager
        //{
        //    get
        //    {
        //        return (ILoggerManager)ControllerContext.Configuration.DependencyResolver.GetService(typeof(ILoggerManager));
        //    }
        //}

        public BaseApiController(IContextManager contextManager)
        {
            this.ContextManager = contextManager;
        }

        //protected override void Initialize(HttpControllerContext controllerContext)
        //{
        //    base.Initialize(controllerContext);

        //    //_configManager = (IConfigManager)controllerContext.Configuration.DependencyResolver.GetService(typeof(IConfigManager));
        //    ContextManager contextManager = (ContextManager)controllerContext.Configuration.DependencyResolver.GetService(typeof(ContextManager));
        //    contextManager._dependencyResolver = controllerContext.Configuration.DependencyResolver;
        //    this.ContextManager = contextManager;
        //    //_loggerManager = (ILoggerManager)controllerContext.Configuration.DependencyResolver.GetService(typeof(ILoggerManager));
        //    //_loggerManager.contextManager = ContextManager;

        //    contextInitialized();
        //}

        //protected abstract void contextInitialized();

    }
}