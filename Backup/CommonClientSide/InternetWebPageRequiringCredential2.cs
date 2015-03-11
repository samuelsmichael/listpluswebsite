using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClientSide {
    public abstract class InternetWebPageRequiringCredential2 : InternetWebPage {
        public InternetWebPageRequiringCredential2() {
            PreInit += new EventHandler(AdminConsole_Events_PreInit);
            PreRender += new EventHandler(InternetWebPageRequiringCredential_PreRender);
        }

        void InternetWebPageRequiringCredential_PreRender(object sender, EventArgs e) {
            object obj = Master;
            int x = 3;
        }

        public string MyMasterPageFile {
            get {
                return "~/CommonClientSide2/PageRequiringCredentials2.Master";
            }
        }


        void AdminConsole_Events_PreInit(object sender, EventArgs e) {
            svm.CredentialsPageNeedsUserId = NeedsUserId;
            MasterPageFile = "~/CommonClientSide2/PageRequiringCredentials2.Master";

        }
        protected abstract bool NeedsUserId { get;}
    }
}
