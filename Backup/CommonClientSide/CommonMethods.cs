using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Configuration;
using System.Web;
using DDCommon;

namespace CommonClientSide {
    public class CommonMethods {
        public static string PaymentURL {
            get {
                return ConfigurationManager.AppSettings["PaymentURL"];
            }
        }
        public static bool isNullDate(DateTime dateTime) {
            return CommonServerSide.CommonMethods.isNullDate(dateTime);
        }

        public static string getParmGoTo(SessionVariableManager svm,string currentPage) {
            return svm.ParmGoToByPage[currentPage];
        }
        public static void setParmGoTo(SessionVariableManager svm, string currentPage,string newValue) {
            svm.ParmGoToByPage[currentPage] = newValue;
        }
        public static DataSet getDataSet(string sproc, string selectString, string whereString, string orderByString, string sessionID) {
            return CommonServerSide.CommonMethods.getDataSet(sproc, selectString, whereString, orderByString, sessionID);
        }
        public static void executeNonQuery(string sproc, List<DbParameter> parameters, string sessionID) {
            CommonServerSide.CommonMethods.executeNonQuery(sproc, parameters, sessionID);
        }
        public static DbParameter buildStringParm(string parmName, string value, ParameterDirection parmDirection) {
            return CommonServerSide.CommonMethods.buildStringParm(parmName, value, parmDirection);
        }

        public static DbParameter buildParm(string parmName, object value, ParameterDirection parmDirection, DbType dbType) {
            return CommonServerSide.CommonMethods.buildParm(parmName, value, parmDirection,dbType);
        }
        public static DbParameter buildUInt32Parm(string parmName, int value, ParameterDirection parmDirection) {
            return CommonServerSide.CommonMethods.buildUInt32Parm(parmName, value, parmDirection);
        }
        public static DbParameter buildDateTimeParm(string parmName, DateTime value, ParameterDirection parmDirection) {
            return CommonServerSide.CommonMethods.buildDateTimeParm(parmName, value, parmDirection);
        }
        public static DbParameter buildDecimalParm(string parmName, object value, ParameterDirection parmDirection) {
            return CommonServerSide.CommonMethods.buildDecimalParm(parmName, value, parmDirection);
        }

        public static bool isTestMode() {
            return ConfigurationManager.AppSettings["IsTestMode"] != null &&
                ConfigurationManager.AppSettings["IsTestMode"].ToLower().Equals("true");
        }
        public static DataSet getDataSet(string selectClause, string sessionID) {
            return CommonServerSide.CommonMethods.getDataSet(selectClause, sessionID);
        }
        public static DataSet getDataSet(string sproc, List<DbParameter> parameters, string sessionID) {
            return CommonServerSide.CommonMethods.getDataSet(sproc, parameters, sessionID);
        }

        public static DbParameter buildBooleanParm(string parmName, int value, ParameterDirection parmDirection) {
            return CommonServerSide.CommonMethods.buildBooleanParm(parmName, value, parmDirection);
        }
        public static DbParameter buildBooleanParm(string parmName, bool value, ParameterDirection parmDirection) {
            return CommonServerSide.CommonMethods.buildBooleanParm(parmName, value?1:0, parmDirection);
        }
        public static string getConnectionString(string name) {
            return CommonServerSide.CommonMethods.getConnectionString(name);
        }
        public static string dateToString(DateTime dateTime) {
            return CommonServerSide.CommonMethods.dateToString(dateTime);
        }
        public static void executeNonQuery(string nonQuery, string sessionID) {
            CommonServerSide.CommonMethods.executeNonQuery(nonQuery, sessionID);
        }
        public static string ConnectionString {
            get {
                return CommonServerSide.CommonMethods.ConnectionString;
            }
        }
        public static void updateDatabaseVersion(HttpServerUtility server) {
            new CommonServerSide.VersionControlManager(server).VersionUpdate();
        }
        public static void setCache(System.Web.Caching.Cache cache) {
            CommonServerSide.CommonMethods.setCache(cache);
        }
        public static void dodah() {
            CommonServerSide.VersionControlManager.dodah();
        }
        public static DataSet getDataSetALaOrion(string id, string sessionId, string showOther) {
            return CommonServerSide.CommonMethods.getDataSetALaOrion(id, sessionId,showOther);
        }
        public static string getDescForDataSetALaOrion(string id, string sessionId, string value) {
            return CommonServerSide.CommonMethods.getDescForDataSetALaOrion(id, sessionId, value);
        }
        public static string getValueForDataSetALaOrionWhoseDescIs(string id, string sessionId, string desc) {
            return CommonServerSide.CommonMethods.getValueForDataSetALaOrionWhoseDescIs(id, sessionId, desc);
        }
    }
}
