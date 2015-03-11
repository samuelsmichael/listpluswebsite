using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CommonClientSide;

namespace CommonClientSide {
    public abstract class LandingPage : InternetWebPage {
        protected string NameOfTXTFile { 
            get { return PageName+".txt"; }
        }
        protected abstract GIGWebControlLibrary.FileViewer YourFileViewer { get;}
        protected abstract void child_child_Page_Load(object sender, EventArgs e);
        protected override void child_Page_Load(object sender, EventArgs e) {
            try {
                int jdindex = Request.Url.AbsoluteUri.LastIndexOf("/");
                string newUri = Request.Url.AbsoluteUri.Substring(0, jdindex+1);
                YourFileViewer.FileSpec = newUri + "StaticContent2/" + NameOfTXTFile;
                YourFileViewer.FileSpec2 = Server.MapPath("~/StaticContent2/" + NameOfTXTFile);
            } catch { }
        }
    }
}
