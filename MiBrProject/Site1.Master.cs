using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MiBrProject {
    public partial class Site1 : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {
            Menu1.FindItem(CurrentlySelectedItem).Selected = true;
            if (Page.ToString().ToLower().IndexOf("policy") != -1) {
                PanelPolicy.Visible = false;
            } else {
                PanelPolicy.Visible = true;
            }

        }
        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e) {
            CurrentlySelectedItem = e.Item.Value;
            switch (e.Item.Value) {
                case "Home":
                    Server.Transfer("~/Default.aspx");
                    break;
                case "Product Features":
                    Server.Transfer("~/ProductFeatures.aspx");
                    break;
                case "Quick Start":
                    Server.Transfer("~/QuickStart.aspx");
                    break;
                case "Contact Us":
                    Server.Transfer("~/ContactUs.aspx");
                    break;
                default:
                    Server.Transfer("~/Default.aspx");
                    break;
            }
        }
        private string CurrentlySelectedItem {
            get {
                object obj = Session["CSI"];
                return obj == null ? "Home" : (string)obj;
            }
            set {
                Session["CSI"] = value;
            }
        }

        protected void LinkButtonPolicyStatement_Click(object sender, EventArgs e) {
            Server.Transfer("~/MiBrPolicyStatement.aspx");
        }
    }
}
