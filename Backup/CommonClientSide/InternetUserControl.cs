using System;
using System.Collections.Generic;
using System.Text;
using DDCommon;
using ProjectSpecific;
using System.Web.Caching;

namespace CommonClientSide {
    public abstract class InternetUserControl : System.Web.UI.UserControl, GIGWebControlLibrary.BindsToData {
        private ProjectSpecificSessionVariableManager _svmps;
        private SessionVariableManager _svm;
        protected abstract void child_Page_Load(object sender, EventArgs e);
        protected string UserControlName {
            get { return this.GetType().Name; }
        }
        /* Override this methods in your user control if you are using ThreeColumnedGenericControl 
         * and wish to bind to data.  Return true if nothing went wrong.
        */
        public virtual bool BindToData() {
            return true;
        }
        public bool AmInScreenPreedits {
            get {
                object obj = svm.getSessionStateUsingCache("AmInScreenPreedits");
                return obj == null ? false : (bool)obj;
            }
            set {
                svm.setSessionStateUsingCache("AmInScreenPreedits", value, CacheItemPriority.High);
            }
        }
        public InternetUserControl() {
            this.Init += new EventHandler(InternetUserControl_Init);
        }

        void InternetUserControl_Init(object sender, EventArgs e) {
            CommonServerSide.CommonMethods.setCache(Page.Cache);
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
                    _svm = new SessionVariableManager(Session, Server, Request.UserHostAddress, UserControlName, this.Context.Cache, Application);
                }
                return _svm;
            }
            set {
                _svm = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e) {
            bool doit=false;
            try {
                if (((InternetWebPage)Page).ParmGoTo!=null && ((InternetWebPage)Page).ParmGoTo.Equals(UserControlName)) {
                    doit = true;
                }
            } catch {
                doit = true;
            }
            if (doit) {
                child_Page_Load(sender, e);
            }
        }
    }
}
