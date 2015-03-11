using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.Caching;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace CommonServerSide {
    public class CommonMethods {
        public static int NULL_INT { 
            get {
                return DDCommon.CommonRoutines.NULL_INT;
            }
        }
        public static bool SqlServerAllowsSprocs {
            get {
                return ConfigurationManager.AppSettings["SQLServerAllowsSprocs"] != null &&
                    ConfigurationManager.AppSettings["SQLServerAllowsSprocs"].ToLower().Equals("true");
            }
        }
        public static DateTime NULL_DATETIME {
            get {
            return DDCommon.CommonRoutines.NULL_DATETIME;}}
        private static Cache _cache;
        public static string ConnectionString {
            get {
                return getConnectionString(ConfigurationManager.AppSettings["DatabaseProvider"]);
            }
        }
        public static bool useSQLCommandLine() {
            return ConfigurationManager.AppSettings["UseSQLCommandLines"] != null &&
                ConfigurationManager.AppSettings["UseSQLCommandLines"].ToLower().Equals("true");
        }
        private static string DatabaseProvider {
            get {
                return getDatabaseProvider(ConfigurationManager.AppSettings["DatabaseProvider"]);
            }
        }
        public static DbParameter buildStringParm(string parmName, string value, ParameterDirection parmDirection) {
            return buildParm(parmName, value, parmDirection, DbType.String);
        }
        public static DbParameter buildTextParm(string parmName, string value, ParameterDirection parmDirection) {
            return buildParm(parmName, value, parmDirection, DbType.Object);
        }
        public static DbParameter buildStringParm(string parmName, string value, ParameterDirection parmDirection, int size) {
            return buildParm(parmName, value, parmDirection, DbType.String, size);
        }
        public static bool isNullDate(DateTime dateTime) {
            switch (DatabaseProvider) {
                case "MySql.Data.MySqlClient":
                    return dateTime.Year == 1;
                case "System.Data.SqlClient":
                    return new System.Data.SqlTypes.SqlDateTime(dateTime).IsNull;
                default:
                    throw new Exception("isNullDate(..) not supported for this Database Provider: " + DatabaseProvider);
            }
        }
        public static string dateToString(DateTime dateTime) {
            switch (DatabaseProvider) {
                case "MySql.Data.MySqlClient":
                    return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                case "System.Data.SqlClient":
                    return dateTime.ToString();
                default:
                    throw new Exception("dateToString(..) not supported for this Database Provider: " + DatabaseProvider);
            }
        }
        public static DbParameter buildIntParm(string parmName, int value, ParameterDirection parmDirection) {
            return buildintParm(parmName, value, parmDirection);
        }
        public static DbParameter buildintParm(string parmName, int value, ParameterDirection parmDirection) {
            switch (DatabaseProvider) {
                case "MySql.Data.MySqlClient":
                    MySqlParameter parm = new MySqlParameter(parmName, MySqlDbType.Int32);
                    parm.Value = value;
                    parm.Direction = parmDirection;
                    return parm;
                case "System.Data.SqlClient":
                    SqlParameter parm2 = new SqlParameter(parmName, SqlDbType.Int);
                    if (value == NULL_INT) {
                        parm2.Value = null;
                    } else {
                        parm2.Value = value;
                    }
                    parm2.Direction = parmDirection;
                    return parm2;
                default:
                    throw new Exception("buildUInt32Parm(..) not supported for this Database Provider: " + DatabaseProvider);
            }
        }
        public static DbParameter buildUInt32Parm(string parmName, int value, ParameterDirection parmDirection) {
            switch (DatabaseProvider) {
                case "MySql.Data.MySqlClient":
                    MySqlParameter parm = new MySqlParameter(parmName, MySqlDbType.UInt32);
                    parm.Value = value;
                    parm.Direction = parmDirection;
                    return parm;
                case "System.Data.SqlClient":
                    SqlParameter parm2= new SqlParameter(parmName, SqlDbType.Int);
                    if (value == NULL_INT) {
                        parm2.Value = null;
                    } else {
                        parm2.Value = value;
                    }
                    parm2.Direction = parmDirection;
                    return parm2;
                default:
                    throw new Exception("buildUInt32Parm(..) not supported for this Database Provider: " + DatabaseProvider);
            }
        }
        public static DbParameter builddatetimeParm(string parmName, DateTime value, ParameterDirection parmDirection) {
            return buildDateTimeParm(parmName, value, parmDirection);
        }
        public static DbParameter buildDateTimeParm(string parmName, DateTime value, ParameterDirection parmDirection) {
            switch (DatabaseProvider) {
                case "MySql.Data.MySqlClient":
                    MySqlParameter parm = new MySqlParameter(parmName, MySqlDbType.DateTime);
                    if (value == DateTime.MinValue || value==null) {
                        parm.Value = null;
                    } else {
                        parm.Value = value;
                    }
                    parm.Direction = parmDirection;
                    return parm;
                case "System.Data.SqlClient":
                    SqlParameter parm2 = new SqlParameter(parmName, SqlDbType.DateTime);
                    if (value == DDCommon.CommonRoutines.NULL_DATETIME) {
                        parm2.Value = null;
                    } else {
                        parm2.Value = value;
                    }
                    parm2.Direction = parmDirection;
                    return parm2;
                default:
                    throw new Exception("buildDateTimeParm(..) not supported for this Database Provider: " + DatabaseProvider);
            }
        }
        public static DbParameter buildbitParm(string parmName, bool value, ParameterDirection parmDirection) {
            return buildBooleanParm(parmName, value, parmDirection);
        }
        public static DbParameter buildBooleanParm(string parmName, bool value, ParameterDirection parmDirection) {
            return buildBooleanParm(parmName,
                value ? 1 : 0,
                parmDirection);
        }
        public static DbParameter buildbitParm(string parmName, int value, ParameterDirection parmDirection) {
            return buildBooleanParm(parmName, value, parmDirection);
        }
        public static DbParameter buildBooleanParm(string parmName, object value, ParameterDirection parmDirection) {
            return buildBooleanParm(parmName,
                value is DBNull ? NULL_INT : (DDCommon.CommonRoutines.ObjectToBool(value)?1:0),
                parmDirection);
        }

        public static DbParameter buildBooleanParm(string parmName, int value, ParameterDirection parmDirection) {
            switch (DatabaseProvider) {
                case "MySql.Data.MySqlClient":
                    MySqlParameter parm = new MySqlParameter(parmName, MySqlDbType.Bit);
                    if (value == DDCommon.CommonRoutines.NULL_INT) {
                        parm.Value = null;
                    } else {
                        parm.Value = value;
                    }
                    parm.Direction = parmDirection;
                    return parm;
                case "System.Data.SqlClient":
                    SqlParameter parm2 = new SqlParameter(parmName, SqlDbType.Bit);
                    if (value == DDCommon.CommonRoutines.NULL_INT) {
                        parm2.Value = null;
                    } else {
                        parm2.Value = value;
                    }
                    parm2.Direction = parmDirection;
                    return parm2;
                default:
                    throw new Exception("buildBooleanParm(..) not supported for this Database Provider: " + DatabaseProvider);
            }
        }
        public static DbParameter buildDecimalParm(string parmName, object value, ParameterDirection parmDirection) {
            if (value is Decimal && ((Decimal)value) == DDCommon.CommonRoutines.NULL_DECIMAL) {
                value = null;
            } else {
                value = DDCommon.CommonRoutines.ObjectToDecimal(value);
            }
            return buildParm(parmName, value, parmDirection, DbType.Decimal);
        }
        public static DbParameter builddecimalParm(string parmName, object value, ParameterDirection parmDirection) {
            return buildDecimalParm(parmName, value, parmDirection);
        }
        public static DbParameter buildParm(string parmName, object value, ParameterDirection parmDirection, DbType dbType) {
            return buildParm(parmName, value, parmDirection, dbType, DDCommon.CommonRoutines.NULL_INT);
        }
        public static DbParameter buildParm(string parmName, object value, ParameterDirection parmDirection, DbType dbType, int size) {
            switch (DatabaseProvider) {
                case "MySql.Data.MySqlClient":
                    MySqlParameter parm = new MySqlParameter(parmName, 
                        dbType.Equals(DbType.String)?MySql.Data.MySqlClient.MySqlDbType.String:(
                        dbType.Equals(DbType.Object)?MySql.Data.MySqlClient.MySqlDbType.Text:(
                        dbType.Equals(DbType.Int32)?MySqlDbType.Int32:(
                        dbType.Equals(DbType.Double)?MySqlDbType.Double:(
                        dbType.Equals(DbType.DateTime)?MySqlDbType.DateTime:(
                        dbType.Equals(DbType.Boolean)?MySqlDbType.Bit:(
                        dbType.Equals(DbType.UInt32)?MySqlDbType.UInt32:(
                        dbType.Equals(DbType.Decimal)?MySqlDbType.Decimal:(
                            MySqlDbType.String
                        )))))))));
                    parm.Value = value;
                    parm.Direction = parmDirection;
                    return parm;
                case "System.Data.SqlClient":
                    SqlParameter parm2 = new SqlParameter(parmName,
                        dbType.Equals(DbType.String) ? SqlDbType.VarChar : (
                        dbType.Equals(DbType.Object) ? SqlDbType.Text : (
                        dbType.Equals(DbType.Int32) ? SqlDbType.Int : (
                        dbType.Equals(DbType.Double) ? SqlDbType.BigInt : (
                        dbType.Equals(DbType.DateTime) ? SqlDbType.DateTime : (
                        dbType.Equals(DbType.Boolean) ? SqlDbType.Bit : (
                        dbType.Equals(DbType.UInt32) ? SqlDbType.Int : (
                        dbType.Equals(DbType.Decimal) ? SqlDbType.Decimal : (
                            SqlDbType.VarChar
                        )))))))));
                    parm2.Value = value;
                    parm2.Direction = parmDirection;
                    if (size != DDCommon.CommonRoutines.NULL_INT) {
                        parm2.Size = size;
                    }
                    return parm2;
                default:
                    throw new Exception("buildParm(..) not supported for this Database Provider: " + DatabaseProvider);
            }
        }
        public static string getConnectionString(string name) {
            ConnectionStringSettings settings;
            settings =
                ConfigurationManager.ConnectionStrings[name];
            return settings.ConnectionString;
        }
        private static string getDatabaseProvider(string name) {
            ConnectionStringSettings settings;
            settings =
                ConfigurationManager.ConnectionStrings[name];
            return settings.ProviderName;
        }
        private static System.Data.Common.DbConnection getSqlConnection() {
            lock (DDCommon.CommonRoutines._lockingObject) {
                switch (DatabaseProvider) {
                    case "MySql.Data.MySqlClient":
                        MySqlConnection connection = new MySqlConnection(ConnectionString);
                        if (connection.State != ConnectionState.Open) {
                            int x = 10;
                            while (x > 0) {
                                try {
                                    if (connection.State == ConnectionState.Open) {
                                        break;
                                    }
                                    connection.Open();
                                    break;
                                } catch (Exception ee) {
                                    if (x == 1) {
                                        throw ee;
                                    }
                                }
                                x--;
                            }
                        }
                        return connection;
                    case "System.Data.SqlClient":
                        SqlConnection connection2 = new SqlConnection(ConnectionString);
                        if (connection2.State != ConnectionState.Open) {
                            int x = 10;
                            while (x > 0) {
                                try {
                                    if (connection2.State == ConnectionState.Open) {
                                        break;
                                    }
                                    connection2.Open();
                                    break;
                                } catch (Exception ee) {
                                    if (x == 1) {
                                        throw ee;
                                    }
                                }
                                x--;
                            }
                        }
                        return connection2;
                    default:
                        throw new Exception("getSqlConnection(..) not supported for this Database Provider: " + DatabaseProvider);
                }
            }
        }
        private static void CacheItemRemovedCallbackMethod(
            string key,
            Object value,
            CacheItemRemovedReason reason) {
            try {
                ((DbConnection)value).Close();
            } catch { }
        }
        private static DbCommand getCommand(string sessionId) {
            switch (DatabaseProvider) {
                case "MySql.Data.MySqlClient":
                    MySqlCommand command = new MySqlCommand();
                    command.Connection=(MySqlConnection)getConnectionForSessionId(sessionId);
                    return command;
                case "System.Data.SqlClient":
                    SqlCommand command2 = new SqlCommand();
                    command2.Connection = (SqlConnection)getConnectionForSessionId(sessionId);
                    return command2;
                default:
                    throw new Exception("getCommand(..) not supported for this Database Provider: " + DatabaseProvider);
            }
        }
        private static DbCommand getCommandForSelect(string commandText, string sessionId) {
            DbCommand command = getCommand(sessionId);
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            return command;
        }

        private static DbCommand getCommandForSproc(string sproc, string sessiodId) {
            DbCommand command = getCommand(sessiodId);
            command.CommandText = sproc;
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }
        private static DbConnection getConnectionForSessionId(string sessionId) {
            if (DDCommon.CommonRoutines.forceANewConnection) {
                if (_cache["Connections" + "_" + sessionId] != null) {
                    try { ((DbConnection)_cache["Connections" + "_" + sessionId]).Close(); } catch { }
                    _cache.Remove("Connections" + "_" + sessionId);
                }
            }
            if (_cache["Connections"+"_"+sessionId] == null) {
                _cache.Add(
                    "Connections" + "_" + sessionId, getSqlConnection(), null, Cache.NoAbsoluteExpiration,
                    new TimeSpan(0, 5, 0), CacheItemPriority.AboveNormal, new CacheItemRemovedCallback(CacheItemRemovedCallbackMethod));
            }
            DbConnection connection=(DbConnection)_cache["Connections" + "_" + sessionId];
            if (connection.State != ConnectionState.Open) {
                int x = 10;
                while (x > 0) {
                    try {
                        connection.Open();
                        break;
                    } catch (Exception ewe) {
                        if (x == 1) {
                            throw ewe;
                        }
                    }
                    x--;
                }
            }
            return connection;
        }
        public static DataAdapter getAdapter(DbCommand command) {
            switch (DatabaseProvider) {
                case "MySql.Data.MySqlClient":
                    MySqlDataAdapter da = new MySqlDataAdapter((MySqlCommand)command);
                    return da;
                case "System.Data.SqlClient":
                    SqlDataAdapter da2 = new SqlDataAdapter((SqlCommand)command);
                    return da2;
                default:
                    throw new Exception("getAdapter(..) not supported for this Database Provider: " + DatabaseProvider);
            }
        }
        public static void setCache(Cache cache) {
            _cache = cache;
        }
        public static DataSet getDataSet(string sproc, string selectString, string whereString, string orderByString, string sessionID) {
            lock (DDCommon.CommonRoutines._lockingObject) {
                DbCommand command = getCommandForSproc(sproc, sessionID);
                DbParameter parm = buildStringParm("SelectClause", selectString, ParameterDirection.Input);
                command.Parameters.Add(parm);
                parm = buildStringParm("WhereClause", whereString, ParameterDirection.Input);
                command.Parameters.Add(parm);
                parm = buildStringParm("OrderByClause", orderByString, ParameterDirection.Input);
                command.Parameters.Add(parm);
                if (useSQLCommandLine()) {
                    List<DbParameter> parmList = new List<DbParameter>();
                    foreach (DbParameter dbParm in command.Parameters) {
                        parmList.Add(dbParm);
                    }
                    return executeNonQueryCommandStatement(sproc, parmList, sessionID);
                } else {
                    if (!SqlServerAllowsSprocs) {
                        StoredProcedureGetDataSet sp = StoredProcedureGetDataSet.buildClassInstance(sproc, selectString, whereString, orderByString);
                        return sp.getDataSet(sessionID);
                    } else {
                        DataAdapter da = getAdapter(command);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
        }
        private static string discernMySQL(object value) {
            switch (DatabaseProvider) {
                case "MySql.Data.MySqlClient":
                    if (value.GetType().ToString().ToLower().IndexOf("string") >= 0) {
                        return value.ToString();
                    } else {
                        if (value.GetType().ToString().ToLower().IndexOf("date") >= 0) {
                            return dateToString((DateTime)value);
                        } else {
                            if (value.GetType().ToString().ToLower().IndexOf("int") >= 0) {
                                return value.ToString();
                            } else {
                                if (value.GetType().ToString().ToLower().IndexOf("decimal") >= 0) {
                                    return value.ToString();
                                } else {
                                    throw new Exception("Unexpected error in routine: discernMySQL");
                                }
                            }
                        }
                    }
                    break;
                case "System.Data.SqlClient":
                    return value.ToString();
                default:
                    throw new Exception("executeNonQueryCommandStatement not supported for DatabaseProvider: " + DatabaseProvider);
            }
        }
        public static string deriveSqlCommandValue(DbParameter parm) {
            if (parm.Value == null) {
                return "null";
            } else {
                if (
                    parm.DbType.ToString().ToLower().IndexOf("string") >= 0 ||
            parm.DbType.ToString().ToLower().IndexOf("date") >= 0
            ) {
                    if (parm.Value == null) {
                        return "''";
                    } else {
                        return "'" + DDCommon.CommonRoutines.doubleQuotify(discernMySQL(parm.Value)) + "'";
                    }
                } else {
                    if (parm.Value == null) {
                        return string.Empty;
                    } else {
                        return parm.Value.ToString();
                    }
                }
            }
        }
        public static string buildValuesString(List<DbParameter> parameters) {
            string comma = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            foreach (DbParameter parm in parameters) {
                sb.Append(comma);
                if (parm.Direction != ParameterDirection.Output) {
                    sb.Append(deriveSqlCommandValue(parm));
                } else {
                    sb.Append("@nada");
                }
                comma = ",";
            }
            sb.Append(")");
            return sb.ToString();
        }
        private static string deriveMySqlCommandText(string sproc, List<DbParameter> parameters) {
            string execVsCall = string.Empty;
            switch (DatabaseProvider) {
                case "MySql.Data.MySqlClient":
                    execVsCall = "CALL";
                    break;
                case "System.Data.SqlClient":
                    execVsCall = "EXEC";
                    break;
                default:
                    throw new Exception("deriveMySqlCommandText not supported for DatabaseProvider: " + DatabaseProvider);
            }
            return execVsCall + " " + sproc + buildValuesString(parameters);
        }
        private static void fillReturnParamIfAny(List<DbParameter> parameters, DataSet ds) {
            foreach (DbParameter param in parameters) {
                if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput) {
                    param.Value = ds.Tables[0].Rows[0][0];
                }
            }
        }
        private static DataSet executeNonQueryCommandStatement(string sproc, List<DbParameter> parameters, string sessionID) {
            lock (DDCommon.CommonRoutines._lockingObject) {
                DbCommand command = getCommandForSproc(sproc, sessionID);
                command.CommandType = CommandType.Text;
                command.CommandText = deriveMySqlCommandText(sproc, parameters);
                DataAdapter da = getAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds);
                fillReturnParamIfAny(parameters, ds);
                return ds;
            }
        }

        public static DataSet executeQuery(string sproc, List<DbParameter> parameters, string sessionID) {
            lock (DDCommon.CommonRoutines._lockingObject) {
                if (useSQLCommandLine()) {
                    return executeNonQueryCommandStatement(sproc, parameters, sessionID);
                } else {

                    switch (DatabaseProvider) {
                        case "MySql.Data.MySqlClient":
                            MySqlCommand command = new MySqlCommand(sproc, (MySqlConnection)getConnectionForSessionId(sessionID));
                            if (parameters != null) {
                                foreach (DbParameter parm in parameters) {
                                    command.Parameters.Add(parm);
                                }
                            }
                            MySqlDataAdapter da = new MySqlDataAdapter(command);
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            return ds;
                        case "System.Data.SqlClient":
                            SqlCommand command2 = new SqlCommand(sproc, (SqlConnection)getConnectionForSessionId(sessionID));
                            if (parameters != null) {
                                foreach (DbParameter parm in parameters) {
                                    command2.Parameters.Add(parm);
                                }
                            }
                            SqlDataAdapter da2 = new SqlDataAdapter(command2);
                            DataSet ds2 = new DataSet();
                            da2.Fill(ds2);
                            return ds2;
                        default:
                            throw new Exception("executeQuery(...) not supported for DatabaseProvider: " + DatabaseProvider);
                    }
                }
            }
        }
        public static void executeNonQuery(string sproc, List<DbParameter> parameters, string sessionID) {
            if (useSQLCommandLine()) {
                executeNonQueryCommandStatement(sproc, parameters, sessionID);
            } else {
                if (!SqlServerAllowsSprocs) {
                    StoredProcedureExecuteNonQuery sp = (StoredProcedureExecuteNonQuery)StoredProcedureSimulator.buildClassInstance(sproc, parameters);
                    sp.executeNonQuery(sessionID);
                } else {
                    DbCommand command = getCommandForSproc(sproc, sessionID);
                    if (parameters != null) {
                        foreach (DbParameter parm in parameters) {
                            command.Parameters.Add(parm);
                        }
                    }
                    command.CommandTimeout = 999999;
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void executeNonQuery(string nonQuery, string sessionID) {
            DbCommand command=getCommand(sessionID);
            command.CommandText = nonQuery;
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
        }

        public static DataSet getDataSet(string selectClause, string sessionID) {
            lock (DDCommon.CommonRoutines._lockingObject) {
                DbCommand command = getCommandForSelect(selectClause, sessionID);
                DataAdapter da = getAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
        public static DataSet getDataSet(string sproc, List<DbParameter> parameters, string sessionID) {
            lock (DDCommon.CommonRoutines._lockingObject) {
                DbCommand command = getCommandForSproc(sproc, sessionID);
                if (parameters != null) {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                if (useSQLCommandLine()) {
                    List<DbParameter> parmList = new List<DbParameter>();
                    foreach (DbParameter dbParm in command.Parameters) {
                        parmList.Add(dbParm);
                    }
                    return executeNonQueryCommandStatement(sproc, parmList, sessionID);
                } else {
                    if (!SqlServerAllowsSprocs) {
                        StoredProcedureGetDataSet sp = (StoredProcedureGetDataSet)StoredProcedureGetDataSet.buildClassInstance(sproc, parameters);
                        return sp.getDataSet(sessionID);
                    } else {
                        command.CommandTimeout = 11560;
                        DataAdapter da = getAdapter(command);
                        DataSet ds = new DataSet();
                        
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
        }
        public static string getValueForDataSetALaOrionWhoseDescIs(string id, string sessionId, string desc) {
            string descToLower = desc.ToLower();
            DataSet ds = getDataSetALaOrion(id, sessionId, null);
            foreach (DataRow dr in ds.Tables[0].Rows) {
                if (dr["desc"].ToString().ToLower().Equals(descToLower)) {
                    return (string)dr["value"];
                }
            }
            throw new Exception("Not found");
        }

        public static string getDescForDataSetALaOrion(string id, string sessionId, string value) {
            try {
                DataSet ds = getDataSetALaOrion(id, sessionId, null);
                return
                    ds.Tables[0].Rows.Find(value)["desc"].ToString();
            } catch {
                return string.Empty;
            }
        }
        public static DataSet getDataSetALaOrion(string id, string sessionId, string showOther) {
            lock (DDCommon.CommonRoutines._lockingObject) {
                List<DbParameter> parms = new List<DbParameter>();
                parms.Add(buildStringParm("@lookupcode", id, ParameterDirection.Input));
                DataSet ds = getDataSet("uspSysGValuePairs", parms, sessionId);
                try {
                    ds.Tables[0].PrimaryKey = new DataColumn[1] { ds.Tables[0].Columns["value"] };
                } catch {
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                    if (showOther != null) {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["value"] = "";
                        dr["desc"] = showOther;
                        ds.Tables[0].Rows.InsertAt(dr, 0);
                    }
                }
                return ds;
            }
        }
    }
}
