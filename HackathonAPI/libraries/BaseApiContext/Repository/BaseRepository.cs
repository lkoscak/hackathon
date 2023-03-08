using WebApi.Core.Config;
using WebApi.Core.Context;
using WebApi.Core.Logging;
using WebApi.Core.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using WebApi.Core.DynamicSQL;
using System.Data.SqlClient;
using System.Data;

namespace WebApi.Core.Repository
{
    //TODO Rename-ati u SQLServerBaseRepository
    public abstract class BaseRepository : BaseDI
    {
        private string _ConnectionName = "default";
        /// <summary>
        /// Može se postaviti od strane manager-a
        /// </summary>
        public string ConnectionName {
            protected get
            {
                return _ConnectionName;
            }
            set
            {
                _ConnectionName = value;
            }
        }
        protected SqlConnection Connection
        {
            get
            {
                //if (ContextManager.dbManager.Connection == null) throw new Exception("DB connection not initialized");
                return (SqlConnection)ContextManager.dbManager.GetConnection(_ConnectionName);
            }
        }
        protected SqlTransaction Transaction
        {
            get
            {
                return (SqlTransaction)ContextManager.dbManager.GetTransaction(_ConnectionName);
            }
        }
        
        private Lazy<DynamicQuery> _dynamicQuery => new Lazy<DynamicQuery>(() => new DynamicQuery(ContextManager.loggerManager));
        public DynamicQuery DynamicQuery
        {
            get
            {
                return _dynamicQuery.Value;
            }
        }

        public BaseRepository(IContextManager contextManager) : base(contextManager)
        {
        }
    }
}
