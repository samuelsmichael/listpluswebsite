using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Web;
using System.IO;
using System.Data.Common;

namespace CommonServerSide {
    public class VersionControlManager {
        private static string VERSION_SESSIONID = "LMNOPQRST_VERSIONCONTROL";
        public static int BYTE_SIZE = 1048576;
        private HttpServerUtility _server;
        public VersionControlManager(HttpServerUtility server) {
            _server = server;
        }
        public void VersionUpdate() {

            string databaseName = string.Empty;
            string connectionString = CommonMethods.ConnectionString;
            string[] nameValuePairs = connectionString.Split(new char[] { ';' });
            foreach (string nameValuePair in nameValuePairs) {
                if (nameValuePair.ToLower().IndexOf("database") != -1) {
                    string[] nameValue = nameValuePair.Split(new char[] { '=' });
                    databaseName = nameValue[1];
                    break;
                }
            }
            string version = string.Empty;
            DataSet ds = CommonMethods.getDataSet("uspsystemcontrolget", null, null, null, VERSION_SESSIONID);
            try {
                version = ds.Tables[0].Rows[0]["Version"].ToString();
            } catch {}

            switch (version) {
                case "2009_01_b2":
                    // here's where we build the next version.
                    break;
                default:
                    doUpdateVersion_01_2009_b2(databaseName);
                    break;
            }
        }
        private string getDataFromFile(string fileName) {
            string path = _server.MapPath("~/");
            string filespec = path + @"\" + fileName;
            StreamReader sr=null;
            try {
                sr=new StreamReader(filespec);
                return sr.ReadToEnd();
            } finally {
                try {
                    sr.Close();
                } catch { }
            }
        }
        private string deriveDelimiter(int index, string str, ref int whiteSpaceAfterDelimiter) {
            StringBuilder sb = new StringBuilder();
            for (int c = 10+index; c < str.Length; c++) {
                char ch = str[c];
                if (char.IsWhiteSpace(ch)) {
                    whiteSpaceAfterDelimiter = c;
                    return sb.ToString();
                }
                sb.Append(str[c]);
            }
            throw new Exception("Invalid parsing of script.  No DELIMITER.");
        }
        private void heresTheNextStatement(string stmt, string currentDelimiter, string leftToDo) {
            int index33 = stmt.IndexOf("CREATE PROCEDURE `propertyinvestors`.`uspuserset`");
            if(index33<10 && index33!=-1) {
                int bkhere333=1;
            }
            if (!stmt.Trim().Equals(string.Empty)) {
                CommonMethods.executeNonQuery(stmt, VERSION_SESSIONID);
            } else {
                if (leftToDo.Trim().Equals(string.Empty)) {
                    return;
                }
            }
            int indexOfDelimiterStatement = leftToDo.IndexOf("DELIMITER");
            int indexOfDelimiter = leftToDo.IndexOf(currentDelimiter);
            if (indexOfDelimiter == -1) {
                int bkhere33 = 33;
            }
            if (indexOfDelimiterStatement == -1 && indexOfDelimiter == -1) {
                return;
            }
            if (indexOfDelimiterStatement < indexOfDelimiter && indexOfDelimiter!=-1 && indexOfDelimiterStatement!=-1) {
                int whiteSpaceAfterDelimiter = -1;
                currentDelimiter = deriveDelimiter(indexOfDelimiterStatement, leftToDo, ref whiteSpaceAfterDelimiter);
                heresTheNextStatement(string.Empty, currentDelimiter, leftToDo.Substring(whiteSpaceAfterDelimiter));
            } else {
                try {
                    string leftToDoString = string.Empty;
                    try {
                        leftToDoString = leftToDo.Substring(indexOfDelimiter + currentDelimiter.Length);
                    } catch { }
                    heresTheNextStatement(leftToDo.Substring(0, indexOfDelimiter), currentDelimiter, leftToDoString);
                } catch (Exception eiei) {
                    int bkhere = 3;
                }
            }
        }
        private void executeMySQLScript(string script) {
            heresTheNextStatement(string.Empty, ";", script);
        }
        private void updateSystem(string fileName,string dbName) {
            try {
                executeMySQLScript(getDataFromFile(fileName).Replace("&&databasename",dbName));
            } catch (Exception ee) {
                int bkhere = 3;
            }
        }
        private void doUpdateVersion_01_2009_b2(string databaseName) {
            DataSet ds = CommonMethods.getDataSet("uspuserget",null,null,null, VERSION_SESSIONID);
            updateSystem("Version__01_2009_b2__Part1of2.sql",databaseName);
            List<DbParameter> parameters = new List<DbParameter>();
            foreach (DataRow dr in ds.Tables[0].Rows) {
                parameters.Clear();
                parameters.Add(CommonMethods.buildUInt32Parm("useridparm",Convert.ToInt32(dr["UserId"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildStringParm("emailparm",DDCommon.CommonRoutines.ObjectToStringV2(dr["Email"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildStringParm("phoneparm",DDCommon.CommonRoutines.ObjectToStringV2(dr["Phone"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildStringParm("companynameparm",DDCommon.CommonRoutines.ObjectToStringV2(dr["CompanyName"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildStringParm("firstnameparm",DDCommon.CommonRoutines.ObjectToStringV2(dr["FirstName"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildStringParm("lastnameparm",DDCommon.CommonRoutines.ObjectToStringV2(dr["LastName"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildBooleanParm("isspouseparm",0,ParameterDirection.Input));
                parameters.Add(CommonMethods.buildUInt32Parm("userpersonidparm",DDCommon.CommonRoutines.NULL_INT,ParameterDirection.Output));
                CommonMethods.executeNonQuery("uspuserpersoninsert",parameters,VERSION_SESSIONID);
                if (!DDCommon.CommonRoutines.isNothing(dr["SpouseFirstName"]) || !DDCommon.CommonRoutines.isNothing(dr["SpouseLastName"])) {
                    parameters.Clear();
                    parameters.Add(CommonMethods.buildUInt32Parm("useridparm", Convert.ToInt32(dr["UserId"]), ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildStringParm("emailparm", string.Empty, ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildStringParm("phoneparm", string.Empty, ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildStringParm("companynameparm", string.Empty, ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildStringParm("firstnameparm", DDCommon.CommonRoutines.ObjectToStringV2(dr["SpouseFirstName"]), ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildStringParm("lastnameparm", DDCommon.CommonRoutines.ObjectToStringV2(dr["SpouseLastName"]), ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildBooleanParm("isspouseparm", 1, ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildUInt32Parm("userpersonidparm", DDCommon.CommonRoutines.NULL_INT, ParameterDirection.Output));
                    CommonMethods.executeNonQuery("uspuserpersoninsert",parameters, VERSION_SESSIONID);
                }
            }
            updateSystem("Version__01_2009_b2__Part2of2.sql",databaseName);
        }
        public static void dodah() {
            string databasename="irrofcolorad";
            DataSet ds = CommonMethods.getDataSet("uspuserget",null,null,null, VERSION_SESSIONID);
            List<DbParameter> parameters = new List<DbParameter>();
            foreach (DataRow dr in ds.Tables[0].Rows) {
                parameters.Clear();
                parameters.Add(CommonMethods.buildUInt32Parm("useridparm",Convert.ToInt32(dr["UserId"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildStringParm("emailparm",DDCommon.CommonRoutines.ObjectToStringV2(dr["Email"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildStringParm("phoneparm",DDCommon.CommonRoutines.ObjectToStringV2(dr["Phone"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildStringParm("companynameparm",DDCommon.CommonRoutines.ObjectToStringV2(dr["CompanyName"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildStringParm("firstnameparm",DDCommon.CommonRoutines.ObjectToStringV2(dr["FirstName"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildStringParm("lastnameparm",DDCommon.CommonRoutines.ObjectToStringV2(dr["LastName"]),ParameterDirection.Input));
                parameters.Add(CommonMethods.buildBooleanParm("isspouseparm",0,ParameterDirection.Input));
                CommonMethods.executeNonQuery("uspuserpersoninsert",parameters,VERSION_SESSIONID);
                if (!DDCommon.CommonRoutines.isNothing(dr["SpouseFirstName"]) || !DDCommon.CommonRoutines.isNothing(dr["SpouseLastName"])) {
                    parameters.Clear();
                    parameters.Add(CommonMethods.buildUInt32Parm("useridparm", Convert.ToInt32(dr["UserId"]), ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildStringParm("emailparm", string.Empty, ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildStringParm("phoneparm", string.Empty, ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildStringParm("companynameparm", string.Empty, ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildStringParm("firstnameparm", DDCommon.CommonRoutines.ObjectToStringV2(dr["SpouseFirstName"]), ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildStringParm("lastnameparm", DDCommon.CommonRoutines.ObjectToStringV2(dr["SpouseLastName"]), ParameterDirection.Input));
                    parameters.Add(CommonMethods.buildBooleanParm("isspouseparm", 1, ParameterDirection.Input));
                    CommonMethods.executeNonQuery("uspuserpersoninsert",parameters, VERSION_SESSIONID);
                    int x = 3;
                } 
            }
        }
    }
}
