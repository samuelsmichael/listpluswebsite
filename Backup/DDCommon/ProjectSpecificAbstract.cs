using System;
using System.Collections.Generic;
using System.Text;

namespace DDCommon.cs {
    public abstract class ProjectSpecificAbstract {
        public abstract string getDestinaireEmailAddress();
        public abstract string getAdminPassword();
        public abstract void InstanceLogDebugMessage(string variable, string msg, string sproc);
    }
}
