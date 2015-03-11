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

namespace CommonClientSide {
    public abstract class EUInternetUserControl : InternetUserControl {
        private EUFieldLevelSecurityManager _fieldLevelSecurityManager = null;
        private ModuleLevelSecurityManager _moduleLevelSecurityManager = null;
        private SecurityManager.SecurityLevel _securityLevel = SecurityManager.SecurityLevel.NULL;
        public EUInternetUserControl() {
            PreRender += new EventHandler(EUInternetUserControl_PreRender);
        }
        protected void armAllControls(Control control, DataRow dr) {
            armAllControls(control, dr, null);
        }
        protected void armAllControls(Control control, DataRow dr, DataSet substitution) {
            try {
                if (control is EUInternetUserControlThreeColumnedControlCapable) {
                    ((EUInternetUserControlThreeColumnedControlCapable)control).DatabaseBindingRow = dr;
                    if (substitution != null) {
                        if (((EUInternetUserControlThreeColumnedControlCapable)control).DatabaseBindingFieldName.Equals("DateDecisionAppealed")) {
                            int xss = 33;
                        }
                        int c = 0;
                        foreach (DataRow dr2 in substitution.Tables[0].Rows) {
                            if (dr2["value"].ToString().Equals(
                                ((EUInternetUserControlThreeColumnedControlCapable)control).DatabaseBindingFieldName)) {
                                c++;
                                ((EUInternetUserControlThreeColumnedControlCapable)control).DatabaseBindingFieldName = dr2["field"].ToString();
                                break;
                                if (c >= substitution.Tables[0].Rows.Count) {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (control is DoesTime) {
                    ((EUInternetUserControlThreeColumnedControlCapable)control).DatabaseBindingRow = dr;
                    if (substitution != null) {
                        int c = 0;
                        foreach (DataRow dr2 in substitution.Tables[0].Rows) {
                            try {
                                if (dr2["value"].ToString().Equals(
                                    ((DoesTime)control).DatesTimeControlDatabaseField)) {
                                    c++;
                                    ((DoesTime)control).DatesTimeControlDatabaseField = dr2["field"].ToString();
                                }
                            } catch { }
                            try {
                                if (dr2["value"].ToString().Equals(
                                    ((DoesTime)control).TimesDateControlDatabaseField)) {
                                    c++;
                                    ((DoesTime)control).TimesDateControlDatabaseField = dr2["field"].ToString();
                                }
                            } catch { }
                            if (c >= substitution.Tables[0].Rows.Count) {
                                break;
                            }
                        }
                    }
                }
                foreach (Control child in control.Controls) {
                    armAllControls(child, dr, substitution);
                }
            } catch { }
        }
        private SecurityManager.SecurityLevel _MyModuleLevelSecurity = SecurityManager.SecurityLevel.None;

        public static EUModuleLevelSecurityManager buildModuleLevelSecurityManager(string pageName) {
            return new EUModuleLevelSecurityManager(pageName);
        }

        protected ModuleLevelSecurityManager MyModuleLevelSecurityManager {
            get {
                if (_moduleLevelSecurityManager == null) {
                    try {
                        _moduleLevelSecurityManager = buildModuleLevelSecurityManager(Page.GetType().Name);
                        _MyModuleLevelSecurity=_moduleLevelSecurityManager.ascertainSecurityLevel((GenericUser)svm.CurrentUser, svm.SessionID);
                    } catch { }
                }
                return _moduleLevelSecurityManager;
            }
        }
        
        protected SecurityManager.SecurityLevel SecurityLevel {
            get {
                if (svm.CurrentUser!=null && svm.CurrentUser.ToString().ToLower().IndexOf("euclientcontact")!=-1) {
                    return SecurityManager.SecurityLevel.Read;
                }
                if (_securityLevel == SecurityManager.SecurityLevel.NULL) {
                    if(_fieldLevelSecurityManager==null) {
                    _fieldLevelSecurityManager = new BusinessObjectsCommon.EUFieldLevelSecurityManager(ID);
                    }
                    _moduleLevelSecurityManager = MyModuleLevelSecurityManager;
/*defunct                    if(_moduleLevelSecurityManager==null) {
                        try {
                            _moduleLevelSecurityManager = ProjectSpecific.ProjectSpecificSessionVariableManager.buildModuleLevelSecurityManager(Page.GetType().Name);
                        } catch { }
                    }
 */
                    SecurityManager.SecurityLevel fieldLevelSecurity = _fieldLevelSecurityManager.ascertainSecurityLevel((GenericUser)svm.CurrentUser, svm.SessionID);
//defunct                    SecurityManager.SecurityLevel moduleLevelSecurity = _moduleLevelSecurityManager.ascertainSecurityLevel((GenericUser)svm.CurrentUser, svm.SessionID);
                    if (fieldLevelSecurity > _MyModuleLevelSecurity) {
                        _securityLevel = fieldLevelSecurity;
                    } else {
                        _securityLevel = _MyModuleLevelSecurity;
                    }
                }
                return _securityLevel;
            }
        }
        protected EUFieldLevelSecurityManager FieldLevelSecurityManager {
            get {
                if (_fieldLevelSecurityManager == null) {
                    _fieldLevelSecurityManager = new BusinessObjectsCommon.EUFieldLevelSecurityManager(ID);
                }
                return _fieldLevelSecurityManager;
            }
        }

        protected abstract void child_PreRender();
        protected override void child_Page_Load(object sender, EventArgs e) {
        }
        void EUInternetUserControl_PreRender(object sender, EventArgs e) {
            child_PreRender();
        }
    }
}
