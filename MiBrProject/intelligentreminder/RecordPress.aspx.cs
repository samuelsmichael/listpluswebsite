using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CommonClientSide;


public partial class RecordPress : InternetWebPage {
    public static string FD = "^~^";
    public RecordPress() {
        Load += new EventHandler(RecordPress_Load);
    }
    char[] ca = new char[4096];
    private bool _weredone = false;
    private String _str = "";
    private String _swk = "";
    private string needMoreFromReader(StreamReader sr) {
        int cnt = sr.Read(ca, 0, 4096);
        if (cnt > 0) {
            char[] ca1 = new char[cnt];
            for (int c = 0; c < cnt; c++) {
                ca1[c] = ca[c];
            }
            return new string(ca1);
        } else {
            _weredone = true;
        }
        return string.Empty;
    }
    private string getMeMyNextField(StreamReader sr) {
        if (_str.Length == 0) {
            _str = needMoreFromReader(sr);
        }
        if (_str.Length == 0) {
            _weredone = true;
            return null;
        } else {
            int index3 = _str.IndexOf(FD);
            if (index3 == -1) {
                _swk = needMoreFromReader(sr);
                if (_swk.Length == 0) {
                    _swk = _str;
                    _str = "";
                    return _swk;
                } else {
                    _str = _str + _swk;
                }
                index3 = _str.IndexOf(FD);
            }
            _swk = _str.Substring(0, index3);
            _str = _str.Substring(index3 + 3);
            return _swk;
        }
    }
    private void db(string str, object obj) {
   //    Response.Write("|" + str + ":" + obj.ToString());
    }

    void RecordPress_Load(object sender, EventArgs e) {
        DateTime rightNow = DateTime.Now;
        StreamReader sr = null;
        int cnt = 0;
        try {
            sr = new StreamReader(Request.InputStream);
            string myAddNbr = getMeMyNextField(sr);

            List<DbParameter> parms = new List<DbParameter>();
            parms.Add(CommonMethods.buildUInt32Parm("addnbrparm", Convert.ToInt32(myAddNbr), ParameterDirection.Input));
            CommonMethods.executeNonQuery("uspincrementpress", parms, svm.SessionID);
            Response.Write("Ok");
            Response.Write(FD);
        } catch (Exception eee3) {
            Response.Write(eee3.Message);
        }
        finally {
            try { sr.Close(); } catch { }
            Response.End();
        }
    }
    protected override void child_Page_Load(object sender, EventArgs e) {

    }
}
