using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessObjectsCommon;
using BusinessObjects;
using GIGWebControlLibrary;

namespace CommonClientSide {
    public abstract class EUInternetUserControlThreeColumnedControlCapable : EUInternetUserControl,ContainsBindsToDataControls {
        private DataRow _databaseBindingRow=null;
        public event EventHandler Updating;
        protected abstract void child_DatabaseBindingRow(DataRow dr);
        public EUInternetUserControlThreeColumnedControlCapable() {
            Init += new EventHandler(EUInternetUserControlThreeColumnedControlCapable_Init);
        }
        protected abstract bool IsControlEnabled { get;}
        public virtual bool Enabled {
            get {
                object obj = ViewState["Enabled"];
                return obj == null ? true : (bool)obj;
            }
            set {
                ViewState["Enabled"] = value;
            }
        }
        protected ContainsBindsToDataControls MyContainsBindsToDataControlsPanel {
            get {
                Control parent = Parent;
                while (parent != null) {
                    if (parent is ContainsBindsToDataControls) {
                        return (ContainsBindsToDataControls)parent;
                    }
                    parent = parent.Parent;
                }
                throw new Exception("Unable to find a 'ContainsBindsToDataControls' for this control");
            }
        }
        void EUInternetUserControlThreeColumnedControlCapable_Init(object sender, EventArgs e) {
            MyContainsBindsToDataControlsPanel.Updating += new EventHandler(panel_Updating);
        }
        void panel_Updating(object sender, EventArgs e) {
            if(!BindToData()) {
                ((DDCommon.EventArgsArrayList)e).AddEvent("Signals failure");
           }
        }
        protected abstract void child_child_PreRender();
        protected override void child_PreRender() {
//            if (DatabaseBindingFieldName != null && !DatabaseBindingFieldName.Trim().Equals(string.Empty)) {
                child_child_PreRender();
  //          } else { 
    //        }
        }

        public string DatabaseBindingFieldName {
            get {
                object s = ViewState["DatabaseBindingFieldName"];
                return s == null ? "" : (string)s;
            }
            set {
                ViewState["DatabaseBindingFieldName"] = value;
            }
        }
        public virtual DataRow DatabaseBindingRow {
            get {
                return _databaseBindingRow;
            }
            set {
                _databaseBindingRow= value;
                child_DatabaseBindingRow((DataRow)value);
            }
        }
    }
}
