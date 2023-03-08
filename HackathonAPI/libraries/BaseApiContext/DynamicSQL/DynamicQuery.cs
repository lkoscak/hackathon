using WebApi.Core.Commons;
using WebApi.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebApi.Core.DB;

namespace WebApi.Core.DynamicSQL
{
    public class DynamicQuery
    {
        readonly ILoggerManager _loggerManager;
        public DynamicQuery(ILoggerManager loggerManager)
        {
            _loggerManager = loggerManager;
        }

        public SqlDataReader GetData(SqlConnection sqlConnection, string sessionKey, int cmd, params object[] queryParameters)
        {
            return (SqlDataReader)Execute(sqlConnection, null, sessionKey, cmd, 1, false, queryParameters);
        }

        //TODO Šprem: vjerojatno je bolje da se ovako dohvaćaju podaci...
        public async Task<SqlDataReader> GetDataAsync(SqlConnection sqlConnection, string sessionKey, int cmd, params object[] queryParameters)
        {
            return (SqlDataReader)(await ExecuteAsync(sqlConnection, null, sessionKey, cmd, 1, false, false, queryParameters));
        }

        public Task<SqlDataReader> GetDataAsync(SqlConnection connection, SqlTransaction transaction, string sessionKey, int v1, object p1, object prl_obj_id, object prl_usr_id, object prl_ext_id, object prl_desc, object p2, object p3, int v2)
        {
            throw new NotImplementedException();
        }

        public SqlDataReader GetData(SqlConnection sqlConnection, SqlTransaction t, string sessionKey, int cmd, params object[] queryParameters)
        {
            return (SqlDataReader)Execute(sqlConnection, t, sessionKey, cmd, 1, false, queryParameters);
        }

        public async Task<SqlDataReader> GetDataAsync(SqlConnection sqlConnection, SqlTransaction t, string sessionKey, int cmd, params object[] queryParameters)
        {
            return (SqlDataReader) await ExecuteAsync(sqlConnection, t, sessionKey, cmd, 1, false, false, queryParameters);
        }

        public async Task<SqlDataReader> GetDataAsync(SqlConnection sqlConnection, SqlTransaction t, string sessionKey, int cmd, bool useNullValues, params object[] queryParameters)
        {
            return (SqlDataReader) await ExecuteAsync(sqlConnection, t, sessionKey, cmd, 1, false, useNullValues, queryParameters);
        }

        public SqlDataReader GetMultipleData(SqlConnection sqlConnection, string sessionKey, int cmd, params object[] queryParameters)
        {
            return (SqlDataReader)Execute(sqlConnection, null, sessionKey, cmd, 3, false, queryParameters);
        }

        public int UpdateData(SqlConnection sqlConnection, string sessionKey, int cmd, params object[] queryParameters)
        {
            return ((int)Execute(sqlConnection, null, sessionKey, cmd, 2, false, queryParameters));
        }

        public async Task<int> UpdateDataAsync(SqlConnection sqlConnection, string sessionKey, int cmd, params object[] queryParameters)
        {
            return ((int)await ExecuteAsync(sqlConnection, null, sessionKey, cmd, 2, false, false, queryParameters));
        }

        public async Task<int> UpdateDataAsync(SqlConnection sqlConnection, SqlTransaction t, string sessionKey, int cmd, bool useNullValues, params object[] queryParameters)
        {
            return ((int)await ExecuteAsync(sqlConnection, t, sessionKey, cmd, 2, false, useNullValues, queryParameters));
        }

        public int UpdateData(SqlConnection sqlConnection, SqlTransaction t, string sessionKey, int cmd, params object[] queryParameters)
        {
            return ((int)Execute(sqlConnection, t, sessionKey, cmd, 2, false, queryParameters));
        }

        private object Execute(SqlConnection sqlConnection, SqlTransaction t, string sessionKey, int cmd, int cmdRange, bool printQueryNames, params object[] queryParameters)
        {
            DataTable queryParametersDataTable = CreateDynamicQueryDataTable(false, queryParameters);
            return ExecuteDb(sqlConnection, t, sessionKey, cmd, cmdRange, printQueryNames, queryParametersDataTable);
        }

        private async Task<object> ExecuteAsync(SqlConnection sqlConnection, SqlTransaction t, string sessionKey, int cmd, int cmdRange, bool printQueryNames, bool useNullValues = false, params object[] queryParameters)
        {
            DataTable queryParametersDataTable = CreateDynamicQueryDataTable(useNullValues, queryParameters);
            return await ExecuteDbAsync(sqlConnection, t, sessionKey, cmd, cmdRange, printQueryNames, queryParametersDataTable);
        }

        private object ExecuteDb(SqlConnection sqlConnection, SqlTransaction t, string sessionKey, int cmd, int cmdRange, bool printQueryNames, DataTable queryParameters)
        {
            try
            {
                if (!IsQueryParametersDataTable(queryParameters)) throw new Exception("Wrong DataTable type for queryParameters");

                //Get user time zone
                MobilisisCommon40.Timezone.UserTimezone userTimezone = null;
                try { MobilisisCommon40.Timezone.UserTimezone.LoadUserTimezone(sqlConnection, sessionKey); } catch { }

                //Set to invariant culture
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

                //Execute Dynamic Query procedure
                using (SqlCommand sqlCommand = new SqlCommand("dbo.DynamicQuery", sqlConnection, t))
                {
                    SqlParameter queryParametersTableParameter = new SqlParameter("@queryParameters", SqlDbType.Structured)
                    {
                        TypeName = "dbo.DynamicQueryParameters",
                        Value = queryParameters,
                        Direction = ParameterDirection.Input
                    };

                    SqlParameter outputParameter = new SqlParameter("returnValue", SqlDbType.Int);
                    outputParameter.Direction = ParameterDirection.ReturnValue;

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@key", SqlDbType.Char).Value = sessionKey;
                    sqlCommand.Parameters.Add("@cmd", SqlDbType.Int).Value = cmd;
                    sqlCommand.Parameters.Add("@type", SqlDbType.Int).Value = cmdRange;
                    sqlCommand.Parameters.Add("@printQueryNames", SqlDbType.TinyInt).Value = printQueryNames ? 1 : 0;
                    sqlCommand.Parameters.Add("@userDatetimeOffset", SqlDbType.DateTimeOffset).Value = userTimezone != null ? userTimezone.GetUserDateTimeOffset() : DateTimeOffset.Now;

                    sqlCommand.Parameters.Add(queryParametersTableParameter);
                    sqlCommand.CommandTimeout = 40000;

                    if (cmd != 3892)
                    {
                        //Common.Log("[DB] " + Common.ToString(sqlCommand));
                        //_loggerManager.debug("[DB] " + Common.ToString(sqlCommand));
                        _loggerManager.debug("[DB] " + DBManager.ToString(sqlCommand) + " " + DBManager.ToStringParams(queryParameters));
                    }

                    if (cmdRange == 2)
                    {
                        sqlCommand.Parameters.Add(outputParameter);
                        sqlCommand.ExecuteNonQuery();
                        return Convert.ToInt32(outputParameter.Value);
                    }
                    else
                    {
                        return sqlCommand.ExecuteReader();
                    }
                }
            }
            catch (SqlException e)
            {
                if (e.Number < 50000)
                {
                    _loggerManager.error(e, $"Error in DynamicQuery, SessionKey:{sessionKey}, CMD:{cmd} , Type: {(cmdRange == 1 ? "GET" : "SET")}");
                    throw e;
                }
            }
            catch (Exception e)
            {
                _loggerManager.error(e, $"Error in DynamicQuery, SessionKey:{sessionKey}, CMD:{cmd} , Type: {(cmdRange == 1 ? "GET" : "SET")}");
                throw e;
            }

            return null;
        }

        //TODO Refaktorirati svakako...
        private async Task<object> ExecuteDbAsync(SqlConnection sqlConnection, SqlTransaction t, string sessionKey, int cmd, int cmdRange, bool printQueryNames, DataTable queryParameters)
        {
            try
            {
                if (!IsQueryParametersDataTable(queryParameters)) throw new Exception("Wrong DataTable type for queryParameters");

                //Get user time zone
                MobilisisCommon40.Timezone.UserTimezone userTimezone = null;
                try { MobilisisCommon40.Timezone.UserTimezone.LoadUserTimezone(sqlConnection, sessionKey); } catch { }

                //Set to invariant culture
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

                //Execute Dynamic Query procedure
                using (SqlCommand sqlCommand = new SqlCommand("dbo.DynamicQuery", sqlConnection, t))
                {
                    SqlParameter queryParametersTableParameter = new SqlParameter("@queryParameters", SqlDbType.Structured)
                    {
                        TypeName = "dbo.DynamicQueryParameters",
                        Value = queryParameters,
                        Direction = ParameterDirection.Input
                    };

                    SqlParameter outputParameter = new SqlParameter("returnValue", SqlDbType.Int);
                    outputParameter.Direction = ParameterDirection.ReturnValue;

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@key", SqlDbType.Char).Value = sessionKey;
                    sqlCommand.Parameters.Add("@cmd", SqlDbType.Int).Value = cmd;
                    sqlCommand.Parameters.Add("@type", SqlDbType.Int).Value = cmdRange;
                    sqlCommand.Parameters.Add("@printQueryNames", SqlDbType.TinyInt).Value = printQueryNames ? 1 : 0;
                    sqlCommand.Parameters.Add("@userDatetimeOffset", SqlDbType.DateTimeOffset).Value = userTimezone != null ? userTimezone.GetUserDateTimeOffset() : DateTimeOffset.Now;

                    sqlCommand.Parameters.Add(queryParametersTableParameter);
                    sqlCommand.CommandTimeout = 40000;

                    if (cmd != 3892)
                    {
                        //Common.Log("[DB] " + Common.ToString(sqlCommand));
                        //_loggerManager.debug("[DB] " + Common.ToString(sqlCommand));
                        _loggerManager.debug("[DB] " + DBManager.ToString(sqlCommand) + " " + DBManager.ToStringParams(queryParameters));
                    }

                    if (cmdRange == 2)
                    {
                        sqlCommand.Parameters.Add(outputParameter);
                        await sqlCommand.ExecuteNonQueryAsync();
                        return Convert.ToInt32(outputParameter.Value);
                    }
                    else
                    {
                        return await sqlCommand.ExecuteReaderAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0)
                {
                    _loggerManager.error(ex, $"Error in DynamicQuery, SessionKey:{sessionKey}, CMD:{cmd} , Type: {(cmdRange == 1 ? "GET" : "SET")}");
                    throw ex;
                }
            }
            catch (Exception e)
            {
                _loggerManager.error(e, $"Error in DynamicQuery, SessionKey:{sessionKey}, CMD:{cmd} , Type: {(cmdRange == 1 ? "GET" : "SET")}");
                throw e;
            }

            return null;
        }

        private bool IsQueryParametersDataTable(DataTable queryParameters)
        {
            bool autoIncrementColumnExists = false,
                 parameterValueColumnExists = false;
            if (queryParameters.TableName != "dbo.DynamicQueryParameters") return false;

            if (queryParameters.Columns.Count != 2) return false;

            foreach (DataColumn tmpColumn in queryParameters.Columns)
            {
                switch (tmpColumn.ColumnName)
                {
                    case "par_index":
                        autoIncrementColumnExists = tmpColumn.AutoIncrement;
                        break;
                    case "par_value":
                        parameterValueColumnExists = true;
                        break;
                }
            }

            return autoIncrementColumnExists && parameterValueColumnExists;
        }

        private DataTable CreateEmptyDataTable()
        {
            DataTable dynamicQueryDataTable = new DataTable("dbo.DynamicQueryParameters");
            DataColumn parameterIndexColumn = new DataColumn("par_index", System.Type.GetType("System.Int32"));
            parameterIndexColumn.AutoIncrement = true;
            parameterIndexColumn.AutoIncrementSeed = 1;
            parameterIndexColumn.AutoIncrementStep = 1;
            parameterIndexColumn.Unique = true;

            dynamicQueryDataTable.Columns.Add(parameterIndexColumn);
            dynamicQueryDataTable.Columns.Add("par_value", typeof(string));

            return dynamicQueryDataTable;
        }

        public DataTable CreateDynamicQueryDataTable(bool useNullValues = false, params object[] queryParameters)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            DataTable dynamicQueryDataTable = CreateEmptyDataTable();

            for (int qPos = 0, qLength = queryParameters.Length; qPos < qLength; qPos++)
            {
                DataRow tmpDataRow = dynamicQueryDataTable.NewRow();

                if (queryParameters[qPos] != null)
                {
                    if (queryParameters[qPos].GetType() == typeof(DateTimeOffset))
                    {
                        tmpDataRow["par_value"] = ((DateTimeOffset)queryParameters[qPos]).ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz");
                    }
                    else
                    {
                        tmpDataRow["par_value"] = queryParameters[qPos].ToString();
                    }
                }
                else if (!useNullValues)
                {
                    tmpDataRow["par_value"] = "";
                }

                dynamicQueryDataTable.Rows.Add(tmpDataRow);
            }

            return dynamicQueryDataTable;
        }

        public DataTable CreateDynamicQueryDataTable(List<string> queryParameters)
        {
            DataTable dynamicQueryDataTable = CreateEmptyDataTable();

            for (int qPos = 0, qLength = queryParameters != null ? queryParameters.Count : 0; qPos < qLength; qPos++)
            {
                DataRow tmpDataRow = dynamicQueryDataTable.NewRow();
                tmpDataRow["par_value"] = queryParameters[qPos].ToString();

                dynamicQueryDataTable.Rows.Add(tmpDataRow);
            }

            return dynamicQueryDataTable;
        }

    }
}