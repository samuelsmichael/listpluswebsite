using System;
using System.Collections.Generic;
using System.Text;

namespace DDCommon {
    public interface ReportServer {
        void enqueueReport(AsyncActivity anAsyncActivity);
    }
}
