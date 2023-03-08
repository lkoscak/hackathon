using WebApi.Core.Authentication.Repository;
using WebApi.Core.Context;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Injection;
using Unity.Resolution;

namespace WebApi.Core.Resolver
{
    public class UnityResolver : IDependencyResolver
    {
        protected IUnityContainer container;

        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                //serviceType.GetType();
                //if (serviceType.IsSubclassOf(typeof(BaseApiController)) || serviceType.IsSubclassOf(typeof(BaseDI)))
                //{
                ContextManager contextManager = (ContextManager)container.Resolve(typeof(IContextManager));
                contextManager._dependencyResolver = this;

                var resolvedObject = container.Resolve(serviceType, new ResolverOverride[]
                                {
                                    new ParameterOverride(typeof(IContextManager), contextManager)
                                });

                return resolvedObject;
                //}
                //return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}