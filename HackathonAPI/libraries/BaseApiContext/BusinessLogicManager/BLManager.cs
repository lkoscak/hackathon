using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Core.Context;

namespace WebApi.Core.BusinessLogicManager
{
    public abstract class BLManager : IDisposable
    {
        protected IContextManager ContextManager { get; set; }

        public BLManager(IContextManager contextManager)
        {
            this.ContextManager = contextManager;
        }
        public void Dispose()
        {
            
        }
    }
}