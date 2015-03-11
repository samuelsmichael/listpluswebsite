using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services;
using System.Web;
using System.Data;
using System.Data.Common;
using DDCommon;
using ProjectSpecific;

namespace CommonClientSide {
    public abstract class InternetWebPage : System.Web.UI.Page {
        private SessionVariableManager _svm;
        private ProjectSpecificSessionVariableManager _svmps;
        public InternetWebPage() {
            Init += new EventHandler(InternetWebPage_Init);
        }
        public void goBackToLastDifferentPage() {
//            _svm.goBackToLastDifferentPage();
            Page.Response.Redirect(_svm.lastPageThatDiffers());
        }

        public bool canGoBack() {
            return _svm.canGoBack();
        }
        public string whatPageWouldYouGoBackTo() {
            return _svm.whatPageWouldYouGoBackTo();
        }
        public void goBackAPage() {
            _svm.goBackAPage();
        }
        void InternetWebPage_Init(object sender, EventArgs e) {
            CommonServerSide.CommonMethods.setCache(Page.Cache);
        }
        protected abstract void child_Page_Load(object sender, EventArgs e);
        public string PageName {
            get { return this.GetType().Name; }
        }
        protected ProjectSpecificSessionVariableManager svmps {
            get {
                if (_svmps == null) {
                    _svmps = new ProjectSpecificSessionVariableManager(svm);
                }
                return _svmps;
            }
            set {
                _svmps = value;
            }
        }
        protected SessionVariableManager svm {
            get {
                if (_svm == null) {
                    _svm = new SessionVariableManager(Session, Server, Request.UserHostAddress, PageName,this.Context.Cache, Application);
                }
                return _svm;
            }
            set {
                _svm = value;
            }
        }
        public string ParmGoTo {
            get {
                return CommonClientSide.CommonMethods.getParmGoTo(svm, PageName);
            }
            set {
                CommonClientSide.CommonMethods.setParmGoTo(svm, PageName, value);
            }
        }
        protected void Page_Init(object sender, EventArgs e) {
            svm.CurrentPage = PageName;
        }
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                try {
                    int index = Request.Url.AbsoluteUri.ToLower().IndexOf(PageName.Replace("members_", "").Substring(0, PageName.Replace("members_", "").Length - 5));

                    if (index != -1) {
                        Application["BaseURI"] = Request.Url.AbsoluteUri.Substring(0, index);
                    }
                    int index2a = Request.Url.AbsoluteUri.ToLower().IndexOf("//");
                    if (Request.Url.AbsoluteUri.Length - index2a > 1) {
                        index2a = Request.Url.AbsoluteUri.ToLower().IndexOf("/", index2a + 2);
                        if (index2a > 1) {
                            Application["BaseURI2"] = Request.Url.AbsoluteUri.Substring(0,index2a+1);
                        }
                    }
                    if (DDCommon.CommonRoutines.isNothing(Application["marre"])) {
                        Application["marre"] = Application["BaseURI"];
                    }

                    string baseURL = Request.Url.ToString();
                    int index2 = baseURL.IndexOf("//");
                    Application["BaseURIWithoutTrailingSlash"] = baseURL.Substring(0, baseURL.IndexOf("/", index2 + 2));
                } catch { }
                if (svm.RecordUserHits) {
                    if (svm.UserHostAddress == null) {
                        List<DbParameter> dbParameters2 = new List<DbParameter>();
                        dbParameters2.Add(CommonClientSide.CommonMethods.buildStringParm(
                                "UserSiteParm", Request.UserHostAddress, ParameterDirection.Input));
                        CommonClientSide.CommonMethods.executeNonQuery("usprecorduserhit", dbParameters2, Session.SessionID);
                        //TODO: (1)Add UserName to this table.
                    }
                }
                if (svm.RecordPageHits) {
                    List<DbParameter> dbParameters = new List<DbParameter>();
                    dbParameters.Add(CommonClientSide.CommonMethods.buildStringParm(
                            "PageNameParm", PageName, ParameterDirection.Input));
                    CommonClientSide.CommonMethods.executeNonQuery("usprecordpagehit", dbParameters, Session.SessionID);
                }
                string lastPushed=string.Empty;
                try {
                    if(svm.PagesVisited.Count>0) {
                        lastPushed = svm.PagesVisited.Peek();
                    }
                } catch { }
                if (!lastPushed.Equals(Request.AppRelativeCurrentExecutionFilePath)) {
                        svm.PagesVisited.Push(Request.AppRelativeCurrentExecutionFilePath);
                }
                svm.UserHostAddress = Request.UserHostAddress;
                svm.IsInitialCallToPage = false;
                child_Page_Load(sender, e);

            }
        }
        [WebMethod]
        public static void AbandonSession() {
            HttpContext.Current.Session.Abandon();

            if (HttpContext.Current.Cache["Connections" + "_" + HttpContext.Current.Session.SessionID] != null) {
                try {
                    ((DbConnection)HttpContext.Current.Cache["Connections" + "_" + HttpContext.Current.Session.SessionID]).Close();
                } catch { }
                HttpContext.Current.Cache.Remove("Connections" + "_" + HttpContext.Current.Session.SessionID);
            }
        }
    }
}
