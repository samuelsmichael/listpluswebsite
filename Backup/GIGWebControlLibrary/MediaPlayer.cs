using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace GIGWebControlLibrary {
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MediaPlayer runat=server></{0}:MediaPlayer>")]
    public class MediaPlayer : WebControl {
/*        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(400)]
        [Localizable(true)]
        public int Height {
            get {
                object s = ViewState["Height"];
                return ((s == null) ? 400 : (int)s);
            }

            set {
                ViewState["Height"] = value;
            }
        }
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(700)]
        [Localizable(true)]
        public int Width {
            get {
                object s = ViewState["Width"];
                return ((s == null) ? 700 : (int)s);
            }

            set {
                ViewState["Width"] = value;
            }
        }*/
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        [Localizable(true)]
        public string FileName {
            get {
                try {
                    if (System.Configuration.ConfigurationManager.AppSettings["DataIsHardcodedForDemoPurposes"].ToLower().Equals("true")) {
                        return ConfigurationManager.AppSettings["DemoPurposesVideo"];
                    } else {
                        Object s = ViewState["FileName"];
                        return ((s == null) ? "" : (string)s);
                    }
                } catch {
                    return "";
                }
            }

            set {
                ViewState["FileName"] = value;
            }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        [Localizable(true)]
        public bool Autostart {
            get {
                object s = ViewState["Autostart"];
                return ((s == null) ? true : (bool)s);
            }

            set {
                ViewState["Autostart"] = value;
            }
        }
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Localizable(true)]
        public bool Mute {
            get {
                object s = ViewState["Mute"];
                return ((s == null) ? true : (bool)s);
            }

            set {
                ViewState["Mute"] = value;
            }
        }
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(0)]
        [Localizable(true)]
        [Description("-100 to 100; 0 means equal balance")]
        public int Balance {
            get {
                object s = ViewState["Balance"];
                return ((s == null) ? 0 : (int)s);
            }

            set {
                ViewState["Balance"] = value;
            }
        }
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool Fullscreen {
            get {
                object s = ViewState["Fullscreen"];
                return ((s == null) ? false : (bool)s);
            }

            set {
                ViewState["Fullscreen"] = value;
            }
        }
/*        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Localizable(true)]
        public bool Enabled {
            get {
                object s = ViewState["Enabled"];
                return ((s == null) ? true : (bool)s);
            }

            set {
                ViewState["Enabled"] = value;
            }
        } */
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(1)]
        [Localizable(true)]
        public int PlayCount {
            get {
                object s = ViewState["PlayCount"];
                return ((s == null) ? 1 : (int)s);
            }

            set {
                ViewState["PlayCount"] = value;
            }
        }
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(-1)]
        [Localizable(true)]
        [Description("0-100; -1 means use last volume setting established for the player")]
        public int Volume {
            get {
                object s = ViewState["Volume"];
                return ((s == null) ? -1 : (int)s);
            }

            set {
                ViewState["Volume"] = value;
            }
        }
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(1.0)]
        [Localizable(true)]
        [Description("Multiplier for playback speed")]
        public double Rate {
            get {
                object s = ViewState["Rate"];
                return ((s == null) ? 1.0 : (double)s);
            }

            set {
                ViewState["Rate"] = value;
            }
        }
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Localizable(true)]
        public bool StretchToFit {
            get {
                object s = ViewState["StretchToFit"];
                return ((s == null) ? true : (bool)s);
            }

            set {
                ViewState["StretchToFit"] = value;
            }
        }
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        [Localizable(true)]
        public bool EnableContextMenu {
            get {
                object s = ViewState["EnableContextMenu"];
                return ((s == null) ? true : (bool)s);
            }

            set {
                ViewState["EnableContextMenu"] = value;
            }
        }

        protected override void Render(HtmlTextWriter output) {
            StringBuilder sb = new StringBuilder("<OBJECT ID='" +
                  this.ClientID + "' name='" + this.ClientID + "' " +
                  "CLASSID='CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6'" +
                  "VIEWASTEXT" +
                  "height=" + Height + " " + "width=" + Width +
                  ">");

            //Render properties as object parameters 
            sb.Append("<PARAM name='URL' value='" +
                  FileName + "'>");
            sb.Append("<PARAM name='mute' value='" +
                  Mute.ToString() + "'>");
            sb.Append("<PARAM name='AutoStart' value='" +
                  Autostart.ToString() + "'>");
            sb.Append("<PARAM name='balance' value='" +
                  Balance + "'>");
            sb.Append("<PARAM name='enabled' value='" +
                  Enabled.ToString() + "'>");
            sb.Append("<PARAM name='fullScreen' value='" +
                  Fullscreen.ToString() + "'>");
            sb.Append("<PARAM name='playCount' value='" +
                  PlayCount.ToString() + "'>");
            if(Volume>=0) {
            sb.Append("<PARAM name='volume' value='" +
                  Volume + "'>");
            }
            sb.Append("<PARAM name='rate' value='" +
                  Rate + "'>");
            sb.Append("<PARAM name='StretchToFit' value='" +
                  StretchToFit.ToString() + "'>");
            sb.Append("<PARAM name='enabledContextMenu' value='" +
                  EnableContextMenu.ToString() + "'>");

            //output ending object tag 
            sb.Append("</OBJECT>");

            //flush everything to the output stream 
            output.Write(sb.ToString());
        }
    }
}
