using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebApi.Core.BusinessLogicManager;
using WebApi.Core.Context;

namespace WebApi.Core.DB
{
    //TODO Rename-ati u SQLServerDBManager
    public class DBManager : IDBManager, IDisposable
    {
        private const string DEFAULT_CONNECTION = "default";
        protected IContextManager _contextManager;

        //private IDbConnection _DBConnection = null;
        private IDictionary<string, IDbConnection> _DBConnections = new Dictionary<string, IDbConnection>();
        private IDictionary<string, Tuple<BLManager, IDbTransaction>> _DBTransactions = new Dictionary<string, Tuple<BLManager, IDbTransaction>>();

        //private IDbTransaction _DbTransaction = null;

        public IDbConnection GetConnection(string connectionName = null)
        {
            IDbConnection _DBConnection = null;
            if (!_DBConnections.TryGetValue(connectionName == null ? DEFAULT_CONNECTION : connectionName, out _DBConnection))
            {
                SqlConnection c = new SqlConnection(_contextManager.configManager.getConnstionString());
                _DBConnections.Add(connectionName == null ? DEFAULT_CONNECTION : connectionName, c);
                _DBConnection = c;
                c.Open();
            }
            return _DBConnection;
        }

        public IDbTransaction GetTransaction(string connectionName = null)
        {
            Tuple<BLManager, IDbTransaction> _DBTransactionTuple = GetTransactionTuple(connectionName);
            return _DBTransactionTuple != null ? _DBTransactionTuple.Item2 : null;
        }
        private Tuple<BLManager, IDbTransaction> GetTransactionTuple(string connectionName = null)
        {
            Tuple<BLManager, IDbTransaction> _DBTransactionTuple = null;
            if (!_DBTransactions.TryGetValue(connectionName == null ? DEFAULT_CONNECTION : connectionName, out _DBTransactionTuple))
            {
            }
            return _DBTransactionTuple;
        }

        //public IDbConnection Connection
        //{
        //    get
        //    {
        //        return _DBConnection;
        //    }
        //}

        //public IDbTransaction Transaction
        //{
        //    get
        //    {
        //        return _DbTransaction;
        //    }
        //}

        public DBManager(
            IContextManager contextManager
            )
        {
            _contextManager = contextManager;
        }

        //public IDisposable Start()
        //{
        //    if (_DBConnection != null)
        //    {
        //        End(false);
        //    }
        //    SqlConnection c = new SqlConnection(_contextManager.configManager.getConnstionString());
        //    _DBConnection = c;
        //    c.Open();
        //    return this;
        //}

        public void BeginTransaction(BLManager blManager, IsolationLevel isolationLevel = IsolationLevel.Unspecified, string connectionName = null)
        {
            IDbTransaction _DBTransaction = GetTransaction(connectionName);
            if (_DBTransaction != null)
            {
                throw new Exception("Transaction already initialized");
                //_DbTransaction.Dispose();
                //_DbTransaction = null;
            }
            if (isolationLevel != IsolationLevel.Unspecified)
            {
                _DBTransaction = GetConnection(connectionName).BeginTransaction(isolationLevel);
            }
            else
            {
                _DBTransaction = GetConnection(connectionName).BeginTransaction();
            }
            _DBTransactions.Add(connectionName == null ? DEFAULT_CONNECTION : connectionName, new Tuple<BLManager, IDbTransaction>(blManager, _DBTransaction));
        }

        public void Commit(BLManager blManager, string connectionName = null)
        {
            Tuple<BLManager, IDbTransaction> _DBTransactionTuple = GetTransactionTuple(connectionName);
            if (_DBTransactionTuple != null && blManager == _DBTransactionTuple.Item1)
            {
                _DBTransactionTuple.Item2.Commit();
                _DBTransactionTuple.Item2.Dispose();
                _DBTransactions.Remove(connectionName == null ? DEFAULT_CONNECTION : connectionName);
            }
        }

        public void Rollback(BLManager blManager, string connectionName = null)
        {
            Tuple<BLManager, IDbTransaction> _DBTransactionTuple = GetTransactionTuple(connectionName);
            if (_DBTransactionTuple != null && blManager == _DBTransactionTuple.Item1)
            {
                _DBTransactionTuple.Item2.Rollback();
                _DBTransactionTuple.Item2.Dispose();
                _DBTransactions.Remove(connectionName == null ? DEFAULT_CONNECTION : connectionName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commit">If commit=false then rollback will be executed</param>
        private void End(bool commit = true)
        {
            foreach (Tuple<BLManager, IDbTransaction> item in _DBTransactions.Values)
            {
                if (commit) item.Item2.Commit();
                else item.Item2.Rollback();
                item.Item2.Dispose();
            }
            _DBTransactions.Clear();

            foreach (IDbConnection item in _DBConnections.Values)
            {
                item.Close();
                item.Dispose();
            }
            _DBConnections.Clear();
            //if (_DbTransaction != null)
            //{
            //    if (commit) _DbTransaction.Commit();
            //    else _DbTransaction.Rollback();

            //    _DbTransaction.Dispose();
            //    _DbTransaction = null;
            //}
            //if (_DBConnection != null)
            //{
            //    _DBConnection.Close();
            //    _DBConnection.Dispose();
            //    _DBConnection = null;
            //}
        }
        public void Dispose()
        {
            End(false);
        }

        /// <summary>
        /// Writes SQL command to pretty executable string.
        /// </summary>
        /// <param name="CMD">Sql command</param>
        /// <returns></returns>
        public static string ToString(SqlCommand CMD)
        {

            if (CMD.CommandType != CommandType.StoredProcedure)
            {
                string s = CMD.CommandText;
                for (int i = 0; i < CMD.Parameters.Count; i++) s = s.Replace(CMD.Parameters[i].ParameterName, Convert.ToString(CMD.Parameters[i].Value));
                return s;
            }
            else
            {
                string s = "EXEC " + CMD.CommandText + " ";
                for (int i = 0; i < CMD.Parameters.Count; i++) s += CMD.Parameters[i].ParameterName + "='" + Convert.ToString(CMD.Parameters[i].Value) + "',";
                return s;
            }
        }

        public static string ToStringParams(DataTable queryParameters)
        {
            StringBuilder paramsLog = new StringBuilder();
            string delimiter = "par";
            int i = 1;
            foreach (DataRow dataRow in queryParameters.Rows)
            {
                paramsLog.Append($"{delimiter}{i}='{dataRow["par_value"]}'");
                //deimiter = "|";
                if (!delimiter.Contains(","))
                {
                    delimiter = ", " + delimiter;
                }
                i++;
            }
            //Common.Log("[DB] " + Common.ToString(sqlCommand));
            return paramsLog.ToString();
        }
    }
}