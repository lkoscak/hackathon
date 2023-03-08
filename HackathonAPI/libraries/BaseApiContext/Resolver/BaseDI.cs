using WebApi.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Core.Resolver
{
    public class BaseDI
    {
        protected IContextManager ContextManager { get; }

        public BaseDI(IContextManager contextManager)
        {
            ContextManager = contextManager;
        }
    }
}