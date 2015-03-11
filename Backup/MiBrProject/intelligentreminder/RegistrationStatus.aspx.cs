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


public partial class RegistrationStatus : InternetWebPage {
    public static string FD = "^~^";
    public RegistrationStatus() {
        Load += new EventHandler(InboundTraffic_Load);
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

    void InboundTraffic_Load(object sender, EventArgs e) {
        DateTime rightNow = DateTime.Now;
        StreamReader sr = null;
        int cnt = 0;
        try {
            sr = new StreamReader(Request.InputStream);
            string myPhoneID = getMeMyNextField(sr);

            db("myPhoneID", myPhoneID);


            List<DbParameter> parms = new List<DbParameter>();
            parms.Add(CommonMethods.buildStringParm("phoneidparm", myPhoneID, ParameterDirection.Input));
            CommonMethods.executeNonQuery("uspdojduser", parms, svm.SessionID);
            parms = new List<DbParameter>();
            parms.Add(CommonMethods.buildStringParm("phoneidparm",myPhoneID,ParameterDirection.Input));
            DataSet ds=CommonMethods.getDataSet("uspgetregistrationstatus",parms,svm.SessionID);
            if(ds.Tables.Count==0 || ds.Tables[0].Rows.Count==0) {
                parms = new List<DbParameter>();
                parms.Add(CommonMethods.buildStringParm("phoneidparm", myPhoneID, ParameterDirection.Input));
                ds = CommonMethods.getDataSet("uspgetjduser", parms, svm.SessionID);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) {
                    Response.Write("TrialPeriodEnding");
                    Response.Write(FD);
                    Response.Write("This is a trial version. The non-trial version only costs $1.99. We have lots of ideas for new features to add, but will not be able to do this without your support.");
                    Response.Write(FD);
                } else {
                    DateTime dateStarted = DDCommon.CommonRoutines.ObjectToDateTime(ds.Tables[0].Rows[0]["datecreated"]);
                    TimeSpan ts = DateTime.Now - dateStarted;
                    if (ts.Days > 10) {
                        Response.Write("TrialPeriodEnding");
                        Response.Write(FD);
                        Response.Write("Your trial version is now " + ts.Days + " days old. The non-trial version only costs $1.99. We have lots of ideas for new features to add, but will not be able to do this without your support.");
                        Response.Write(FD);
                    } else {
                        Response.Write("NotOk");
                        Response.Write(FD);
                        Response.Write("No registration");
                        Response.Write(FD);
                    }
                    parms = new List<DbParameter>();
                    ds = CommonMethods.getDataSet("uspgetads", parms, svm.SessionID);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >= 2) {
                        int c = 0;
                        Response.Write("YesAds");
                        Response.Write(FD);
                        while (c < 2) {
                            Response.Write(ds.Tables[0].Rows[c]["adtext"] + "|" + ds.Tables[0].Rows[c]["aduri"]);
                            Response.Write(FD);
                            c++;
                        }
                    } else {
                        Response.Write("NoAdds");
                        Response.Write(FD);
                    }
                    parms = new List<DbParameter>();
                    ds = CommonMethods.getDataSet("uspgetsystemcontrol", parms, svm.SessionID);
                    Response.Write(ds.Tables[0].Rows[0]["doviewcount"].ToString());
                    Response.Write(FD);
                }
            } else {
                if (DDCommon.CommonRoutines.isNothing(ds.Tables[0].Rows[0]["dateauthorized"])) {
                    Response.Write("Ok");
                    Response.Write(FD);
                } else {
                    if(DDCommon.CommonRoutines.ObjectToStringV2(ds.Tables[0].Rows[0]["authorizationstatus"]).ToLower().Equals("ok")) {
                        Response.Write("Ok");
                        Response.Write(FD);
                    } else {
                        Response.Write("NotOk");
                        Response.Write(FD);
                        Response.Write(DDCommon.CommonRoutines.ObjectToStringV2(ds.Tables[0].Rows[0]["authorizationstatus"]));
                        Response.Write(FD);
                    }
                }
            }
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
