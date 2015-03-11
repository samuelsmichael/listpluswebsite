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


public partial class InboundTraffic : InternetWebPage {
    public static string FD = "^~^";
    public InboundTraffic() {
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
            string[] hisIDs = getMeMyNextField(sr).Split(new char[1] { ';' });

            db("myPhoneID", myPhoneID);
            db("hisIDs.size", hisIDs.Length.ToString());
            db("hisIDs[0]", hisIDs[0]);


            List<DbParameter> parms = new List<DbParameter>();
            foreach (string hisId in hisIDs) {



                parms.Clear();
                parms.Add(CommonMethods.buildStringParm("phoneidpullerparm", myPhoneID, ParameterDirection.Input));
                parms.Add(CommonMethods.buildStringParm("phoneidpusherparm", hisId, ParameterDirection.Input));
                DataSet ds = CommonMethods.getDataSet("uspLatestDateForPhoneIDs", parms, svm.SessionID);

                String sql = "SELECT DateCreated FROM phoneidassociations " +
                                " where phoneidreceiver='" + myPhoneID + "' and phoneidsender='" + hisId + "' " +
                                " order by DateCreated desc limit 1;";



                db("# records", ds.Tables[0].Rows.Count.ToString());
                try { db("Rows[0][0]", ds.Tables[0].Rows[0][0].ToString()); } catch { }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0 &&DDCommon.CommonRoutines.isNothingNot(ds.Tables[0].Rows[0][0])) {
                    DateTime zDate = DDCommon.CommonRoutines.ObjectToDateTime(ds.Tables[0].Rows[0][0]);
                    List<DbParameter> par2 = new List<DbParameter>();
                    par2.Add(CommonMethods.buildStringParm("phoneidpusher", hisId, ParameterDirection.Input));
                    db("HisId=", hisId);
                    par2.Add(CommonMethods.buildDateTimeParm("DateCreated", zDate, ParameterDirection.Input));
                    DataSet ds2 = CommonMethods.getDataSet("uspLatestRecords", par2, svm.SessionID);
                    if (ds2.Tables[0].Rows.Count == 0) {
                        Response.Write(hisId);
                        Response.Write(FD);
                    } else {
                        foreach (DataRow dr in ds2.Tables[0].Rows) {
                            Response.Write(dr["phoneid"].ToString());
                            Response.Write(FD);
                            Response.Write(dr["nameitem"].ToString());
                            Response.Write(FD);
                            Response.Write(dr["description"].ToString());
                            Response.Write(FD);
                            Response.Write(dr["namelocation"].ToString());
                            Response.Write(FD);
                            Response.Write(dr["address"].ToString());
                            Response.Write(FD);
                            Response.Write(dr["latitude"].ToString());
                            Response.Write(FD);
                            Response.Write(dr["longitude"].ToString());
                            Response.Write(FD);
                            Response.Write(dr["notificationdx"].ToString());
                            Response.Write(FD);
                            Response.Write(dr["fidneed"].ToString());
                            Response.Write(FD);
                            Response.Write(dr["fiditem"].ToString());
                            Response.Write(FD);
                            Response.Write(dr["fidlocation"].ToString());
                            Response.Write(FD);
                            Response.Write(dr["company"].ToString());
                            Response.Write(FD);
                        }
                    }
                }
            }
            /* If there are any "deleted on belhalf of" then return them here */
            parms.Clear();
            parms.Add(CommonMethods.buildStringParm("foreignphoneidparm", myPhoneID, ParameterDirection.Input));
            DataSet ds3 = CommonMethods.getDataSet("uspgetneedsdeletedonbehalfof", parms, svm.SessionID);
            if (ds3.Tables != null && ds3.Tables[0].Rows != null && ds3.Tables[0].Rows.Count > 0) {
                Response.Write("**##@@");
                Response.Write(FD);
                foreach (DataRow dr2 in ds3.Tables[0].Rows) {
                    Response.Write(dr2["foreignneedid"].ToString());
                    Response.Write(FD);
                    Response.Write(dr2["localphoneid"].ToString());
                    Response.Write(FD);
                    object foreignLocationId = dr2["foreignlocationid"];
                    if (DDCommon.CommonRoutines.isNothingNot(foreignLocationId)) {
                        Response.Write(foreignLocationId.ToString());
                        Response.Write(FD);
                    }
                }
            }
            parms.Clear();
            parms.Add(CommonMethods.buildStringParm("foreignphoneidparm", myPhoneID, ParameterDirection.Input));
            CommonMethods.executeNonQuery("uspdeleteneedsdeletedonbehalfof", parms, svm.SessionID);
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
