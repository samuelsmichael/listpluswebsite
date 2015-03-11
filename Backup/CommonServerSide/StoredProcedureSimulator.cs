using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.Common;

namespace CommonServerSide {
    public abstract class StoredProcedureSimulator {
        protected List<DbParameter> _sqlParameterCollection;
        public StoredProcedureSimulator(List<DbParameter> sqlParms) {
            _sqlParameterCollection = sqlParms;
        }
        protected DbParameter getParameterWhoseNameIs(string name) {
            foreach (DbParameter param in _sqlParameterCollection) {
                if (param.ParameterName.ToLower().Equals(name.ToLower())) {
                    return param;
                }
            }
            throw new Exception("Parameter " + name + " not found.");
        }
        public static string sprocNameToClassName(string sprocName) {
            return sprocName.Substring(0, 1).ToUpper() + sprocName.Substring(1);
        }
        public static StoredProcedureSimulator buildClassInstance(string sproc, List<DbParameter> sqlParms) {
            Type tp = new CommonMethods().GetType();
            Assembly assembly = tp.Assembly;
            Type type;
            type = assembly.GetType("CommonServerSide." + sprocNameToClassName(sproc));
            object[] paramValues = new object[1];
            paramValues[0] = sqlParms;
            System.Type[] constructorName = new System.Type[1];
            constructorName[0] = typeof(List<DbParameter>);
            ConstructorInfo c = type.GetConstructor(constructorName);
            if (c == null) {
                throw new Exception("Unable to find constructor for " + sproc);
            }
            return (StoredProcedureSimulator)c.Invoke(paramValues);
        }
    }
    public abstract class StoredProcedureExecuteNonQuery : StoredProcedureSimulator {
        protected abstract string getCmdString(string sessionId);
        public virtual void executeNonQuery(string sessionId) {
            CommonMethods.executeNonQuery(getCmdString(sessionId),sessionId);
        }
        public StoredProcedureExecuteNonQuery(List<DbParameter> sqlParameterCollection) : base(sqlParameterCollection) {
        }
        protected bool recordExists(string tblName, string keyName, string keyValue, string sessionId) {
            try {
               return CommonMethods.getDataSet("SELECT * FROM " + tblName + " WHERE " + keyName + "='" + keyValue + "'", sessionId).Tables[0].Rows.Count>0;
            } catch {
                return false;
            }
        }
        private string buildFieldNames() {
            string comma = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (DbParameter parm in _sqlParameterCollection) {
                sb.Append(comma + parm.ParameterName);
                comma = ",";
            }
            return sb.ToString();
        }
        protected string createAnInsertStatementFor(string tblName) {
            return
                "INSERT INTO " + tblName + " (" +
                buildFieldNames() +
                ") VALUES (" +
                CommonMethods.buildValuesString(_sqlParameterCollection) +
                ")";
        }
        private string buildSetStatements() {
            string comma = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (DbParameter parm in _sqlParameterCollection) {
                sb.Append(comma+"SET "+parm.ParameterName+"="+CommonMethods.deriveSqlCommandValue(parm));
            }
            return sb.ToString();
        }
        protected string createAnUpdateStatementFor(string tblName) {
            return
                "UPDATE " + tblName + buildSetStatements();
        }
    }
    public abstract class StoredProcedureExecuteNonQueryWithOutputParameter : StoredProcedureExecuteNonQuery {
        protected abstract void plugOutputValue();
        public StoredProcedureExecuteNonQueryWithOutputParameter(List<DbParameter> sqlParameterCollection)
            : base(sqlParameterCollection) {
        }
        public override void executeNonQuery(string sessionId) {
            base.executeNonQuery(sessionId);
            plugOutputValue();
        }
    }
    public abstract class StoredProcedureGetDataSet : StoredProcedureSimulator {
        public static StoredProcedureGetDataSet buildClassInstance(string sproc, string selectClause, string whereClause, string orderByClause) {
            int countParms = 0;
            if (selectClause != null) countParms++;
            if (orderByClause != null) countParms++;
            if (whereClause != null) countParms++;
            Type tp = new CommonMethods().GetType();
            Assembly assembly = tp.Assembly;
            Type type;
            type = assembly.GetType("CommonServerSide." + sprocNameToClassName(sproc));
            object[] paramValues = new object[countParms];
            paramValues[0] = selectClause;
            if (orderByClause != null) paramValues[1] = whereClause;
            if (orderByClause != null) paramValues[2] = orderByClause;
            System.Type[] constructorName = new System.Type[countParms];
            constructorName[0] = typeof(String);
            if (whereClause != null) constructorName[1] = typeof(String);
            if (orderByClause != null) constructorName[2] = typeof(String);
            ConstructorInfo c = type.GetConstructor(constructorName);
            if (c == null) {
                throw new Exception("Unable to find constructor for " + sproc);
            }
            return (StoredProcedureGetDataSet)c.Invoke(paramValues);
        }
        protected abstract string ChildSelectClause { get;}
        protected abstract string ChildWhereClause { get;}
        protected abstract string ChildOrderByClause { get;}
        protected string _whereClause = null;
        protected string _selectClause = null;
        protected string _orderByClause = null;
        public StoredProcedureGetDataSet(List<DbParameter> sqlParameterCollection)
            : base(sqlParameterCollection) {
        }
        public DataSet getDataSet(string sessionId) {
            string cmdText = string.Empty;
            if (_selectClause != null) {
                cmdText = _selectClause;
            } else {
                if (!DDCommon.CommonRoutines.isNothing(ChildSelectClause)) {
                    cmdText = ChildSelectClause;
                }
            }
            if (_whereClause != null) {
                cmdText += " WHERE " + _whereClause;
            } else {
                if (!DDCommon.CommonRoutines.isNothing(ChildWhereClause)) {
                    cmdText += " WHERE " + ChildWhereClause;
                }
            }
            if (_orderByClause != null) {
                cmdText += " ORDER BY " + _orderByClause;
            } else {
                if (!DDCommon.CommonRoutines.isNothing(ChildOrderByClause)) {
                    cmdText += " ORDER BY " + ChildOrderByClause;
                }
            }
            return CommonMethods.getDataSet(cmdText,sessionId);
        }
    }
#region gets
    public class UspGetAllRoles : StoredProcedureGetDataSet {
        public UspGetAllRoles(List<DbParameter> parms)
            : base(parms) {
        }
        protected override string ChildSelectClause {
            get { return "SELECT * FROM tblRoles"; }
        }
        protected override string ChildWhereClause {
            get {
                return null;
            }
        }
        protected override string ChildOrderByClause {
            get {
                return null;
            }
        }
    }
    public class UspGetRoleByRoleCode : StoredProcedureGetDataSet {
        public UspGetRoleByRoleCode(List<DbParameter> parms)
            : base(parms) {
        }
        protected override string ChildSelectClause {
            get { return "SELECT * FROM tblAdminUserRoles"; }
        }
        protected override string ChildWhereClause {
            get {
                return "RoleCode='" + getParameterWhoseNameIs("RoleCode") + "'"; 
            }
        }
        protected override string ChildOrderByClause {
            get {
                return null;
            }
        }
    }
#endregion
    #region puts
    public class UspPutRole : StoredProcedureExecuteNonQuery {
        public UspPutRole(List<DbParameter> dbParameters)
            : base(dbParameters) {
        }
        protected override string getCmdString (string sessionId) {
                if(recordExists("tblRole","RoleCode", getParameterWhoseNameIs("RoleCode").Value.ToString(),sessionId)) {
                    return createAnUpdateStatementFor("uspAdminPRole");
                } else {
                    return createAnInsertStatementFor("uspAdminPRole");
                }
        }
    }
    #endregion
}
