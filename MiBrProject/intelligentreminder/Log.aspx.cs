using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.IO;
using CommonClientSide;
using CommonServerSide;

public partial class Log : InternetWebPage {

    public static string FD = "^~^";
    public Log() {
        Load += new EventHandler(Log_Load);
    }

    protected override void child_Page_Load(object sender, EventArgs e) { }
    void Log_Load(object sender, EventArgs e) {

        StringBuilder sb = new StringBuilder();
        sb.Append("Log"+FD);
        StreamReader sr = null;
        StreamWriter sw = null;
        StreamWriter swRaw = null;
        try {
            char[] ca = new char[4096];
            sr = new System.IO.StreamReader(Request.InputStream);
            int cnt = sr.Read(ca,0,4096);
            sb.Append("cnt=" + cnt + FD);
            char[] cabs = new char[cnt];
            for (int c = 0; c < cnt; c++) {
                cabs[c] = ca[c];
            }
            string str = new String(cabs);

//            Directory.CreateDirectory(Server.MapPath("~/logfiles/"));
            string filespecRaw = Server.MapPath("~/intelligentreminder/logfiles/" +
                                "RawLog" + "_" + DDCommon.CommonRoutines.dateInCCYYMMDDHHMMSSFormat(DateTime.Now) + ".log");
            swRaw = new StreamWriter(filespecRaw, false);
            swRaw.WriteLine(str);
            
            sb.Append("str=" + str + FD);
            int index3 = str.IndexOf(FD);
            
            string sa1 = str.Substring(0, index3);
            string sa2 = str.Substring(index3 + 3);

            string filespec = Server.MapPath("~/intelligentreminder/logfiles/" +
                                sa1 + "_" + DDCommon.CommonRoutines.dateInCCYYMMDDHHMMSSFormat(DateTime.Now)+".log");
            sw=new StreamWriter(filespec,true);

            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(CommonServerSide.CommonMethods.buildStringParm(
                "phoneidparm", sa1, ParameterDirection.Input));
            parameters.Add(CommonServerSide.CommonMethods.buildStringParm(
                "filespecparm",filespec,ParameterDirection.Input));
            //DbParameter returnParm = CommonServerSide.CommonMethods.buildUInt32Parm(
            //    "logidparm", DDCommon.CommonRoutines.NULL_INT, ParameterDirection.Output);
            //parameters.Add(returnParm); 
            DataSet ds=CommonServerSide.CommonMethods.getDataSet("uspcreatelogentry", parameters, svm.SessionID);

            int logID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            cnt = sa2.Length;
            int cnt2=cnt;
            
            if (cnt > 0) {
                do {
                    
                    sw.Write(sa2);
                    cnt = sr.Read(ca,0,4096);
                    if (cnt > 0) {
                        cnt2 += cnt;
                        cabs = new char[cnt];
                        for (int c = 0; c < cnt; c++) {
                            cabs[c] = ca[c];
                        }
                        sa2 = new string(cabs);
                    }
                } while (cnt > 0);
            }
            
            Response.Write("ok"+"|"+cnt2);

        } catch (Exception e2) {
            Response.Write("notok" + FD + "Log="+sb.ToString()+FD+"msg="+e2.Message);
        } finally {
            try { sr.Close(); } catch { }
            try { sw.Close(); } catch {}
            try { swRaw.Close(); } catch { }
        }
        Response.End();
    }
}
