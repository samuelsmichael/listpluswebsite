using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CommonClientSide;
using System.IO;

public partial class OutboundTraffic : InternetWebPage {
    public static string FD = "^~^";
    public OutboundTraffic() {
        Load += new EventHandler(OutboundTraffic_Load);
    }
    char[] ca = new char[4096];
    private bool _weredone=false;
    private String _str="";
    private String _swk="";
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
        if(_str.Length==0) {
            _str=needMoreFromReader(sr);
        }
        if(_str.Length==0) {
            _weredone=true;
            return null;
        } else {
            int index3=_str.IndexOf(FD);
            if(index3==-1) {
                _swk=needMoreFromReader(sr);
                if(_swk.Length==0) {
                    _swk=_str;
                    _str="";
                    return _swk;
                } else {
                    _str=_str+_swk;
                }
                index3=_str.IndexOf(FD);
            }
            _swk=_str.Substring(0,index3);
            _str=_str.Substring(index3+3);
            return _swk;
        }
    }

    private void db(string str, object obj) {
        Response.Write("|" + str + ":" + obj.ToString());
    }
    DateTime RightNow {
        get {
            if (Session["RN"] == null) {
                Session["RN"] = DateTime.Now;
            }
            return (DateTime)Session["RN"];
        }
    }
    void OutboundTraffic_Load(object sender, EventArgs e) {
        DateTime RightNow = DateTime.Now;
        StreamReader sr = null;
        int cnt = 0;
        try {
            sr = new StreamReader(Request.InputStream);
            string myPhoneID = getMeMyNextField(sr);
            List<DbParameter> parms2 = new List<DbParameter>();
            parms2.Clear();
            parms2.Add(CommonMethods.buildStringParm("phoneidparm", myPhoneID, ParameterDirection.Input));
            CommonMethods.executeNonQuery("uspclearrecordsforphoneid", parms2, svm.SessionID);
            string[] hisIDs = getMeMyNextField(sr).Split(new char[1] { ';' });
            List<DbParameter> parms = new List<DbParameter>();
            foreach (string hisId in hisIDs) {
                parms.Clear();
                parms.Add(CommonMethods.buildStringParm("phoneidsenderparm", myPhoneID, ParameterDirection.Input));
                parms.Add(CommonMethods.buildStringParm("phoneidreceiverparm", hisId, ParameterDirection.Input));
                parms.Add(CommonMethods.buildDateTimeParm("rightnow", RightNow, ParameterDirection.Input));
                CommonMethods.executeNonQuery("uspcreatephoneidassociations", parms, svm.SessionID);
            }
            while (!_weredone) {
                int antecedentneedid = DDCommon.CommonRoutines.ObjectToInt(getMeMyNextField(sr));
                if (antecedentneedid != 0) {
                    int antecedentlocationid = DDCommon.CommonRoutines.ObjectToInt(getMeMyNextField(sr));
                    getMeMyNextField(sr); /// bypass a duplicate needid
                    string needDescription = getMeMyNextField(sr);
                    getMeMyNextField(sr); // bypass a duplicate phoneid
                    getMeMyNextField(sr); // bypass a duplicate locationid
                    string locationName = getMeMyNextField(sr);
                    //                db("locationMame", locationName);
                    string address = getMeMyNextField(sr);
                    //              db("address", address);
                    string latitude = getMeMyNextField(sr);
                    string longitude = getMeMyNextField(sr);
                    int notificationdx = DDCommon.CommonRoutines.ObjectToInt(getMeMyNextField(sr));
                    int antecedentitemid = DDCommon.CommonRoutines.ObjectToInt(getMeMyNextField(sr));
                    string itemName = getMeMyNextField(sr);
                    //            db("itemMame", itemName);
                    string company = getMeMyNextField(sr);
                    parms = new List<DbParameter>();

                    parms.Add(CommonMethods.buildStringParm("phoneidparm", myPhoneID, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildUInt32Parm("antecedentitemidparm", antecedentitemid, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildStringParm("nameparm", itemName, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildDateTimeParm("rightnow", RightNow, ParameterDirection.Input));
                    DataSet ds = CommonMethods.getDataSet("uspcreateitem", parms, svm.SessionID);
                    int itemID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    parms = new List<DbParameter>();
                    parms.Add(CommonMethods.buildStringParm("phoneidparm", myPhoneID, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildUInt32Parm("antecedentlocationidparm", antecedentlocationid, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildStringParm("nameparm", locationName, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildStringParm("addressparm", address, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildStringParm("latitudeparm", latitude, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildStringParm("longitudeparm", longitude, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildUInt32Parm("notificationdxparm", notificationdx, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildDateTimeParm("rightnow", RightNow, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildStringParm("companyparm", locationName, ParameterDirection.Input));
                    ds = CommonMethods.getDataSet("uspcreatelocation", parms, svm.SessionID);
                    int locationID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    parms = new List<DbParameter>();
                    parms.Add(CommonMethods.buildStringParm("phoneidparm", myPhoneID, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildUInt32Parm("antecedentneedidparm", antecedentneedid, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildUInt32Parm("itemidparm", itemID, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildStringParm("descriptionparm", needDescription, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildDateTimeParm("rightnow", RightNow, ParameterDirection.Input));
                    ds = CommonMethods.getDataSet("uspcreateneed", parms, svm.SessionID);
                    int needID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    parms = new List<DbParameter>();
                    parms.Add(CommonMethods.buildStringParm("phoneidparm", myPhoneID, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildUInt32Parm("needidparm", needID, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildUInt32Parm("locationidparm", locationID, ParameterDirection.Input));
                    parms.Add(CommonMethods.buildDateTimeParm("rightnow", RightNow, ParameterDirection.Input));
                    CommonMethods.executeNonQuery("uspcreatelocationneedassociation", parms, svm.SessionID);
                    cnt++;
                } else {
                    _weredone = true;
                }
            }
            Response.Write("Ok:"+cnt++);
        } catch (Exception eee) {
            Response.Write("NotOK:" + eee.Message);
        } finally {
            try { sr.Close(); } catch { }
            Response.End();
        }
    }
    protected override void child_Page_Load(object sender, EventArgs e) {
    }

    /* A google address obtained using: 
     * {id:"B",cid:"11437636244782660208",latlng:{lat:39.759808999999997,lng:-105.02531},image:"http://maps.gstatic.com/intl/en_us/mapfiles/markerB.png",sprite:{width:20,height:34,top:34,image:"http://maps.gstatic.com/intl/en_us/mapfiles/red_markers_A_J2.png"},icon_id:"B",drg:true,laddr:"2975 North Federal Boulevard, Denver, CO 80211-3741 (Walgreens)",geocode:"CYnaizNiT1SSFcGvXgId4nC9-SFwOmfAkKW6ng",sxti:"Walgreens",sxst:"North Federal Boulevard",sxsn:"2975",sxct:"Denver",sxpr:"CO",sxpo:"80211-3741",sxcn:"US",sxph:"+13034338911",name:"Walgreens",infoWindow:{title:"\\x3cb\\x3eWalgreens\\x3c/b\\x3e",addressLines:["2975 North Federal Boulevard","Denver, CO 80211-3741"],phones:[{number:"(303) 433-26gl=US\\x26latlng=5592275590197317215\\x26q=walgreens\\x26near=denver+co",stars:2},ss:{edit:true,detailseditable:true,id:"00046ed7da446cc8180d8",deleted:false,lkg:{deleted:false},rapenabled:true,mmenabled:true},b_s:2,viewcode_data:[{preferred_panoid:"dRPNS85IN9T4KCe6kydywQ",viewcode_lat_e7:397189001,viewcode_lon_e7:3245093440,yaw:113.36814117431641,pitch:-6.4458804130554199}],hover_snippet:"I have never had an issue with another \\x3cb\\x3eWalgreens\\x3c/b\\x3e Store. That being said, I \\x3cb\\x3e...\\x3c/b\\x3e"}
     * {id:"A",cid:"8878321436803894353",latlng:{lat:39.696075999999998,lng:-104.941041},image:"http://maps.gstatic.com/intl/en_us/mapfiles/markerA.png",sprite:{width:20,height:34,top:0,image:"http://maps.gstatic.com/intl/en_us/mapfiles/red_markers_A_J2.png"},icon_id:"A",drg:true,laddr:"1111 S Colorado Blvd, Denver, CO 80246-2901 (Walgreens)",geocode:"CbcXskFDVdDfFcy2XQIdD7q--SFRjG6yTiI2ew",sxti:"Walgreens",sxst:"S Colorado Blvd",sxsn:"1111",sxct:"Denver",sxpr:"CO",sxpo:"80246-2901",sxcn:"US",sxph:"+13037588083",name:"Walgreens",infoWindow:{title:"\\x3cb\\x3eWalgreens\\x3c/b\\x3e",addressLines:["1111 S Colorado Blvd","Denver, CO 80246-2901"],phones:[{number:"(303) 758-8083"}]
    */
}
