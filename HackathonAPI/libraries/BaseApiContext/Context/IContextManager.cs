using WebApi.Core.Caching;
using WebApi.Core.Config;
using WebApi.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using WebApi.Core.DB;
using WebApi.Core.Authentication.Repository.Model;

namespace WebApi.Core.Context
{
    public interface IContextManager : IDisposable
    {
        ILoggerManager loggerManager { get; }
        IConfigManager configManager { get; }
        ICacheManager cacheManager { get; }
        IDBManager dbManager { get; }
        Session session { get; }
        string sessionKey { get; set; }
        string GUID  { get; }
        string requestHost { get; }
        string userAgent { get; }
        T resolveDI<T>();

        //T getDI<T>();

    }
}
