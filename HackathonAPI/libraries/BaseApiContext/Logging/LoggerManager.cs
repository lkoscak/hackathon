using WebApi.Core.Commons;
using WebApi.Core.Config;
using WebApi.Core.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApi.Core.Logging
{
    public class LoggerManager : ILoggerManager
    {
        protected IConfigManager _configManager;
        protected IContextManager _contextManager;
        public string _path;

        //public IContextManager contextManager {
        //    set
        //    {
        //        _contextManager = value;
        //    }
        //}

        public LoggerManager(
            IConfigManager configManager,
            IContextManager contextManager
            )
        {
            _configManager = configManager;
            _contextManager = contextManager;
            _path = configManager.getLoggerDefaultPath();
        }

        public void debug(string message, Exception e = null)
        {
            Log(message, "DEBUG");
        }

        public void error(Exception e, string message = "")
        {
            Log(message + "\r\n" + e.Message + "\r\n" + e.StackTrace, "ERROR");
        }

        public void info(string message, Exception e = null)
        {
            Log(message, "INFO");
        }

        public void trace(string message, Exception e = null)
        {
            Log(message, "TRACE");
        }

        public void warning(string message, Exception e = null)
        {
            Log(message, "WARNING");
        }

        private void Log(string message, string type)
        {
            try
            {
                string Dir = _path;
                if (Dir == "") return;
                string File = Dir + "\\" + DateTime.Now.ToString("yyyyMMddHH") + ".txt";
                if (!Directory.Exists(Dir)) Directory.CreateDirectory(Dir);

                string uniqueId = !String.IsNullOrEmpty(_contextManager.sessionKey) ? _contextManager.sessionKey : "PUBLIC " + _contextManager.GUID;
                string Text = String.Format("[{0}][{1}]{2}", type, uniqueId, message);

                string str = DateTime.Now.ToString("HH:mm:ss> ") + Text;
                using (FileStream fs = new FileStream(File, FileMode.Append, FileAccess.Write, FileShare.Write))
                {
                    //byte[] data = Encoding.ASCII.GetBytes(str + "\r\n");
                    byte[] data = Encoding.UTF8.GetBytes(str + "\r\n");
                    fs.Write(data, 0, data.Length);
                }
            }
            catch { }
        }

        public void addCustomRelativePath(params string[] customRelativePath)
        {
            List<string> paths = new List<string>();
            paths.Add(_path);
            paths.AddRange(customRelativePath);

            _path = Path.Combine(paths.ToArray());
            //_path = _path + "\\" + customRelativePath;
        }
    }
}