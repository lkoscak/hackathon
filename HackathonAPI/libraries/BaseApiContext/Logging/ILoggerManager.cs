using WebApi.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Core.Logging
{
    public interface ILoggerManager
    {
        //IContextManager contextManager { set; }
        //void setContextManager(IContextManager contextManager);
        //void addCustomRelativePath(string customRelativePath);
        void addCustomRelativePath(params string[] customRelativePath);
        void debug(string message, Exception e = null);
        void info(string message, Exception e = null);
        void warning(string message, Exception e = null);
        void error(Exception e, string message = "");
        void trace(string message, Exception e = null);
    }
}
