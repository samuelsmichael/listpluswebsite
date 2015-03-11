using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Web.Caching;

namespace DDCommon {
    public class SessionVariableManager {
        private bool _USE_SESSION_STATE_INSTEAD_OF_CACHE = true;
        System.Web.SessionState.HttpSessionState _session;
        System.Web.HttpApplicationState _application;
        System.Web.HttpServerUtility _server;
        string _pageName;
        string _userHostAddress;
        private static Cache _cache;
        public enum LoginCredentialsViewState {
            JustPassword = 0,
            IForgotMyPassword
        }
        private bool USE_SESSION_STATE_INSTEAD_OF_CACHE {
            get {
                if (_session == null) {
                    return false;
                } else {
                    return ConfigurationManager.AppSettings["USE_SESSION_STATE_INSTEAD_OF_CACHE"] == null ||
                            ConfigurationManager.AppSettings["USE_SESSION_STATE_INSTEAD_OF_CACHE"].ToLower().Equals("true");
                }
            }
        }
        public bool RecordUserHits {
            get {
                return ConfigurationManager.AppSettings["RecordUserHits"] == null ||
                    ConfigurationManager.AppSettings["RecordUserHits"].ToLower().Equals("true");
            }
        }
        public bool RecordPageHits {
            get {
                return ConfigurationManager.AppSettings["RecordPageHits"] == null ||
                    ConfigurationManager.AppSettings["RecordPageHits"].ToLower().Equals("true");
            }
        }
        public object[] BobbleBibble {
            get {
                return (object[])getSessionStateUsingCache("BobbleBibble");
            }
            set {
                setSessionStateUsingCache("BobbleBibble",value);
            }
        }
        public int MyEditIndex {
            get {
                object obj = getSessionStateUsingCache("MyEditIndex");
                return obj == null ? -1 : (int)obj;
            }
            set {
                setSessionStateUsingCache("MyEditIndex",value,CacheItemPriority.High);
            }
        }
        public int RowUpdatingIndex {
            get {
                object obj = getSessionStateUsingCache("RowUpdatingIndex");
                return obj == null ? DDCommon.CommonRoutines.NULL_INT : (int)obj;
            }
            set {
                setSessionStateUsingCache("RowUpdatingIndex",value,CacheItemPriority.High);
            }
        }
        public int MySelectedIndex {
            get {
                object obj = getSessionStateUsingCache("MySelectedIndex");
                return obj == null ? -1 : (int)obj;
            }
            set {
                setSessionStateUsingCache("MySelectedIndex",value,CacheItemPriority.High);
            }
        }
        public LoginCredentialsViewState MyLoginCredentialsViewState {
            get {
                object obj = getSessionStateUsingCache("LoginCredentialsViewState");
                return obj == null ? LoginCredentialsViewState.JustPassword : (LoginCredentialsViewState)obj;
            }
            set {
                setSessionStateUsingCache("LoginCredentialsViewState",value,CacheItemPriority.High);
            }
        }
        public System.Web.HttpServerUtility Server {
            get {
                return _server;
            }
        }
        public bool HaveAuthenticatedUser {
            get {
                return CurrentUser != null;
            }
        }
        public bool HaveAuthenticatedAdministrator {
            get {
                object obj = getSessionStateUsingCache("HaveAuthenticatedAdministrator");
                return obj == null ? false : (bool)obj;
            }
            set {
                setSessionStateUsingCache("HaveAuthenticatedAdministrator",value,CacheItemPriority.High);
            }
        }
        public Object CurrentUser {
            get {
                object obj = getSessionStateUsingCache("CurrentUser");
                return obj == null ? null : obj;
            }
            set {
                setSessionStateUsingCache("CurrentUser",value,CacheItemPriority.AboveNormal);
            }
        }
        public DataSet UserForgotHisOrHerPassword {
            get {
                object obj = getSessionStateUsingCache("UserForgotHisOrHerPassword");
                return obj == null ? null : (DataSet)obj;
            }
            set {
                setSessionStateUsingCache("UserForgotHisOrHerPassword",value,CacheItemPriority.BelowNormal);
            }
        }
        public string PaypalReturnString {
            get {
                object obj = getSessionStateUsingCache("PaypalReturnString");
                return obj == null ? string.Empty : obj.ToString();
            }
            set {
                setSessionStateUsingCache("PaypalReturnString",value,CacheItemPriority.High);
            }
        }
        public bool CredentialsPageNeedsUserId {
            get {
                object obj = _session["CredentialsPageNeedsUserId"];
                return obj == null ? false : (bool)_session["CredentialsPageNeedsUserId"];
            }
            set {
                _session["CredentialsPageNeedsUserId"] = value;
            }
        }
        public PaymentManager.TypeOfPurchase TypeOfPurchaseBeingMade {
            get {
                try {
                    return (PaymentManager.TypeOfPurchase)_session["TypeOfPurchaseBeingMade"];
                } catch {
                    return PaymentManager.TypeOfPurchase.Null;
                }
            }
            set { _session["TypeOfPurchaseBeingMade"] = value; }
        }
        public System.Web.SessionState.HttpSessionState Session {
            get {
                return _session;
            }
        }
        public SessionVariableManager(System.Web.SessionState.HttpSessionState session, System.Web.HttpServerUtility server, string userHostAddress, string pageName, Cache cache, System.Web.HttpApplicationState application) {
            _application=application;
            _session = session;
            _server = server;
            _userHostAddress = userHostAddress;
            _pageName = pageName;
            _cache = cache;
        }
        private void cacheItemRemoved(string str, object obj, CacheItemRemovedReason cirr) {
            int bkhere = 3;
        }
        private string HomeSite {
            get {
                return   (string)_application["BaseURIWithoutTrailingSlash"];
            }
        }

        private Hashtable KnownCacheStringsBySessionID {
            get {
                try {
                    if (_session["KnownCacheStringsBySessionID"] == null) {
                        _session["KnownCacheStringsBySessionID"] = new Hashtable();
                    }
                } catch (Exception ee3) {
                    int x = 3;
                }
                return (Hashtable)_session["KnownCacheStringsBySessionID"];
            }
        }
        public delegate void CleanUpCacheDelegate(string sessionId);
        public void cleanupCache(string sessionId) {
            foreach (string key in KnownCacheStringsBySessionID.Keys) {
                _cache.Remove(key + "_" + sessionId);
            }
        }
        public bool WeveSignedOnAtLeastOnce {
            get {
                object obj=_application["WeveSignedOnAtLeastOnce"+SessionID];
                return obj == null ? false : (bool)obj;
            }
            set {
                _application["WeveSignedOnAtLeastOnce" + SessionID] = value;
            }
        }
        private Hashtable _SessionCache=new Hashtable();
        public object getSessionStateUsingCache(string name) {
            if (USE_SESSION_STATE_INSTEAD_OF_CACHE) {
                return Session[name];
            } else {
                bool doSignon = true;
                if (!WeveSignedOnAtLeastOnce) {
                    doSignon = false;
                }
                if (name.Equals("HaveAuthenticatedUser")
                    ) {
                    doSignon = false;
                }
                if (doSignon) {
                    if (_cache["CurrentUser_" + SessionID] == null) { // we've timed out
                        WeveSignedOnAtLeastOnce = false;
                        try {
                            Server.Transfer(PagesVisited.ToArray()[0]);
                        } catch { }
                        Server.Transfer("~/CMS/Default.aspx");
                    }
                }
                return _cache[name + "_" + SessionID];
            }
        }
        public Cache MyCache {
            get {
                return _cache;
            }
        }
        public void setSessionStateUsingCache(string name, object value) {
            setSessionStateUsingCache(name, value, CacheItemPriority.Normal);
        }
        public void setSessionStateUsingCache(string name, object value, CacheItemPriority priority) {
            if (null == value) {
                if (USE_SESSION_STATE_INSTEAD_OF_CACHE) {
                    Session[name] = null;
                } else {
                    _cache.Remove(name + "_" + SessionID);
                }
            } else {
                if (USE_SESSION_STATE_INSTEAD_OF_CACHE) {
                    Session[name] = value;
                } else {
                    if (_session != null) {
                        KnownCacheStringsBySessionID[name] = true;
                    }
                    _cache.Insert(name + "_" + SessionID, value, null, Cache.NoAbsoluteExpiration,
                        new TimeSpan(0, 120, 20), priority,
                        new CacheItemRemovedCallback(cacheItemRemoved));
                }
                if (name.Equals("CurrentUser")) {
                    WeveSignedOnAtLeastOnce = true;
                }
            }
        }
        public Hashtable xSessionStateUsingCache {
            get {
                Hashtable sessionStateUsingCache = (Hashtable)_cache[SessionID];
                if (sessionStateUsingCache == null) {
                    _cache.Add(SessionID,new Hashtable(),null,Cache.NoAbsoluteExpiration,
                        new TimeSpan(0, 120, 20), CacheItemPriority.Normal, new CacheItemRemovedCallback(cacheItemRemoved));
                }
                return (Hashtable)_cache[SessionID];
            }
        }
        public bool IsInitialCallToPage {
            get {
                if (_session[_pageName] == null) {
                    _session[_pageName] = true;
                }
                return (bool)_session[_pageName];
            }
            set {
                _session[_pageName] = value;
            }
        }
        private string __SessionID=null;
        public string SessionID {
            get {
                if (__SessionID == null) {
                    if (_session == null) {
                        __SessionID = DDCommon.CommonRoutines.dateInCCYYMMDDHHMMSSFormat(DateTime.Now);
                    } else {
                        __SessionID= _session.SessionID.ToString();
                    }
                }
                return __SessionID;
            }
        }
        public string CurrentPage {
            get {
                object obj = _session["CurrentPage"];
                return obj == null ? "" : (string)obj;
            }
            set {
                _session["CurrentPage"] = value;
            }
        }
        public bool InAPaymentTransaction {
            get {
                object obj = _session["InAPaymentTransaction"];
                return obj == null ? false : (bool)obj;
            }
            set {
                _session["InAPaymentTransaction"] = value;
            }
        }
        public bool InARenewalTransactionFromThePointOfViewOfPage {
            get {
                object obj = _session["InARenewalTransactionFromThePointOfViewOfPage"];
                return obj == null ? false : (bool)obj;
            }
            set {
                _session["InARenewalTransactionFromThePointOfViewOfPage"] = value;
            }
        }
        public bool InARenewalTransaction {
            get {
                object obj = _session["InARenewalTransaction"];
                return obj == null ? false : (bool)obj;
            }
            set {
                _session["InARenewalTransaction"] = value;
            }
        }
        public string ErrorMessage {
            get {
                object obj = _session["ErrorMessage"];
                return obj == null ? "" : obj.ToString();
            }
            set {
                _session["ErrorMessage"] = value;
            }
        }
        public System.Collections.Generic.Dictionary<String, String> ParmGoToByPage {
            get {
                if (getSessionStateUsingCache("ParmGoToByPage") == null) {
                    setSessionStateUsingCache("ParmGoToByPage",new System.Collections.Generic.Dictionary<String, String>(),CacheItemPriority.BelowNormal);
                }
                return (System.Collections.Generic.Dictionary<string, string>)getSessionStateUsingCache("ParmGoToByPage");
            }
            set {
                setSessionStateUsingCache("ParmGoToByPage",value,CacheItemPriority.BelowNormal);
            }
        }
        public string UserHostAddress {
            get {
                return (string)_session["UserHostAddress"];
            }
            set {
                _session["UserHostAddress"] = value;
            }
        }
        public string IFrameBusterURL {
            get {
                object obj = _session["IFrameBusterURL"];
                return obj == null ? "" : obj.ToString();
            }
            set {
                _session["IFrameBusterURL"] = value;
            }
        }
        public string IFrameReentryURL {
            get {
                object obj = _session["IFrameReentryURL"];
                return obj == null ? "" : obj.ToString();
            }
            set {
                _session["IFrameReentryURL"] = value;
            }
        }
        public string ReasonForPaymentDenial {
            get {
                object obj = _session["ReasonForPaymentDenial"];
                return obj == null ? string.Empty : obj.ToString();
            }
            set {
                _session["ReasonForPaymentDenial"] = value;
            }
        }
        /* changed 2/5/2009 
        public int rowDeletingId {
            get {
                object obj = _session["rowDeletingId"];
                return obj == null ? CommonRoutines.NULL_INT : Convert.ToInt32(obj);
            }
            set {
                _session["rowDeletingId"] = value;
            }
        }
        public int rowUpdatingId {
            get {
                object obj = _session["rowUpdatingId"];
                return obj == null ? CommonRoutines.NULL_INT : Convert.ToInt32(obj);
            }
            set {
                _session["rowUpdatingId"] = value;
            }
        }
        */
        public object rowDeletingId {
            get {
                return _session["rowDeletingId"];
            }
            set {
                _session["rowDeletingId"] = value;
            }
        }
        public object rowUpdatingId {
            get {
                return _session["rowUpdatingId"];
            }
            set {
                _session["rowUpdatingId"] = value;
            }
        }
        /* Updated 2/5/2009 
        public bool amInRowUpdatingMode {
            get {
                return rowUpdatingId != DDCommon.CommonRoutines.NULL_INT;
            }
        }
         * */
        public bool amInRowUpdatingMode {
            get {
                return rowUpdatingId != null && !rowUpdatingId.Equals(DDCommon.CommonRoutines.NULL_INT);
            }
        }
        
        public Stack<string> PagesVisited {
            get {
                if (getSessionStateUsingCache("PagesVisited") == null) {
                    setSessionStateUsingCache("PagesVisited",new Stack<string>(),CacheItemPriority.BelowNormal);
                }
                return (Stack<string>)getSessionStateUsingCache("PagesVisited");
            }
        }
        public int CurrentPaymentTransactionId {
            get {
                if (getSessionStateUsingCache("CurrentPaymentTransactionId") == null) {
                    setSessionStateUsingCache("CurrentPaymentTransactionId", DDCommon.CommonRoutines.NULL_INT,CacheItemPriority.High);
                }
                return (int)getSessionStateUsingCache("CurrentPaymentTransactionId");
            }
            set {
                setSessionStateUsingCache("CurrentPaymentTransactionId",value,CacheItemPriority.High);
            }
        }
        public bool canGoBack() {
            if(PagesVisited.Count>1) {
                string page1=PagesVisited.Pop();
                string page2=PagesVisited.Peek();
                PagesVisited.Push(page1);
                if(page2.ToLower().IndexOf("payment")==-1) {
                    if(
                        (page2.ToLower().IndexOf("candidates_jobsearch")!=-1) &&
                        (page1.ToLower().IndexOf("jobviewpage")!=-1)
                       ) {
                        return true;
                    }
                } 
            }
            return false;
        }
        public string lastPageThatDiffers() {
            string thisPage = PagesVisited.Pop();
            string priorPage = PagesVisited.Pop();
            while (priorPage.Equals(thisPage)) {
                priorPage = PagesVisited.Pop();
            }
            return priorPage;
        }
        public string whatPageWouldYouGoBackTo() {
            try {
                PagesVisited.Pop();
                string priorPage = PagesVisited.Pop();
                if (priorPage.ToLower().IndexOf("payment") == -1) {
                    return priorPage;
                } else {
                    return "Default.aspx";
                }
            } catch (Exception ee) {
                return "Default.aspx";
            }
        }
        public void goBackToLastDifferentPage() {
            try {
                Server.Transfer(lastPageThatDiffers(),false);
            } catch {
                try {
                    Server.Transfer("Default.aspx");
                } catch {
                    try {
                        Server.Transfer("/Default.aspx");
                    } catch { }
                }
            }
        }
        public void goBackAPage() {
            try {
                Server.Transfer(whatPageWouldYouGoBackTo(),false);
            } catch {
                try {
                    Server.Transfer("Default.aspx");
                } catch {
                    try {
                        Server.Transfer("/Default.aspx");
                    } catch { }
                }
            }
        }
    }
}
