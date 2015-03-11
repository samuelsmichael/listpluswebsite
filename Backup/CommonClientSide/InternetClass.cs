using System;
using System.Collections.Generic;
using System.Text;
using ProjectSpecific;
using System.Web;
using System.Web.SessionState;
using System.Web.Caching;
using DDCommon;

namespace CommonClientSide {
    public class InternetClass {
        protected HttpServerUtility _server;
        protected HttpSessionState _session;
        protected HttpApplicationState _application;
        protected Cache _cache;
        public InternetClass(HttpSessionState session, HttpServerUtility server, Cache cache, HttpApplicationState application) {
            _svm = new SessionVariableManager(session, server, null, null, cache, application);
            _server=server;
            _session=session;
            _application = application;
            _cache = cache;
        }
        private ProjectSpecificSessionVariableManager _svmps;
        private SessionVariableManager _svm;
        protected ProjectSpecificSessionVariableManager svmps {
            get {
                if (_svmps == null) {
                    _svmps = new ProjectSpecificSessionVariableManager(svm);
                }
                return _svmps;
            }
        }
        protected SessionVariableManager svm {
            get {
                return _svm;
            }
        }

    }
}
