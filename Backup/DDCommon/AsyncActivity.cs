using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;

public delegate void AsynchronousThreadFinishedCallback(bool successful, string msg);
public delegate void AsynchronousThreadBeginCallback(Dictionary<string, object> runParameters);

namespace DDCommon {
    public class AsyncActivity {
        private AsynchronousThreadFinishedCallback _callbackFinished;
        private AsynchronousThreadBeginCallback _callbackStart;
        private Dictionary<string, object> _runParameters;
        private System.ComponentModel.BackgroundWorker _BackgroundWorker;

        // reportServer=null -> create my own thread; else, use the ReportServer
        public AsyncActivity(Dictionary<string, object> runParameters, AsynchronousThreadBeginCallback callbackStart, 
                AsynchronousThreadFinishedCallback callbackFinished, bool useBackgroundWoika, ReportServer reportServer ) {
            _callbackFinished = callbackFinished;
            _callbackStart = callbackStart;
            _runParameters = runParameters;
            if (useBackgroundWoika) {
                ThreadStart threadStart = new ThreadStart(run);
                Thread thread = new Thread(threadStart);
                thread.Start();
            } else {
                _BackgroundWorker = new System.ComponentModel.BackgroundWorker();
                _BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(_BackgroundWorker_DoWork);
                _BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(_BackgroundWorker_RunWorkerCompleted);
                _BackgroundWorker.RunWorkerAsync();
            //    reportServer.enqueueReport(this);
            }
        }

        void _BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            _callbackFinished(e.Error==null,e.Error!=null?e.Error.Message:null);
            
        }

        void _BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            _callbackStart(_runParameters);        
        }
        public AsynchronousThreadFinishedCallback CallbackFinished {
            get {
                return _callbackFinished;
            }
        }
        public AsynchronousThreadBeginCallback CallbackStart {
            get {
                return _callbackStart;
            }
        }
        public Dictionary<string, object> RunParameters {
            get {
                return _runParameters;
            }
        }
        private void run() {
            try {
                _callbackStart(_runParameters);
                _callbackFinished(true, null);
            } catch (Exception e) {
                _callbackFinished(false, e.Message);
            }
        }
    }
}
