using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace DDCommon {
    public abstract class PaymentManager {
        protected double _costPerSelection;
        protected int _numberOfItemsSelected;
        protected string _email;
        protected string _URLReturnPaymentAccepted;
        protected string _URLReturnCancel;
        protected string _URLReturnPaymentDenied;
        protected Control _parent;
        protected TypeOfPurchase _typeOfPurchase;
        public enum TypeOfPurchase {
            Null=-1,
            Month=0,
            Year=1,
            FinishedRecordingTransaction=2,
            Subscription
        }
        public PaymentManager(Control parent, TypeOfPurchase typeOfPurchase, double costPerSelection, int numberOfItemsSelected, string email,
            string URLReturnPaymentAccepted, string URLReturnCancel, string URLReturnPaymentDenied) {
            _parent = parent;
            _numberOfItemsSelected = numberOfItemsSelected;
            _costPerSelection = costPerSelection;
            _email = email;
            _URLReturnCancel = URLReturnCancel;
            _URLReturnPaymentAccepted = URLReturnPaymentAccepted;
            _URLReturnPaymentDenied = URLReturnPaymentDenied;
            _typeOfPurchase = typeOfPurchase;
        }
        public abstract void Execute();
    }
    public class PaymentManagerURL : PaymentManager {
        protected string _URL;
        protected decimal _totalCost;
        protected SessionVariableManager _svm;
        protected HttpResponse _httpResponse;
        public PaymentManagerURL(Control parent,TypeOfPurchase typeOfPurchase, double costPerSelection, int numberOfItemsSelected, string email,
            string URLReturnPaymentAccepted, string URLReturnCancel, string URLReturnPaymentDenied, string URL, SessionVariableManager svm, HttpResponse httpResponse,
            decimal totalCost)
            :
                base(parent,typeOfPurchase,costPerSelection, numberOfItemsSelected, email, URLReturnPaymentAccepted, URLReturnCancel,
                    URLReturnPaymentDenied) {
            _URL = URL;
            _svm = svm;
            _httpResponse = httpResponse;
            _svm.TypeOfPurchaseBeingMade = _typeOfPurchase;
            _totalCost = totalCost;
        }
        public override void Execute() {

            _svm.IFrameBusterURL = _URL + "?CostPerSelection=" + _costPerSelection.ToString("##.00") +
                "&NumberOfItemsSelected=" + _numberOfItemsSelected.ToString() +
                "&Email=" + _email +
                "&URLReturnPaymentAccepted=" + _URLReturnPaymentAccepted +
                "&URLReturnCancel=" + _URLReturnCancel +
                "&URLReturnPaymentDenied=" + _URLReturnPaymentDenied +
                "&TotalCost=" + _totalCost.ToString("##.00");
            _svm.Server.Transfer("IFrameBuster.aspx");
        }
    }
    public class PaymentMethodPaypal : PaymentManagerURL {
        private string _businessIdentifier;
        private string _customVariable;
        private string _notifyURL;
        public PaymentMethodPaypal(Control parent, TypeOfPurchase typeOfPurchase, double costPerSelection, int numberOfItemsSelected, string email, string URL, SessionVariableManager svm, HttpResponse httpResponse, string URLReturnPaymentAccepted, string URLReturnCancel,
                   string URLReturnPaymentDenied, string businessIdentifier, decimal totalCost, string customVariable, string notifyURL)
            :
            base(parent,typeOfPurchase, costPerSelection, numberOfItemsSelected, email, URLReturnPaymentAccepted, URLReturnCancel, URLReturnPaymentDenied, URL, svm, httpResponse,totalCost) {
            _businessIdentifier = businessIdentifier;
            _customVariable = customVariable;
            _notifyURL = notifyURL;
        }
        public override void Execute() {
            string itemName = "Job Posting";
            string url = _URL +
                "?" + "cmd=_xclick" +
                "&business=" + _businessIdentifier + //egout1_1205084684_biz@gmail.com" +
                "&item_name=" + itemName +
                "&item_number=1" +
                "&quantity=" + this._numberOfItemsSelected +
                "&amount=" + (_costPerSelection==0d?(Math.Round(_totalCost/_numberOfItemsSelected,2)).ToString():(this._costPerSelection * this._numberOfItemsSelected).ToString()) +
                "&no_shipping=1" +
                "&return=" + this._URLReturnPaymentAccepted +
                "&cancel_return=" + this._URLReturnCancel +
                "&currency_code=USD&lc=US&bn=PP-BuyNowBF&charset=UTF-8&custom=" + _customVariable
                +
                "&notify_url=" + _notifyURL
                ;
            _svm.IFrameBusterURL = url;
           _svm.Server.Transfer("IFrameBuster.aspx");
        }
    }
}
