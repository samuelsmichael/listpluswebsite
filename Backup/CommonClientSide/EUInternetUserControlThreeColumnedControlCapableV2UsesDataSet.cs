using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CommonClientSide {
    public abstract class EUInternetUserControlThreeColumnedControlCapableV2UsesDataSet : EUInternetUserControlThreeColumnedControlCapableV2 {
        private object _dataSource = null;
        public string DataTextField {
            get {
                object obj = ViewState["DataTextField"];
                return obj == null ? "desc" : (string)obj;
            }
            set {
                ViewState["DataTextField"] = value;
            }
        }
        public string DataValueField {
            get {
                object obj = ViewState["DataValueField"];
                return obj == null ? "value" : (string)obj;
            }
            set {
                ViewState["DataValueField"] = value;
            }
        }
        public object DataSource {
            get {
                return _dataSource;
            }
            set {
                _dataSource = value;
            }
        }
    }
}
