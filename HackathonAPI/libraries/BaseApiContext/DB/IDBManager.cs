using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Core.BusinessLogicManager;

namespace WebApi.Core.DB
{
    public interface IDBManager
    {
        IDbConnection GetConnection(string connectionName = null);
        IDbTransaction GetTransaction(string connectionName = null);
        void BeginTransaction(BLManager blManager, IsolationLevel isolationLevel = IsolationLevel.Unspecified, string connectionName = null);
        void Commit(BLManager blManager, string connectionName = null);
        void Rollback(BLManager blManager, string connectionName = null);
        //IDbConnection Connection { get; }
        //IDbTransaction Transaction { get; }

        //IDisposable Start();
        //void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        //void Commit();
        //void Rollback();
        //void End(bool commit = true);

        void Dispose();
    }
}
