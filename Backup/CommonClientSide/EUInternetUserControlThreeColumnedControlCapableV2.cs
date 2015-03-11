using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BusinessObjects;
using System.Data;
using System.Web.Caching;

namespace CommonClientSide {
    public abstract class EUInternetUserControlThreeColumnedControlCapableV2 : EUInternetUserControlThreeColumnedControlCapable {
        protected abstract bool IsEmpty { get;}
        public event EventHandler PromptLabelTextSetDynamically;
        public System.Web.UI.WebControls.Unit WidthCenterColumn {
            get {
                object obj = ViewState["WidthCenterColumn"];
                return obj == null ? new Unit(200d) : (Unit)obj;
            }
            set {
                ViewState["WidthCenterColumn"] = value;
            }
        }
        public virtual System.Web.UI.WebControls.Unit WidthLeftColumn {
            get {
                object obj = ViewState["WidthLeftColumn"];
                return obj == null ? new Unit(200d) : (Unit)obj;
            }
            set {
                ViewState["WidthLeftColumn"] = value;
            }
        }
        public bool IsRequiredField {
            get {
                object obj = ViewState["IsRequiredField"];
                return obj == null ? false : (bool)ViewState["IsRequiredField"];
            }
            set {
                ViewState["IsRequiredField"] = value;
            }
        }
        public virtual string PromptLabelText {
            get {
                object obj = ViewState["PromptLabelText"];
                return obj == null ? "" : (string)obj;
            }
            set {
                ViewState["PromptLabelText"] = value;
                if (PromptLabelTextSetDynamically != null) {
                    PromptLabelTextSetDynamically(this, null);
                }
            }
        }
        public override bool Visible {
            get {
                if (!MustHaveOneOfTheseRoleCodes_SpaceDelimited.Equals(string.Empty)) {
                    return MyModuleLevelSecurityManager.hasOneOfTheseRoleCodes(requiredRoleCodesAsStringArray(), svm.CurrentUser, svm.SessionID, null);
                } else {
                    return base.Visible;
                }
            }
            set {
                base.Visible = value;
            }
        }
        public override bool Enabled {
            get {
                if (!AmInScreenPreedits && !MustHaveOneOfTheseRoleCodesToUpdate_SpaceDelimited.Equals(string.Empty)) {
                    return MyModuleLevelSecurityManager.hasOneOfTheseRoleCodes(roleCodesForUpdatingAsArray(), svm.CurrentUser, svm.SessionID, null);
                }

                if (!AmInScreenPreedits && !MustHaveOneOfTheseRoleCodesAfterFilledToUpdate_SpaceDelimited.Equals(string.Empty) && !IsEmpty) {
                    return MyModuleLevelSecurityManager.hasOneOfTheseRoleCodes(roleCodesForUpdatingAfterFilledAsArray(), svm.CurrentUser, svm.SessionID, null);
                } else {
                    return base.Enabled;
                }
            }
            set {
                base.Enabled = value;
            }
        }
        protected string[] requiredRoleCodesAsStringArray() {
            return MustHaveOneOfTheseRoleCodes_SpaceDelimited.Split(' ');
        }
        protected string[] roleCodesForUpdatingAfterFilledAsArray() {
            return MustHaveOneOfTheseRoleCodesAfterFilledToUpdate_SpaceDelimited.Split(' ');
        }
        protected string[] roleCodesForUpdatingAsArray() {
            return MustHaveOneOfTheseRoleCodesToUpdate_SpaceDelimited.Split(' ');
        }
        public string MustHaveOneOfTheseRoleCodes_SpaceDelimited {
            get {
                object obj = ViewState["MustHaveOneOfTheseRoleCodes_SpaceDelimited"];
                return obj == null ? string.Empty : ((string)obj).Trim();
            }
            set {
                ViewState["MustHaveOneOfTheseRoleCodes_SpaceDelimited"]=value.Trim();
            }
        }
        public string MustHaveOneOfTheseRoleCodesAfterFilledToUpdate_SpaceDelimited {
            get {
                object obj = ViewState["MustHaveOneOfTheseRoleCodesAfterFilledToUpdate_SpaceDelimited"];
                return obj == null ? string.Empty : ((string)obj).Trim();
            }
            set {
                ViewState["MustHaveOneOfTheseRoleCodesAfterFilledToUpdate_SpaceDelimited"] = value.Trim();
            }
        }
        public string MustHaveOneOfTheseRoleCodesToUpdate_SpaceDelimited {
            get {
                object obj = ViewState["MustHaveOneOfTheseRoleCodesToUpdate_SpaceDelimited"];
                return obj == null ? string.Empty : ((string)obj).Trim();
            }
            set {
                ViewState["MustHaveOneOfTheseRoleCodesToUpdate_SpaceDelimited"] = value.Trim();
            }
        }
    }
}
