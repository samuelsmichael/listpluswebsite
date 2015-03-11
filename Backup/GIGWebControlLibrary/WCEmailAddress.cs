using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Permissions;
using System.IO;
using System.Reflection;
using System.Data;

namespace GIGWebControlLibrary {
    #region Experimental classes
    [ToolboxData("<{0}:TDWithInputControl runat=server></{0}:TDWithInputControl")]
    [PersistChildren(true)]
    public class TDWithInputControl : CompositeControl {
        TextBox _valueTextBox;
        public override ControlCollection Controls {
            get {
                EnsureChildControls();
                return base.Controls;
            }
        }
        protected override void CreateChildControls() {
            Controls.Clear();
            _valueTextBox = new TextBox();
            _valueTextBox.Width = Width;
            _valueTextBox.Height = Height;
            Controls.Add(_valueTextBox);
            base.CreateChildControls();
        }
        protected override void RecreateChildControls() {
            EnsureChildControls();
        }
        protected override HtmlTextWriterTag TagKey {
            get {
                return HtmlTextWriterTag.Td;
            }
        }
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Value {
            get {
                return _valueTextBox.Text;
            }
            set {
                _valueTextBox.Text = value;
            }
        }
    }
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TDWithTextControl runat=server></{0}:TDWithTextControl")]
    public class TDWithTextControl : WebControl {
        [Bindable(false)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text {
            get {
                String s = (String)ViewState["Text"];
                return ((s == null || s.Trim().Equals(string.Empty)) ? "&nbsp;" : s);
            }
            set {
                ViewState["Text"] = value;
            }
        }
        protected override HtmlTextWriterTag TagKey {
            get {
                return HtmlTextWriterTag.Td;
            }
        }
        protected override void RenderContents(HtmlTextWriter writer) {
            writer.Write(Text);
        }
    }

    [ToolboxData("<{0}:ThreeColumnedPrompt runat=server></{0}:ThreeColumnedPrompt")]
    public class ThreeColumnedPrompt : CompositeControl {
        public override ControlCollection Controls {
            get {
                EnsureChildControls();
                return base.Controls;
            }
        }

        [Bindable(false)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Prompt {
            get {
                String s = (String)ViewState["Prompt"];
                return ((s == null) ? String.Empty : s);
            }

            set {
                ViewState["Prompt"] = value;
            }
        }
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Value {
            get {
                String s = (String)ViewState["Value"];
                return ((s == null) ? String.Empty : s);
            }
            set {
                ViewState["Value"] = value;
            }
        }
        [Bindable(false)]
        [Category("Behavior")]
        [DefaultValue(true)]
        [Localizable(false)]
        public bool IsARequiredField {
            get {
                if (ViewState["IsARequiredField"] == null) {
                    ViewState["IsARequiredField"] = true;
                }
                return (bool)ViewState["IsARequiredField"];
            }
            set {
                ViewState["IsARequiredField"] = value;
            }
        }
        [Bindable(false)]
        [Category("Behavior")]
        [DefaultValue("")]
        [Localizable(true)]
        public string InvalidValue_ErrorMessage {
            get {
                String s = (String)ViewState["InvalidValue_ErrorMessage"];
                return ((s == null) ? String.Empty : s);
            }
            set {
                ViewState["InvalidValue_ErrorMessage"] = value;
            }
        }
        protected override HtmlTextWriterTag TagKey {
            get {
                return HtmlTextWriterTag.Tr;
            }
        }
        protected override void CreateChildControls() {
            TDWithTextControl td = new TDWithTextControl();
            td.Text = Prompt;
            Controls.Add(td);
            TDWithInputControl tdi = new TDWithInputControl();
            Controls.Add(tdi);
            tdi.Value = Value;
            TDWithTextControl td2 = new TDWithTextControl();
            Controls.Add(td2);
            td2.ForeColor = System.Drawing.Color.Red;
            if (tdi.Value == null || tdi.Value.Trim().Equals(string.Empty)) {
                td2.Text = "*";
            } else {
                td2.Text = "";
            }
            base.CreateChildControls();
        }
    }

 // Register.cs

    [
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, 
        Level=AspNetHostingPermissionLevel.Minimal),
    DefaultEvent("Submit"),
    DefaultProperty("ButtonText"),
    ToolboxData("<{0}:Register runat=\"server\"> </{0}:Register>"),
    ]
    public class Register : CompositeControl
    {
        private Button submitButton;
        private TextBox nameTextBox;
        private Label nameLabel;
        private TextBox emailTextBox;
        private Label emailLabel;
        private RequiredFieldValidator emailValidator;
        private RequiredFieldValidator nameValidator;

        private static readonly object EventSubmitKey = 
            new object();

        // The following properties are delegated to 
        // child controls.
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("The text to display on the button.")
        ]
        public string ButtonText
        {
            get
            {
                EnsureChildControls();
                return submitButton.Text;
            }
            set
            {
                EnsureChildControls();
                submitButton.Text = value;
            }
        }

        [
        Bindable(true),
        Category("Default"),
        DefaultValue(""),
        Description("The user name.")
        ]
        public string Name
        {
            get
            {
                EnsureChildControls();
                return nameTextBox.Text;
            }
            set
            {
                EnsureChildControls();
                nameTextBox.Text = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description(
            "Error message for the name validator.")
        ]
        public string NameErrorMessage
        {
            get
            {
                EnsureChildControls();
                return nameValidator.ErrorMessage;
            }
            set
            {
                EnsureChildControls();
                nameValidator.ErrorMessage = value;
                nameValidator.ToolTip = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("The text for the name label.")
        ]
        public string NameLabelText
        {
            get
            {
                EnsureChildControls();
                return nameLabel.Text;
            }
            set
            {
                EnsureChildControls();
                nameLabel.Text = value;
            }
        }

        [
        Bindable(true),
        Category("Default"),
        DefaultValue(""),
        Description("The e-mail address.")
        ]
        public string Email
        {
            get
            {
                EnsureChildControls();
                return emailTextBox.Text;
            }
            set
            {
                EnsureChildControls();
                emailTextBox.Text = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description(
            "Error message for the e-mail validator.")
        ]
        public string EmailErrorMessage
        {
            get
            {
                EnsureChildControls();
                return emailValidator.ErrorMessage;
            }
            set
            {
                EnsureChildControls();
                emailValidator.ErrorMessage = value;
                emailValidator.ToolTip = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("The text for the e-mail label.")
        ]
        public string EmailLabelText
        {
            get
            {
                EnsureChildControls();
                return emailLabel.Text;
            }
            set
            {
                EnsureChildControls();
                emailLabel.Text = value;

            }
        }

        // The Submit event.
        [
        Category("Action"),
        Description("Raised when the user clicks the button.")
        ]
        public event EventHandler Submit
        {
            add
            {
                Events.AddHandler(EventSubmitKey, value);
            }
            remove
            {
                Events.RemoveHandler(EventSubmitKey, value);
            }
        }

        // The method that raises the Submit event.
        protected virtual void OnSubmit(EventArgs e)
        {
            EventHandler SubmitHandler =
                (EventHandler)Events[EventSubmitKey];
            if (SubmitHandler != null)
            {
                SubmitHandler(this, e);
            }
        }

        // Handles the Click event of the Button and raises
        // the Submit event.
        private void _button_Click(object source, EventArgs e)
        {
            OnSubmit(EventArgs.Empty);
        }

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }


        protected override void CreateChildControls()
        {
            Controls.Clear();

            nameLabel = new Label();

            nameTextBox = new TextBox();
            nameTextBox.ID = "nameTextBox";

            nameValidator = new RequiredFieldValidator();
            nameValidator.ID = "validator1";
            nameValidator.ControlToValidate = nameTextBox.ID;
            nameValidator.Text = "Failed validation.";
            nameValidator.Display = ValidatorDisplay.Static;

            emailLabel = new Label();

            emailTextBox = new TextBox();
            emailTextBox.ID = "emailTextBox";

            emailValidator = new RequiredFieldValidator();
            emailValidator.ID = "validator2";
            emailValidator.ControlToValidate = 
                emailTextBox.ID;
            emailValidator.Text = "Failed validation.";
            emailValidator.Display = ValidatorDisplay.Static;

            submitButton = new Button();
            submitButton.ID = "button1";
            submitButton.Click 
                += new EventHandler(_button_Click);

            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(nameValidator);
            this.Controls.Add(emailLabel);
            this.Controls.Add(emailTextBox);
            this.Controls.Add(emailValidator);
            this.Controls.Add(submitButton);
        }


        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);

            writer.AddAttribute(
                HtmlTextWriterAttribute.Cellpadding,
                "1", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            nameLabel.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            nameTextBox.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            nameValidator.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            emailLabel.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            emailTextBox.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            emailValidator.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(
                HtmlTextWriterAttribute.Colspan, 
                "3", false);
            writer.AddAttribute(
                HtmlTextWriterAttribute.Align, 
                "center", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            submitButton.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();
        }
    }


    #endregion
    [
    ToolboxData("<{0}:DDSignUp runat=server></{0}:DDSignUp"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class DDSignUp : UserProfile {
        private ListBox _TypesOfSubscriptions = new ListBox();
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(true),
        Description(
            "Show Subscriptions Box")
        ]
        public bool ShowSubscriptionsBox {
            get {
                EnsureChildControls();
                object b = ViewState["ShowSubscriptionsBox"];
                return ((b == null) ? true : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["ShowSubscriptionsBox"] = value;
            }
        }
        protected override void hookForStuffBeforeButtons() {
        }
        public void addSubscriptionsListboxItem(ListItem li) {
            _TypesOfSubscriptions.Items.Add(li);
        }
        public int numberOfItemsInList() {
            return _TypesOfSubscriptions.Items.Count;
        }
        protected override void hookToRenderStuffBeforeButtons(HtmlTextWriter writer) {
            if (ShowSubscriptionsBox) {
                this.writeARow(writer, _TypesOfSubscriptions,"Choose a subscription: ");
            }
        }
        public string getSelectedValue() {
            if (ShowSubscriptionsBox) {
                return _TypesOfSubscriptions.SelectedValue;
            } else {
                return "-1";
            }
        }
        public void setSelectedValue(string selectedValue) {
            if (ShowSubscriptionsBox) {
                _TypesOfSubscriptions.SelectedValue = selectedValue;
            }
        }
    }
    [
    ToolboxData("<{0}:UserProfile runat=server></{0}:FileViewer"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class FileViewer : CompositeControl {
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(""),
        Description("The file to display.")
        ]
        public string FileSpec {
            get {
                EnsureChildControls();
                object w = ViewState["FileSpec"];
                return (w == null) ? "" : (String)w;
            }
            set {
                EnsureChildControls();
                ViewState["FileSpec"] = value;
            }
        }
        public string FileSpec2 {
            get {
                EnsureChildControls();
                object w = ViewState["FileSpec2"];
                return (w == null) ? "" : (String)w;
            }
            set {
                EnsureChildControls();
                ViewState["FileSpec2"] = value;
            }
        }
        protected override void RecreateChildControls() {
            EnsureChildControls();
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();
        }
        protected override void Render(HtmlTextWriter writer) {
            StreamReader sr=null;
            byte[] ba=new byte[1000];
            char[] ca=new char[1000];
            int nbrRead=0;
            bool forceToFileSystem = false;
            try {
                if (forceToFileSystem) {
                    throw new Exception("phony");
                }
                System.Net.WebRequest wr = System.Net.WebRequest.Create(FileSpec2);
                Stream str=wr.GetResponse().GetResponseStream();
                nbrRead=str.Read(ba,0,1000);
                System.Text.Decoder dec = Encoding.Default.GetDecoder();
                while (nbrRead>0) {
                    dec.GetChars(ba,0,nbrRead,ca,0);
                    writer.Write(ca,0,nbrRead);
                    nbrRead=str.Read(ba,0,1000);
                } 
            } catch (Exception ee3) {
                sr = new StreamReader(FileSpec2);
                nbrRead = sr.Read(ca, 0, 1000);
                while (nbrRead > 0) {
                    writer.Write(ca, 0, nbrRead);
                    nbrRead = sr.Read(ca, 0, 1000);
                }
            } finally {
                try {
                    sr.Close();
                } catch {}
            }
        }
    }
    public class FileViewerWithFormating : FileViewer {
        private Dictionary<string, string> _substitutions;
        public Dictionary<string,string> Substitutions {
            set { _substitutions = value; }
        }
        protected override void Render(HtmlTextWriter writer) {
            try {
                StreamReader sr = null;
                StringBuilder sa = new StringBuilder();
                byte[] ba = new byte[1000];
                char[] ca = new char[1000];
                int nbrRead = 0;
                bool forceToLocalFileSystem = false;
                try {
                    if (forceToLocalFileSystem ) {
                        throw new Exception("phony");
                    }
                    System.Net.WebRequest wr = System.Net.WebRequest.Create(FileSpec);
                    Stream str = wr.GetResponse().GetResponseStream();
                    nbrRead = str.Read(ba, 0, 1000);
                    System.Text.Decoder dec = Encoding.Default.GetDecoder();
                    while (nbrRead > 0) {
                        dec.GetChars(ba, 0, nbrRead, ca, 0);
                        sa.Append(ca, 0, nbrRead);
                        nbrRead = str.Read(ba, 0, 1000);
                    }
                } catch (Exception ekk3) {
                    sr = new StreamReader(FileSpec2);
                    nbrRead = sr.Read(ca, 0, 1000);
                    while (nbrRead > 0) {
                        sa.Append(ca, 0, nbrRead);
                        nbrRead = sr.Read(ca, 0, 1000);
                    }

                } finally {
                    try {
                        sr.Close();
                    } catch { }
                }
                string text = sa.ToString();
                foreach (string substitutionKey in _substitutions.Keys) {
                    text = text.Replace("&&" + substitutionKey, _substitutions[substitutionKey]);
                }
                writer.Write(text);
            } catch { }
        }
    }
    [
    ToolboxData("<{0}:UserProfile runat=server></{0}:UserProfile"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class UserProfile : CompositeControl {
        private ThreeColumnedPromptV2 _userID;
        CompareTwoThreeColumnedPromptsV2 _passwords;
        private EmailThreeColumnedPrompt _email;
        private ThreeColumnedPromptV2 _question;
        private ThreeColumnedPromptV2 _answer;
        private ThreeColumnedPromptV2 _firstName;
        private ThreeColumnedPromptV2 _lastName;
        private Button submitButton;
        private Button cancelButton;
        private static readonly object EventSubmitKey =
            new object();
        private static readonly object EventCancelKey =
            new object();
        #region Public Attributes
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Last name"),
        Description("The text to display for the Last Name prompt.")
        ]
        public string LastNamePromptText {
            get {
                EnsureChildControls();
                object w = ViewState["LastNamePromptText"];
                return (w == null) ? "Last name" : (String)w;
            }
            set {
                EnsureChildControls();
                ViewState["LastNamePromptText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("First name"),
        Description("The text to display for the First Name prompt.")
        ]
        public string FirstNamePromptText {
            get {
                EnsureChildControls();
                object w = ViewState["FirstNamePromptText"];
                return (w == null) ? "First name" : (String)w;
            }
            set {
                EnsureChildControls();
                ViewState["FirstNamePromptText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Enter new password"),
        Description("The text to display for the Password prompt.")
        ]
        public string PasswordPromptText {
            get {
                EnsureChildControls();
                object w = ViewState["PasswordPromptText"];
                return (w == null) ? "Enter new password" : (String)w;
            }
            set {
                EnsureChildControls();
                ViewState["PasswordPromptText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Submit"),
        Description("The text to display on the Submit button.")
        ]
        public string ButtonText {
            get {
                object w = ViewState["ButtonText"];
                return (w == null) ? "Submit" : (String)w;                
            }
            set {
                EnsureChildControls();
                ViewState["ButtonText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Cancel"),
        Description("The text to display on the Cancel button.")
        ]
        public string CancelButtonText {
            get {
                object w = ViewState["CancelButtonText"];
                return (w == null) ? "Cancel" : (String)w;
            }
            set {
                EnsureChildControls();
                ViewState["CancelButtonText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(111),
        Description(
            "Width of prompt")
        ]
        public Unit PromptWidth {
            get {
                EnsureChildControls();
                object w = ViewState["PromptWidth"];
                return (w == null) ? 111 : (Unit)w;
            }
            set {
                EnsureChildControls();
                ViewState["PromptWidth"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(133),
        Description(
            "Width of error messages")
        ]
        public Unit ErrorMessagesWidth {
            get {
                EnsureChildControls();
                object w = ViewState["ErrorMessagesWidth"];
                return (w == null) ? 133 : (Unit)w;
            }
            set {
                EnsureChildControls();
                ViewState["ErrorMessagesWidth"] = value;
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description(
            "Show first and last name?")
        ]
        public bool ShowFirstAndLastName {
            get {
                EnsureChildControls();
                object b = ViewState["ShowFirstAndLastName"];
                return ((b == null) ? false : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["ShowFirstAndLastName"] = value;
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description(
            "User Name is an email address?")
        ]
        public bool UserNameIsAnEmailAddress {
            get {
                EnsureChildControls();
                object b = ViewState["UserNameIsAnEmailAddress"];
                return ((b == null) ? false : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["UserNameIsAnEmailAddress"] = value;
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(true),
        Description(
            "Is last name required?")
        ]
        public bool IsLastNameRequired {
            get {
                EnsureChildControls();
                object b = ViewState["IsLastNameRequired"];
                return ((b == null) ? true : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["IsLastNameRequired"] = value;
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(true),
        Description(
            "Is first name required?")
        ]
        public bool IsFirstNameRequired {
            get {
                EnsureChildControls();
                object b = ViewState["IsFirstNameRequired"];
                return ((b == null) ? true : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["IsFirstNameRequired"] = value;
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description(
            "Is email required?")
        ]
        public bool IsEmailRequired {
            get {
                if (UserNameIsAnEmailAddress == true) {
                    return true;
                } else {
                    EnsureChildControls();
                    object b = ViewState["IsEmailRequired"];
                    return ((b == null) ? false : (bool)b);
                }
            }
            set {
                EnsureChildControls();
                ViewState["IsEmailRequired"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description(
            "Message")
        ]
        public string Message {
            get {
                String s = (String)ViewState["Message"];
                return ((s == null) ? "" : s);
            }
            set {
                ViewState["Message"] = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(false),
        Description(
            "Is message an error")
        ]
        public bool IsMessageAnError {
            get {
                object b = ViewState["IsMessageAnError"];
                return ((b == null) ? false : (bool)b);
            }
            set {
                ViewState["IsMessageAnError"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Passwords do not match."),
        Description(
            "Error message for the mismatching passwords.")
        ]
        public string MismatchingPasswordsErrorMessage
        {
            get {
                String s = (String)ViewState["MismatchingPasswordsErrorMessage"];
                return ((s == null) ? "Passwords do not match" : s);
            }
            set {
                ViewState["MismatchingPasswordsErrorMessage"] = value;
            }
        }
#endregion
#region Public Interface
        public void userFocus() {
            try {
                _userID.Focus();
            } catch { }
        }
        public void passwordsFocus() {
            try {
                _passwords.Focus();
            } catch { }
        }
        public void setUserID(string userID) {
            _userID.Value = userID;
        }
        public void setFirstName(string firstName) {
            _firstName.Value = firstName;
        }
        public void setLastName(string lastName) {
            _lastName.Value = lastName;
        }
        public string getFirstName() {
            return _firstName.Value.Trim();
        }
        public string getLastName() {
            return _lastName.Value.Trim();
        }
        public string getUserID() {
            return _userID.Value.Trim();
        }
        public string getPassword() {
            return _passwords.getValue1().Trim();
        }
        public void clearPasswords() {
            _passwords.clearValues();
        }
        public void setEmail(string email) {
            _email.Value = email;
        }
        public void setPassword(string password) {
            _passwords.setTextBox1(password);
            _passwords.setTextBox2(password);
        }
        public string getEmail() {
            return _email.Value.Trim();
        }
        public void setSecurityQuestion(string question) {
            _question.Value = question;
        }
        public string getSecurityQuestion() {
            return _question.Value.Trim(); ;
        }
        public void setSecurityAnswer(string answer) {
            _answer.Value = answer;
        }
        public string getSecurityAnswer() {
            return _answer.Value.Trim();
        }
#endregion
#region Events
        // The Submit event.
        [
        Category("Action"),
        Description("Raised when the user clicks the Submit button.")
        ]
        public event EventHandler Submit {
            add {
                Events.AddHandler(EventSubmitKey, value);
            }
            remove {
                Events.RemoveHandler(EventSubmitKey, value);
            }
        }

        // The method that raises the Submit event.
        protected virtual void OnSubmit(EventArgs e) {
            EventHandler SubmitHandler =
                (EventHandler)Events[EventSubmitKey];
            if (SubmitHandler != null) {
                SubmitHandler(this, e);
            }
        }
        // Handles the Click event of the Button and raises
        // the Submit event.
        private void _button_Click(object source, EventArgs e) {
            OnSubmit(EventArgs.Empty);
        }
        // The Cancel event.
        [
        Category("Action"),
        Description("Raised when the user clicks the cancel button.")
        ]
        public event EventHandler Cancel {
            add {
                Events.AddHandler(EventCancelKey, value);
            }
            remove {
                Events.RemoveHandler(EventCancelKey, value);
            }
        }
                // The method that raises the Cancel event.
        protected virtual void OnCancel(EventArgs e) {
            EventHandler CancelHandler =
                (EventHandler)Events[EventCancelKey];
            if (CancelHandler != null) {
                CancelHandler(this, e);
            }
        }
        void  _cancelButton_Click(object source, EventArgs e) {
            OnCancel(EventArgs.Empty);
        }
#endregion

        protected override void RecreateChildControls() {
            EnsureChildControls();
        } 
        private void CreateChildControlsUserStuff() {
            _userID = new ThreeColumnedPromptV2();
            _userID.IsRequiredField = true;
            _userID.PromptLabelText = "Login name";
            _passwords = new CompareTwoThreeColumnedPromptsV2();
            _passwords.IsRequiredField = true;
            _passwords.PromptLabelText1 = PasswordPromptText;
            _passwords.PromptLabelText2 = "Re-key password";
            _passwords.CompareErrorMessage = MismatchingPasswordsErrorMessage;

            _email = new EmailThreeColumnedPrompt();
            _email.PromptLabelText = "Email";
            _question = new ThreeColumnedPromptV2();
            _question.PromptLabelText = "Security question";
            _answer = new ThreeColumnedPromptV2();
            _answer.PromptLabelText = "Security answer";
            _firstName = new ThreeColumnedPromptV2();
            _firstName.PromptLabelText = FirstNamePromptText;
            _firstName.IsRequiredField = true;
            _lastName = new ThreeColumnedPromptV2();
            _lastName.PromptLabelText = LastNamePromptText;
            _lastName.IsRequiredField = true;
            if (UserNameIsAnEmailAddress == false) {
                Controls.Add(_userID);
                Controls.Add(_passwords);
                Controls.Add(_email);
            } else {
                Controls.Add(_email);
                Controls.Add(_passwords);
            }
            if (ShowFirstAndLastName) {
                Controls.Add(_firstName);
                Controls.Add(_lastName);
            }
            Controls.Add(_question);
            Controls.Add(_answer);
        }
        private void CreateChildControlsButtons() {
            cancelButton = new Button();
            cancelButton.ID="button2";
            cancelButton.CausesValidation = false;
            /* Here's with using javascript */
//            string goback = "javascript:window.history.go(-1)";
//            cancelButton.Attributes.Add("OnClick", goback);
            /* Here's with using code behind */
            cancelButton.Click 
                +=new EventHandler(_cancelButton_Click);

            submitButton = new Button();
            submitButton.ID = "button1";
            submitButton.Click
                += new EventHandler(_button_Click);

            Controls.Add(submitButton);
            Controls.Add(cancelButton);
        }
        protected virtual void hookForStuffBeforeButtons() {
        }
        protected override void CreateChildControls() {
            CreateChildControlsUserStuff();
            hookForStuffBeforeButtons();
            CreateChildControlsButtons();
            base.CreateChildControls();
        }
        protected void writeARow(HtmlTextWriter writer, WebControl control, string heading) {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(heading+"<br>");
            control.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected void writeARow(HtmlTextWriter writer, WebControl control) {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            control.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
        private void RenderUserStuff(HtmlTextWriter writer) {
            if (UserNameIsAnEmailAddress == false) {
                writeARow(writer, _userID);
                writeARow(writer, _passwords);
                writeARow(writer, _email);
            } else {
                writeARow(writer, _email);
                writeARow(writer, _passwords);
            }
            if (ShowFirstAndLastName) {
                writeARow(writer, _firstName);
                writeARow(writer, _lastName);
            }
            writeARow(writer, _question);
            writeARow(writer, _answer);
        }
        private void RenderButtons(HtmlTextWriter writer) {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(
                HtmlTextWriterAttribute.Colspan,
                "3", false);
            writer.AddAttribute(
                HtmlTextWriterAttribute.Align,
                "center", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            submitButton.RenderControl(writer);
            writer.Write("&nbsp;&nbsp;");
            cancelButton.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
        private void manageUpdatingControls() {
            if (PromptWidth != 0) {
                _userID.WidthLeftColumn = PromptWidth;
                _passwords.WidthLeftColumn = PromptWidth;
                _email.WidthLeftColumn = PromptWidth;
                _question.WidthLeftColumn = PromptWidth;
                _answer.WidthLeftColumn = PromptWidth;
                if (ShowFirstAndLastName) {
                    _lastName.WidthLeftColumn = PromptWidth;
                    _firstName.WidthLeftColumn = PromptWidth;
                }
            }
            if (ErrorMessagesWidth != 0) {
                _userID.WidthRightColumn = ErrorMessagesWidth;
                _passwords.WidthRightColumn = ErrorMessagesWidth;
                _email.WidthRightColumn = ErrorMessagesWidth;
                _question.WidthRightColumn = ErrorMessagesWidth;
                _answer.WidthRightColumn = ErrorMessagesWidth;
                if (ShowFirstAndLastName) {
                    _firstName.WidthRightColumn = ErrorMessagesWidth;
                    _lastName.WidthRightColumn = ErrorMessagesWidth;
                }
            }
            _email.IsRequiredField = IsEmailRequired;
            _question.IsRequiredField = IsEmailRequired;
            _answer.IsRequiredField = IsEmailRequired;
            _passwords.PromptLabelText1 = PasswordPromptText;
            _firstName.IsRequiredField = IsFirstNameRequired;
            _lastName.IsRequiredField = IsLastNameRequired;
            cancelButton.Text = CancelButtonText;
            submitButton.Text = ButtonText;
        }
        protected virtual void hookToRenderStuffBeforeButtons(HtmlTextWriter writer) {
        }
        protected override void Render(HtmlTextWriter writer) {
            manageUpdatingControls();
            AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
            if (IsMessageAnError == true) {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold");
            } else {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "black");
                writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "normal");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.WriteEncodedText(Message);
            writer.RenderEndTag();
            writer.AddAttribute(
                HtmlTextWriterAttribute.Cellpadding,
                "1", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            RenderUserStuff(writer);
            hookToRenderStuffBeforeButtons(writer);
            RenderButtons(writer);
            writer.RenderEndTag();
        }
    }
    public class UserProfileWithNames : CompositeControl {
        CompareTwoThreeColumnedPromptsV2 _passwords;
        private EmailThreeColumnedPrompt _email;
        private ThreeColumnedPromptV2 _question;
        private ThreeColumnedPromptV2 _answer;
        private ThreeColumnedPromptV2 _firstName;
        private ThreeColumnedPromptV2 _lastName;
        private Button submitButton;
        private Button cancelButton;
        private static readonly object EventSubmitKey =
            new object();
        private static readonly object EventCancelKey =
            new object();
        #region Public Attributes
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Last name"),
        Description("The text to display for the Last Name prompt.")
        ]
        public string LastNamePromptText {
            get {
                EnsureChildControls();
                object w = ViewState["LastNamePromptText"];
                return (w == null) ? "Last name" : (String)w;
            }
            set {
                EnsureChildControls();
                ViewState["LastNamePromptText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("First name"),
        Description("The text to display for the First Name prompt.")
        ]
        public string FirstNamePromptText {
            get {
                EnsureChildControls();
                object w = ViewState["FirstNamePromptText"];
                return (w == null) ? "First name" : (String)w;
            }
            set {
                EnsureChildControls();
                ViewState["FirstNamePromptText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Enter new password"),
        Description("The text to display for the Password prompt.")
        ]
        public string PasswordPromptText {
            get {
                EnsureChildControls();
                object w = ViewState["PasswordPromptText"];
                return (w == null) ? "Enter new password" : (String)w;
            }
            set {
                EnsureChildControls();
                ViewState["PasswordPromptText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Submit"),
        Description("The text to display on the Submit button.")
        ]
        public string ButtonText {
            get {
                object w = ViewState["ButtonText"];
                return (w == null) ? "Submit" : (String)w;
            }
            set {
                EnsureChildControls();
                ViewState["ButtonText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Cancel"),
        Description("The text to display on the Cancel button.")
        ]
        public string CancelButtonText {
            get {
                object w = ViewState["CancelButtonText"];
                return (w == null) ? "Cancel" : (String)w;
            }
            set {
                EnsureChildControls();
                ViewState["CancelButtonText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(111),
        Description(
            "Width of prompt")
        ]
        public Unit PromptWidth {
            get {
                EnsureChildControls();
                object w = ViewState["PromptWidth"];
                return (w == null) ? 111 : (Unit)w;
            }
            set {
                EnsureChildControls();
                ViewState["PromptWidth"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(133),
        Description(
            "Width of error messages")
        ]
        public Unit ErrorMessagesWidth {
            get {
                EnsureChildControls();
                object w = ViewState["ErrorMessagesWidth"];
                return (w == null) ? 133 : (Unit)w;
            }
            set {
                EnsureChildControls();
                ViewState["ErrorMessagesWidth"] = value;
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(true),
        Description(
            "Is first name required?")
        ]
        public bool IsFirstNameRequired {
            get {
                EnsureChildControls();
                object b = ViewState["IsFirstNameRequired"];
                return ((b == null) ? true : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["IsFirstNameRequired"] = value;
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(true),
        Description(
            "Is last name required?")
        ]
        public bool IsLastNameRequired {
            get {
                EnsureChildControls();
                object b = ViewState["IsLastNameRequired"];
                return ((b == null) ? true : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["IsLastNameRequired"] = value;
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description(
            "Is email required?")
        ]
        public bool IsEmailRequired {
            get {
                    EnsureChildControls();
                    object b = ViewState["IsEmailRequired"];
                    return ((b == null) ? false : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["IsEmailRequired"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description(
            "Message")
        ]
        public string Message {
            get {
                String s = (String)ViewState["Message"];
                return ((s == null) ? "" : s);
            }
            set {
                ViewState["Message"] = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(false),
        Description(
            "Is message an error")
        ]
        public bool IsMessageAnError {
            get {
                object b = ViewState["IsMessageAnError"];
                return ((b == null) ? false : (bool)b);
            }
            set {
                ViewState["IsMessageAnError"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Passwords do not match."),
        Description(
            "Error message for the mismatching passwords.")
        ]
        public string MismatchingPasswordsErrorMessage {
            get {
                String s = (String)ViewState["MismatchingPasswordsErrorMessage"];
                return ((s == null) ? "Passwords do not match" : s);
            }
            set {
                ViewState["MismatchingPasswordsErrorMessage"] = value;
            }
        }
        #endregion
        #region Public Interface
        public void emailFocus() {
            try {
                _email.Focus();
            } catch { }
        }
        public void passwordsFocus() {
            try {
                _passwords.Focus();
            } catch { }
        }
        public void setFirstName(string firstName) {
            _firstName.Value = firstName;
        }
        public void setLastName(string lastName) {
            _lastName.Value = lastName;
        }
        public string getFirstName() {
            return _firstName.Value.Trim();
        }
        public string getLastName() {
            return _lastName.Value.Trim();
        }
        public string getPassword() {
            return _passwords.getValue1().Trim();
        }
        public void clearPasswords() {
            _passwords.clearValues();
        }
        public void setEmail(string email) {
            _email.Value = email;
        }
        public void setPassword(string password) {
            _passwords.setTextBox1(password);
            _passwords.setTextBox2(password);
        }
        public string getEmail() {
            return _email.Value.Trim();
        }
        public void setSecurityQuestion(string question) {
            _question.Value = question;
        }
        public string getSecurityQuestion() {
            return _question.Value.Trim(); ;
        }
        public void setSecurityAnswer(string answer) {
            _answer.Value = answer;
        }
        public string getSecurityAnswer() {
            return _answer.Value.Trim();
        }
#endregion
        #region Events
        // The Submit event.
        [
        Category("Action"),
        Description("Raised when the user clicks the Submit button.")
        ]
        public event EventHandler Submit {
            add {
                Events.AddHandler(EventSubmitKey, value);
            }
            remove {
                Events.RemoveHandler(EventSubmitKey, value);
            }
        }

        // The method that raises the Submit event.
        protected virtual void OnSubmit(EventArgs e) {
            EventHandler SubmitHandler =
                (EventHandler)Events[EventSubmitKey];
            if (SubmitHandler != null) {
                SubmitHandler(this, e);
            }
        }
        // Handles the Click event of the Button and raises
        // the Submit event.
        private void _button_Click(object source, EventArgs e) {
            OnSubmit(EventArgs.Empty);
        }
        // The Cancel event.
        [
        Category("Action"),
        Description("Raised when the user clicks the cancel button.")
        ]
        public event EventHandler Cancel {
            add {
                Events.AddHandler(EventCancelKey, value);
            }
            remove {
                Events.RemoveHandler(EventCancelKey, value);
            }
        }
        // The method that raises the Cancel event.
        protected virtual void OnCancel(EventArgs e) {
            EventHandler CancelHandler =
                (EventHandler)Events[EventCancelKey];
            if (CancelHandler != null) {
                CancelHandler(this, e);
            }
        }
        void _cancelButton_Click(object source, EventArgs e) {
            OnCancel(EventArgs.Empty);
        }
#endregion

        protected override void RecreateChildControls() {
            EnsureChildControls();
        }
        private void CreateChildControlsUserStuff() {
            _passwords = new CompareTwoThreeColumnedPromptsV2();
            _passwords.IsRequiredField = true;
            _passwords.PromptLabelText1 = PasswordPromptText;
            _passwords.PromptLabelText2 = "Re-key password";
            _passwords.CompareErrorMessage = MismatchingPasswordsErrorMessage;

            _email = new EmailThreeColumnedPrompt();
            _email.PromptLabelText = "Email";
            _question = new ThreeColumnedPromptV2();
            _question.PromptLabelText = "Security question";
            _answer = new ThreeColumnedPromptV2();
            _answer.PromptLabelText = "Security answer";
            _firstName = new ThreeColumnedPromptV2();
            _firstName.PromptLabelText = "First name";
            _firstName.IsRequiredField = true;
            _lastName = new ThreeColumnedPromptV2();
            _lastName.PromptLabelText = "Last name";
            _lastName.IsRequiredField = true;
                Controls.Add(_email);
                Controls.Add(_passwords);
            Controls.Add(_firstName);
            Controls.Add(_lastName);
            Controls.Add(_question);
            Controls.Add(_answer);
        }
        private void CreateChildControlsButtons() {
            cancelButton = new Button();
            cancelButton.ID = "button2";
            cancelButton.CausesValidation = false;
            /* Here's with using javascript */
            //            string goback = "javascript:window.history.go(-1)";
            //            cancelButton.Attributes.Add("OnClick", goback);
            /* Here's with using code behind */
            cancelButton.Click
                += new EventHandler(_cancelButton_Click);

            submitButton = new Button();
            submitButton.ID = "button1";
            submitButton.Click
                += new EventHandler(_button_Click);

            Controls.Add(submitButton);
            Controls.Add(cancelButton);
        }
        protected virtual void hookForStuffBeforeButtons() {
        }
        protected override void CreateChildControls() {
            CreateChildControlsUserStuff();
            hookForStuffBeforeButtons();
            CreateChildControlsButtons();
            base.CreateChildControls();
        }
        protected void writeARow(HtmlTextWriter writer, WebControl control, string heading) {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(heading + "<br>");
            control.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected void writeARow(HtmlTextWriter writer, WebControl control) {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            control.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
        private void RenderUserStuff(HtmlTextWriter writer) {
                writeARow(writer, _email);
                writeARow(writer, _passwords);
            writeARow(writer, _firstName);
            writeARow(writer, _lastName);
            writeARow(writer, _question);
            writeARow(writer, _answer);
        }
        private void RenderButtons(HtmlTextWriter writer) {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(
                HtmlTextWriterAttribute.Colspan,
                "3", false);
            writer.AddAttribute(
                HtmlTextWriterAttribute.Align,
                "center", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            submitButton.RenderControl(writer);
            writer.Write("&nbsp;&nbsp;");
            cancelButton.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
        private void manageUpdatingControls() {
            if (PromptWidth != 0) {
                _passwords.WidthLeftColumn = PromptWidth;
                _email.WidthLeftColumn = PromptWidth;
                _question.WidthLeftColumn = PromptWidth;
                _answer.WidthLeftColumn = PromptWidth;
                    _lastName.WidthLeftColumn = PromptWidth;
                    _firstName.WidthLeftColumn = PromptWidth;
            }
            if (ErrorMessagesWidth != 0) {
                _passwords.WidthRightColumn = ErrorMessagesWidth;
                _email.WidthRightColumn = ErrorMessagesWidth;
                _question.WidthRightColumn = ErrorMessagesWidth;
                _answer.WidthRightColumn = ErrorMessagesWidth;
                    _firstName.WidthRightColumn = ErrorMessagesWidth;
                    _lastName.WidthRightColumn = ErrorMessagesWidth;
            }
            _email.IsRequiredField = IsEmailRequired;
            _question.IsRequiredField = IsEmailRequired;
            _answer.IsRequiredField = IsEmailRequired;
            _passwords.PromptLabelText1 = PasswordPromptText;
            _firstName.IsRequiredField = IsFirstNameRequired;
            _lastName.IsRequiredField = IsLastNameRequired;
            cancelButton.Text = CancelButtonText;
            submitButton.Text = ButtonText;
        }
        protected virtual void hookToRenderStuffBeforeButtons(HtmlTextWriter writer) {
        }
        protected override void Render(HtmlTextWriter writer) {
            manageUpdatingControls();
            AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
            if (IsMessageAnError == true) {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold");
            } else {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "black");
                writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "normal");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.WriteEncodedText(Message);
            writer.RenderEndTag();
            writer.AddAttribute(
                HtmlTextWriterAttribute.Cellpadding,
                "1", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            RenderUserStuff(writer);
            hookToRenderStuffBeforeButtons(writer);
            RenderButtons(writer);
            writer.RenderEndTag();
        }
    }

    [
    ToolboxData("<{0}:EmailThreeColumnedPrompt runat=server></{0}:EmailThreeColumnedPrompt"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class EmailThreeColumnedPrompt : ThreeColumnedPromptV2 {
        private RegularExpressionValidator _EmailValidator;
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Invalid email address"),
        Description(
            "Error message for invalid email validator.")
        ]
        public string EmailErrorMessage {
            get {
                EnsureChildControls();
                String s = (String)ViewState["EmailErrorMessage"];
                return ((s == null) ? "Invalid email address" : s);
            }
            set {
                EnsureChildControls();
                ViewState["EmailErrorMessage"] = value;
            }
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();
            _EmailValidator = new RegularExpressionValidator();
            _EmailValidator.ID = "emailValidator1BBH1";
            _EmailValidator.ControlToValidate = _valueTextBox.ID;
            _EmailValidator.Display = ValidatorDisplay.Dynamic;
            _EmailValidator.ValidationExpression=@"([\w-]+(?:\.[\w-]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7};?)+";
            _EmailValidator.SetFocusOnError = true;
            _EmailValidator.CssClass = "ValidatorClass";

            this.Controls.Add(_EmailValidator);
        }
        protected override void hookForValidators(HtmlTextWriter writer) {
            base.hookForValidators(writer);

            _EmailValidator.ErrorMessage = EmailErrorMessage;
            _EmailValidator.Text = EmailErrorMessage;
            _EmailValidator.Enabled = true;
            _EmailValidator.SetFocusOnError = true;
            _EmailValidator.CssClass = "ValidatorClass";
            _EmailValidator.ForeColor = System.Drawing.Color.FromName(ValidatorForeColor);
            _EmailValidator.RenderControl(writer);

        }
    }


    [
    ToolboxData("<{0}:DateThreeColumnedPromptAJAX runat=server></{0}:DateThreeColumnedPromptAJAX"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class DateThreeColumnedPromptAJAX : DateThreeColumnedPrompt {
        //TODO: How do I hook the thing up to AJAX?
        public virtual string ImageID {
            get {
                EnsureChildControls();
                String s = (String)ViewState["ImageID"];
                if (s == null) {
                    ViewState["ImageID"] = "ImageID" + DateTime.Now.Ticks.ToString();
                }
                return (string)ViewState["ImageID"];
            }
            set {
                EnsureChildControls();
                ViewState["ImageID"] = value;
            }
        }

        protected override void DoHookForOtherStuffInTextArea(HtmlTextWriter writer) {

            writer.Write("</td><td style=\"padding-left:2px;\">");
            if (Enabled && !TextboxViewOnly) {
                writer.AddAttribute(HtmlTextWriterAttribute.Type,"image");
                writer.AddAttribute(HtmlTextWriterAttribute.Id,"ImageID633918795890312500");
                writer.AddAttribute(HtmlTextWriterAttribute.Src,
                    "./images/show-calendar.gif");
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "24px");
                writer.AddAttribute(HtmlTextWriterAttribute.Height, "22px");
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Align, "top");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
            }
            writer.Write("</td></tr></table>");
            
            writer.Write("<script type=\"text/javascript\">//<![CDATA[" +
                            "Sys.Application.initialize();" +
                            "Sys.Application.add_init(function() {" +
                            "$create(AjaxControlToolkit.CalendarBehavior, {\"button\":$get(\""+
                            ImageID+"\"),\"id\":\"CalendarExtender1\"}, null, null, $get(\""+
                            TextBoxClientID+"\"));" +
                        "});//]]></script>");
             
//            writer.Write("<cc1:CalendarExtender TargetControlID=\""+this.TextBoxClientID+"\" PopupButtonID=\""+ImageID+"\" ID=\"CalendarExtender1\" runat=\"server\"></cc1:CalendarExtender>");
             
        }
    }

    public class DateThreeColumnedPrompt : ThreeColumnedPromptV2 {
        public static DateTime SQL_NULL_DATETIME = new DateTime(1900, 1, 1);
        public static DateTime NULL_DATETIME = DateTime.MinValue;
        private RegularExpressionValidator _DateValidator;
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Invalid date"),
        Description(
            "Error message for invalid date validator.")
        ]
        public string DateErrorMessage {
            get {
                EnsureChildControls();
                String s = (String)ViewState["DateErrorMessage"];
                return ((s == null) ? "Invalid date" : s);
            }
            set {
                EnsureChildControls();
                ViewState["DateErrorMessage"] = value;
            }
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();
            _DateValidator = new RegularExpressionValidator();
            _DateValidator.ID = "dateValidator1BB3";
            _DateValidator.ControlToValidate = _valueTextBox.ID;
            _DateValidator.Display = ValidatorDisplay.Dynamic;
            //            _DateValidator.ValidationExpression = @"^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$";
            _DateValidator.ValidationExpression = @"^((((0[1-9]|1[012])(0[1-9]|1\d|2[0-8])|(0[13456789]|1[012])(29|30)|(0[13578]|1[02])31)\d{2})|0229([02468][048]|[13579][26]))|((((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00))))$";
            _DateValidator.SetFocusOnError = true;
            _DateValidator.CssClass = "ValidatorClass";


            this.Controls.Add(_DateValidator);
        }
        protected override void hookForValidators(HtmlTextWriter writer) {
            base.hookForValidators(writer);
            _DateValidator.ErrorMessage = DateErrorMessage;
            _DateValidator.Text = DateErrorMessage;
            _DateValidator.Enabled = true;
            _DateValidator.SetFocusOnError = true;
            _DateValidator.CssClass = "ValidatorClass";
            _DateValidator.ForeColor = System.Drawing.Color.FromName(ValidatorForeColor);
            _DateValidator.RenderControl(writer);

        }
        public DateTime getDateValue() {
            try {
                return Convert.ToDateTime(_valueTextBox.Text);
            } catch {
                try {
                    int mm = Convert.ToInt32(_valueTextBox.Text.Substring(0, 2));
                    int dd = Convert.ToInt32(_valueTextBox.Text.Substring(2, 2));
                    int yy = Convert.ToInt32(_valueTextBox.Text.Substring(4, 2));
                    return new DateTime(yy >= 70 ? (1900 + yy) : (2000 + yy), mm, dd);
                } catch {
                    return SQL_NULL_DATETIME;
                }
            }
        }
        public DateTime getDateValueNull() {
            try {
                return Convert.ToDateTime(_valueTextBox.Text);
            } catch {
                try {
                    int mm = Convert.ToInt32(_valueTextBox.Text.Substring(0, 2));
                    int dd = Convert.ToInt32(_valueTextBox.Text.Substring(2, 2));
                    int yy = Convert.ToInt32(_valueTextBox.Text.Substring(4, 2));
                    return new DateTime(yy >= 70 ? (1900 + yy) : (2000 + yy), mm, dd);
                } catch {
                    return NULL_DATETIME;
                }
            }
        }
        protected override void hookForOtherStuffConcerningTextArea(HtmlTextWriter writer) {
            writer.Write("<table cellpadding=\"0\" cellspacing=\"0\"><tr><td>");
            if (WidthTextbox != null && WidthTextbox.Value != 0) {
                _valueTextBox.Width = new Unit(_valueTextBox.Width.Value -
                    (Enabled ? 26 : 0),
                    UnitType.Pixel);
            }
        }
        protected virtual void DoHookForOtherStuffInTextArea(HtmlTextWriter writer) {
            writer.Write("</td><td style=\"padding-left:2px;\">");
            if (Enabled && !TextboxViewOnly) {
                writer.AddAttribute(HtmlTextWriterAttribute.Href,
                    "javascript:show_calendar('forms[0]." + getTextboxName() + "');");
                writer.AddAttribute("onmouseover",
                    "window.status='Date Picker';return true;");
                writer.AddAttribute("onmouseout",
                    "window.status='';return true;");
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.AddAttribute(HtmlTextWriterAttribute.Src,
                    "./images/show-calendar.gif");
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "24px");
                writer.AddAttribute(HtmlTextWriterAttribute.Height, "22px");
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Align, "top");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            writer.Write("</td></tr></table>");
        }

        protected override void hookForOtherStuffInTextArea(HtmlTextWriter writer) {
            base.hookForOtherStuffInTextArea(writer);
            DoHookForOtherStuffInTextArea(writer);
        }
    }
    [
    ToolboxData("<{0}:ZIPThreeColumnedPrompt runat=server></{0}:ZIPThreeColumnedPrompt"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class ZIPThreeColumnedPrompt : ThreeColumnedPromptV2 {
        private RegularExpressionValidator _ZIPValidator;
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Invalid ZIP"),
        Description(
            "Error message for invalid ZIP validator.")
        ]
        public string ZIPErrorMessage {
            get {
                EnsureChildControls();
                String s = (String)ViewState["ZIPErrorMessage"];
                return ((s == null) ? "Invalid ZIP" : s);
            }
            set {
                EnsureChildControls();
                ViewState["ZIPErrorMessage"] = value;
            }
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();
            _ZIPValidator = new RegularExpressionValidator();
            _ZIPValidator.ID = "ZIPValidator1BB4";
            _ZIPValidator.ControlToValidate = _valueTextBox.ID;
            _ZIPValidator.Display = ValidatorDisplay.Dynamic;
            _ZIPValidator.CssClass = "ValidatorClass";
            _ZIPValidator.SetFocusOnError = true;
            //_ZIPValidator.ValidationExpression = @"(((mailto\:|(news|(ht|f)tp(s?))\://)|www){1}\S+)";
            //_ZIPValidator.ValidationExpression = @"^(((ht|f)tp(s?))\://)?(www.|[a-zA-Z].)[a-zA-Z0-9\-\.]+\.(com|edu|gov|mil|net|org|biz|info|name|museum|us|ca|uk)(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\;\?\'\\\+&amp;%\$#\=~_\-]+))*$";

            _ZIPValidator.ValidationExpression = @"^\d{5}$|^\d{5}-\d{4}$";


            this.Controls.Add(_ZIPValidator);
        }
        protected override void hookForValidators(HtmlTextWriter writer) {
            base.hookForValidators(writer);

            _ZIPValidator.ErrorMessage = ZIPErrorMessage;
            _ZIPValidator.Text = ZIPErrorMessage;
            _ZIPValidator.Enabled = true;
            _ZIPValidator.CssClass = "ValidatorClass";
            _ZIPValidator.SetFocusOnError = true;
            _ZIPValidator.ForeColor = System.Drawing.Color.FromName(ValidatorForeColor);
            _ZIPValidator.RenderControl(writer);
        }
    }

    [
    ToolboxData("<{0}:SSNThreeColumnedPrompt runat=server></{0}:SSNThreeColumnedPrompt"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class SSNThreeColumnedPrompt : ThreeColumnedPromptV2 {
        private RegularExpressionValidator _SSNValidator;
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Invalid ssn"),
        Description(
            "Error message for invalid SSN validator.")
        ]
        public string SSNErrorMessage {
            get {
                EnsureChildControls();
                String s = (String)ViewState["SSNErrorMessage"];
                return ((s == null) ? "Invalid SSN address" : s);
            }
            set {
                EnsureChildControls();
                ViewState["SSNErrorMessage"] = value;
            }
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();
            _SSNValidator = new RegularExpressionValidator();
            _SSNValidator.ID = "SSNValidator1BB5";
            _SSNValidator.ControlToValidate = _valueTextBox.ID;
            _SSNValidator.Display = ValidatorDisplay.Dynamic;
            _SSNValidator.CssClass = "ValidatorClass";
            _SSNValidator.SetFocusOnError = true;
            //_SSNValidator.ValidationExpression = @"(((mailto\:|(news|(ht|f)tp(s?))\://)|www){1}\S+)";
            //_SSNValidator.ValidationExpression = @"^(((ht|f)tp(s?))\://)?(www.|[a-zA-Z].)[a-zA-Z0-9\-\.]+\.(com|edu|gov|mil|net|org|biz|info|name|museum|us|ca|uk)(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\;\?\'\\\+&amp;%\$#\=~_\-]+))*$";

       //     _SSNValidator.ValidationExpression = @"\d{9}";
            _SSNValidator.ValidationExpression = @"(\d{3}-\d{2}-\d{4})|(\d{9})";


            this.Controls.Add(_SSNValidator);
        }
        protected override void hookForValidators(HtmlTextWriter writer) {
            base.hookForValidators(writer);

            _SSNValidator.ErrorMessage = SSNErrorMessage;
            _SSNValidator.Text = SSNErrorMessage;
            _SSNValidator.Enabled = true;
            _SSNValidator.CssClass = "ValidatorClass";
            _SSNValidator.SetFocusOnError = true;
            _SSNValidator.ForeColor = System.Drawing.Color.FromName(ValidatorForeColor);
            _SSNValidator.RenderControl(writer);

        }
    }

    [
    ToolboxData("<{0}:URLThreeColumnedPrompt runat=server></{0}:URLThreeColumnedPrompt"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class URLThreeColumnedPrompt : ThreeColumnedPromptV2 {
        private RegularExpressionValidator _URLValidator;
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Invalid url address"),
        Description(
            "Error message for invalid url validator.")
        ]
        public string URLErrorMessage {
            get {
                EnsureChildControls();
                String s = (String)ViewState["URLErrorMessage"];
                return ((s == null) ? "Invalid URL address" : s);
            }
            set {
                EnsureChildControls();
                ViewState["URLErrorMessage"] = value;
            }
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();
            _URLValidator = new RegularExpressionValidator();
            _URLValidator.ID = "urlValidator1BB5";
            _URLValidator.ControlToValidate = _valueTextBox.ID;
            _URLValidator.Display = ValidatorDisplay.Dynamic;
            _URLValidator.CssClass = "ValidatorClass";
            _URLValidator.SetFocusOnError = true;
            //_URLValidator.ValidationExpression = @"(((mailto\:|(news|(ht|f)tp(s?))\://)|www){1}\S+)";
            //_URLValidator.ValidationExpression = @"^(((ht|f)tp(s?))\://)?(www.|[a-zA-Z].)[a-zA-Z0-9\-\.]+\.(com|edu|gov|mil|net|org|biz|info|name|museum|us|ca|uk)(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\;\?\'\\\+&amp;%\$#\=~_\-]+))*$";

            _URLValidator.ValidationExpression = @"(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?";


            this.Controls.Add(_URLValidator);
        }
        protected override void hookForValidators(HtmlTextWriter writer) {
            base.hookForValidators(writer);

            _URLValidator.ErrorMessage = URLErrorMessage;
            _URLValidator.Text = URLErrorMessage;
            _URLValidator.Enabled = true;
            _URLValidator.CssClass = "ValidatorClass";
            _URLValidator.SetFocusOnError = true;
            _URLValidator.ForeColor = System.Drawing.Color.FromName(ValidatorForeColor);
            _URLValidator.RenderControl(writer);

        }
    }
    [
    ToolboxData("<{0}:TimeThreeColumnedPrompt runat=server></{0}:TimeThreeColumnedPrompt"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class TimeThreeColumnedPrompt : ThreeColumnedPromptV2 {
        private RegularExpressionValidator _TimeValidator;
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Invalid time"),
        Description(
            "Error message for invalid time validator.")
        ]
        public string TimeErrorMessage {
            get {
                EnsureChildControls();
                String s = (String)ViewState["TimeErrorMessage"];
                return ((s == null) ? "Invalid Time" : s);
            }
            set {
                EnsureChildControls();
                ViewState["TimeErrorMessage"] = value;
            }
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();
            _TimeValidator = new RegularExpressionValidator();
            _TimeValidator.ID = "timeValidator1BB6";
            _TimeValidator.ControlToValidate = _valueTextBox.ID;
            _TimeValidator.Display = ValidatorDisplay.Dynamic;
            _TimeValidator.CssClass = "ValidatorClass";
            _TimeValidator.SetFocusOnError = true;
            //_URLValidator.ValidationExpression = @"(((mailto\:|(news|(ht|f)tp(s?))\://)|www){1}\S+)";
            //_URLValidator.ValidationExpression = @"^(((ht|f)tp(s?))\://)?(www.|[a-zA-Z].)[a-zA-Z0-9\-\.]+\.(com|edu|gov|mil|net|org|biz|info|name|museum|us|ca|uk)(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\;\?\'\\\+&amp;%\$#\=~_\-]+))*$";

            _TimeValidator.ValidationExpression = @"^((([0]?[1-9]|1[0-2])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))|(([0]?[0-9]|1[0-9]|2[0-3])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?))$";



            this.Controls.Add(_TimeValidator);
        }
        protected override void hookForValidators(HtmlTextWriter writer) {
            base.hookForValidators(writer);

            _TimeValidator.ErrorMessage = TimeErrorMessage;
            _TimeValidator.Text = TimeErrorMessage;
            _TimeValidator.Enabled = true;
            _TimeValidator.CssClass = "ValidatorClass";
            _TimeValidator.SetFocusOnError = true;
            _TimeValidator.ForeColor = System.Drawing.Color.FromName(ValidatorForeColor);
            _TimeValidator.RenderControl(writer);

        }
    }

    [
    ToolboxData("<{0}:PhoneThreeColumnedPrompt runat=server></{0}:PhoneThreeColumnedPrompt"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class PhoneThreeColumnedPrompt : ThreeColumnedPromptV2 {
        private RegularExpressionValidator _PhoneValidator;
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Format: (303) 377-4315"),
        Description(
            "Error message for invalid phone validator.")
        ]
        public override string Value {
            get {
                EnsureChildControls();
                return getValue();
            }
            set {
                EnsureChildControls();
                string zValue = value;
                if(zValue.Length==10) {
                    try {
                        double nbr = Convert.ToDouble(zValue);
                        zValue = "(" + zValue.Substring(0, 3) + ") " + zValue.Substring(3, 3) + "-" + zValue.Substring(6, 4);

                    } catch { }
                }
                _valueTextBox.Text = zValue;
            }
        }
        public override string getValue() {
            string hisValue = base.getValue();
            try {
                if (hisValue != null && hisValue.Trim().Length == 10) {
                    double nbr = Convert.ToDouble(hisValue);
                    hisValue = "(" + hisValue.Substring(0, 3) + ") " + hisValue.Substring(3, 3) + "-" + hisValue.Substring(6, 4);
                }
            } catch {
            }
            return hisValue;
        }
        public string PhoneErrorMessage {
            get {
                EnsureChildControls();
                String s = (String)ViewState["PhoneErrorMessage"];
                return ((s == null) ? "Invalid" : s);
            }
            set {
                EnsureChildControls();
                ViewState["PhoneErrorMessage"] = value;
            }
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();
            _PhoneValidator = new RegularExpressionValidator();
            _PhoneValidator.ID = "phoneValidator1BB7";
            _PhoneValidator.ControlToValidate = _valueTextBox.ID;
            _PhoneValidator.Display = ValidatorDisplay.Dynamic;
            _PhoneValidator.CssClass = "ValidatorClass";
            _PhoneValidator.SetFocusOnError = true;
            //_URLValidator.ValidationExpression = @"(((mailto\:|(news|(ht|f)tp(s?))\://)|www){1}\S+)";
            //_URLValidator.ValidationExpression = @"^(((ht|f)tp(s?))\://)?(www.|[a-zA-Z].)[a-zA-Z0-9\-\.]+\.(com|edu|gov|mil|net|org|biz|info|name|museum|us|ca|uk)(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\;\?\'\\\+&amp;%\$#\=~_\-]+))*$";

//            _PhoneValidator.ValidationExpression = @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}";
            _PhoneValidator.ValidationExpression = @"(\(\d{3}\) \d{3}-\d{4})|(\d{10})";


            this.Controls.Add(_PhoneValidator);
        }
        protected override void hookForValidators(HtmlTextWriter writer) {
            base.hookForValidators(writer);

            _PhoneValidator.ErrorMessage = PhoneErrorMessage;
            _PhoneValidator.Text = PhoneErrorMessage;
            _PhoneValidator.Enabled = true;
            _PhoneValidator.CssClass = "ValidatorClass";
            _PhoneValidator.SetFocusOnError = true;
            _PhoneValidator.ForeColor = System.Drawing.Color.FromName(ValidatorForeColor);
            _PhoneValidator.RenderControl(writer);

        }
    }

    [
    ToolboxData("<{0}:NumericThreeColumnedPrompt runat=server></{0}:NumericThreeColumnedPrompt"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class NumericThreeColumnedPrompt : ThreeColumnedPromptV2 {
        private RegularExpressionValidator _NumericValidator;
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Must be numeric"),
        Description(
            "Error message for invalid numeric validator.")
        ]
        public string NumericErrorMessage {
            get {
                EnsureChildControls();
                String s = (String)ViewState["NumericErrorMessage"];
                return ((s == null) ? "Must be numeric" : s);
            }
            set {
                EnsureChildControls();
                ViewState["NumericErrorMessage"] = value;
            }
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();
            _NumericValidator = new RegularExpressionValidator();
            _NumericValidator.ID = "NumericValidator1BB8";
            _NumericValidator.ControlToValidate = _valueTextBox.ID;
            _NumericValidator.Display = ValidatorDisplay.Dynamic;
            _NumericValidator.CssClass = "ValidatorClass";
            _NumericValidator.SetFocusOnError = true;
//            _NumericValidator.ValidationExpression = @"^[0-9^.]{1}$|^[0-9]{1}[0-9]{1}$|^[0-9]{1}[0-9]{1}[0-9]{1}$|^[0-9]{1}[0-9]{1}[0-9]{1}[0-9]{1}$|^[0-9]{1}[0-9]{1}[0-9]{1}[0-9]{1}[0-9]{1}$";
            _NumericValidator.ValidationExpression = @"^\-?\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(\.[0-9][0-9])?$";
//            _NumericValidator.ValidationExpression = @"^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$";
            this.Controls.Add(_NumericValidator);
        }
        protected override void hookForValidators(HtmlTextWriter writer) {
            base.hookForValidators(writer);

            _NumericValidator.ErrorMessage = NumericErrorMessage;
            _NumericValidator.Text = NumericErrorMessage;
            _NumericValidator.Enabled = true;
            _NumericValidator.CssClass = "ValidatorClass";
            _NumericValidator.SetFocusOnError = true;
            _NumericValidator.ForeColor = System.Drawing.Color.FromName(ValidatorForeColor);
            _NumericValidator.RenderControl(writer);

        }
    }



    [
    ToolboxData("<{0}:CompareTwoThreeColumnedPromptsV2 runat=server></{0}:CompareTwoThreeColumnedPromptsV2"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]

    public class CompareTwoThreeColumnedPromptsV2 : CompositeControl {
        private Label _promptLabel1;
        protected TextBox _valueTextBox1;
        private Label _promptLabel2;
        protected TextBox _valueTextBox2;
        private RequiredFieldValidator _valueValidator1;
        private RequiredFieldValidator _valueValidator2;
        private CompareValidator _compareValidator;
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description(
            "Are Passwords?")
        ]
        public bool ArePasswords {
            get {
                EnsureChildControls();
                object b = ViewState["ArePasswords"];
                return ((b == null) ? false : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["ArePasswords"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(90),
        Description("Width Textbox.")
        ]
        public Unit WidthTextbox {
            get {
                EnsureChildControls();
                object w = ViewState["WidthTextbox"];
                return (w == null) ? 90 : (Unit)w;
            }
            set {
                EnsureChildControls();
                ViewState["WidthTextbox"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("*"),
        Description("Required indicator.")
        ]
        public string RequiredIndicatorText {
            get {
                EnsureChildControls();
                object s = ViewState["RequiredIndicatorText"];
                return s == null ? "*" : (string)s;
            }
            set {
                EnsureChildControls();
                ViewState["RequiredIndicatorText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("red"),
        Description("Required indicator color.")
        ]
        public string RequiredIndicatorColor {
            get {
                EnsureChildControls();
                object s = ViewState["RequiredIndicatorColor"];
                string str = s == null ? "red" : ((string)s);
                if (str.Trim().Equals("")) {
                    return "red";
                } else {
                    return str.ToLower();
                }
            }
            set {
                EnsureChildControls();
                ViewState["RequiredIndicatorColor"] = value;
            }
        }
        
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description(
            "Is field required?")
        ]
        public bool IsRequiredField {
            get {
                EnsureChildControls();
                object b = ViewState["IsRequiredField"];
                return ((b == null) ? false : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["IsRequiredField"] = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(111),
        Description("Width left column.")
        ]
        public Unit WidthLeftColumn {
            get {
                EnsureChildControls();
                object w = ViewState["WidthLeftColumn"];
                return (w==null)?111:(Unit)w;
            }
            set {
                EnsureChildControls();
                ViewState["WidthLeftColumn"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(55),
        Description("Width right column.")
        ]
        public Unit WidthRightColumn {
            get {
                EnsureChildControls();
                object w = ViewState["WidthRightColumn"];
                return (w == null) ? 55 : (Unit)w;
            }
            set {
                EnsureChildControls();
                ViewState["WidthRightColumn"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("required"),
        Description(
            "Error message for the value field validator.")
        ]
        public string ValueErrorMessage {
            get {
                EnsureChildControls();
                String s = (String)ViewState["ValueErrorMessage"];
                return ((s == null) ? "required" : s);
            }
            set {
                EnsureChildControls();
                ViewState["ValueErrorMessage"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Fields must be equal"),
        Description(
            "Error message for the compare field validator.")
        ]
        public string CompareErrorMessage {
            get {
                EnsureChildControls();
                String s = (String)ViewState["CompareErrorMessage"];
                return ((s == null) ? "Fields must be equal" : s);
            }
            set {
                EnsureChildControls();
                ViewState["CompareErrorMessage"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("The text for the 1st prompt.")
        ]
        public string PromptLabelText1 {
            get {
                EnsureChildControls();
                object s = ViewState["PromptLabelText1"];
                return s == null ? "" : (string)s;
            }
            set {
                EnsureChildControls();
                ViewState["PromptLabelText1"] = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("The text for the 2nd prompt.")
        ]
        public string PromptLabelText2 {
            get {
                EnsureChildControls();
                object s = ViewState["PromptLabelText2"];
                return s == null ? "" : (string)s;
            }
            set {
                EnsureChildControls();
                ViewState["PromptLabelText2"] = value;
            }
        }

        protected override void RecreateChildControls() {
            EnsureChildControls();
        }
        public string getValue1() {
            return _valueTextBox1.Text;
        }
        public void clearValues() {
            _valueTextBox1.Text = string.Empty;
            _valueTextBox2.Text = string.Empty;
        }
        public void setValues(string value) {
            _valueTextBox1.Text = value;
            _valueTextBox2.Text = value;
        }
        protected override void CreateChildControls() {
            _promptLabel1 = new Label();
            _valueTextBox1 = new TextBox();
            _valueTextBox1.ID = "valueTextBox1";
            _promptLabel2 = new Label();
            _valueTextBox2 = new TextBox();
            _valueTextBox2.ID = "valueTextBox2";
            _compareValidator = new CompareValidator();
            _compareValidator.ControlToValidate = _valueTextBox2.ID;
            _compareValidator.ControlToCompare = _valueTextBox1.ID;
            _compareValidator.Display = ValidatorDisplay.Dynamic;
            _compareValidator.SetFocusOnError = true;

            _valueValidator1 = new RequiredFieldValidator();
            _valueValidator1.ControlToValidate = _valueTextBox1.ID;
            _valueValidator1.Display = ValidatorDisplay.Dynamic;
            _valueValidator1.SetFocusOnError = true;
            _valueValidator2 = new RequiredFieldValidator();
            _valueValidator2.ControlToValidate = _valueTextBox2.ID;
            _valueValidator2.Display = ValidatorDisplay.Dynamic;
            _valueValidator2.SetFocusOnError = true;

            this.Controls.Add(_promptLabel1);
            this.Controls.Add(_valueTextBox1);
            this.Controls.Add(_valueValidator1);
            this.Controls.Add(_promptLabel2);
            this.Controls.Add(_valueTextBox2);
            this.Controls.Add(_valueValidator2);
            Controls.Add(_compareValidator);
            base.CreateChildControls();
        }
        public void setTextBox1(string str) {
            _valueTextBox1.Text = str;
        }
        public void setTextBox2(string str) {
            _valueTextBox2.Text = str;
        }
        protected override void Render(HtmlTextWriter writer) {
            AddAttributesToRender(writer);
            writer.AddAttribute(
                HtmlTextWriterAttribute.Cellpadding,
                "1", false);

            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            if (WidthLeftColumn != 0 && WidthLeftColumn != null) {
                writer.AddAttribute(
                    HtmlTextWriterAttribute.Width,
                    WidthLeftColumn.ToString(), false);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _promptLabel1.Text = PromptLabelText1;
            _promptLabel1.RenderControl(writer);
            if (!RequiredIndicatorText.Trim().Equals("")) {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "position:relative;top:-5px;color:" + this.RequiredIndicatorColor + ";font-size:8pt;font-weight:bold;");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(this.RequiredIndicatorText);
                writer.RenderEndTag();
            }
            writer.Write(":");
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (WidthTextbox != null && WidthTextbox.Value > 0) {
                _valueTextBox1.Width = WidthTextbox;
            }
            if (ArePasswords) {
                _valueTextBox1.TextMode = TextBoxMode.Password;
            } else {
                _valueTextBox1.TextMode = TextBoxMode.SingleLine;
            }
            _valueTextBox1.RenderControl(writer);
            writer.RenderEndTag();
            if (WidthRightColumn != 0 && WidthRightColumn != null) {
                writer.AddAttribute(
                    HtmlTextWriterAttribute.Width,
                    WidthRightColumn.ToString(), false);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _valueValidator1.ErrorMessage = ValueErrorMessage;
            _valueValidator1.Text = ValueErrorMessage;
            _valueValidator1.Enabled = IsRequiredField;
            _valueValidator1.RenderControl(writer);
            _compareValidator.ErrorMessage = CompareErrorMessage;
            _compareValidator.Text = CompareErrorMessage;
            _compareValidator.Enabled = IsRequiredField;
            _compareValidator.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            if (WidthLeftColumn != 0 && WidthLeftColumn != null) {
                writer.AddAttribute(
                    HtmlTextWriterAttribute.Width,
                    WidthLeftColumn.ToString(), false);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _promptLabel2.Text = PromptLabelText2;
            _promptLabel2.RenderControl(writer);
            if (!RequiredIndicatorText.Trim().Equals("")) {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "position:relative;top:-5px;color:" + this.RequiredIndicatorColor + ";font-size:8pt;font-weight:bold;");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(this.RequiredIndicatorText);
                writer.RenderEndTag();
            }
            writer.Write(":");
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (WidthTextbox != null && WidthTextbox.Value > 0) {
                _valueTextBox2.Width = WidthTextbox;
            }
            if (ArePasswords) {
                _valueTextBox2.TextMode = TextBoxMode.Password;
            } else {
                _valueTextBox2.TextMode = TextBoxMode.SingleLine;
            }
            _valueTextBox2.RenderControl(writer);
            writer.RenderEndTag();
            if (WidthRightColumn != 0 && WidthRightColumn != null) {
                writer.AddAttribute(
                    HtmlTextWriterAttribute.Width,
                    WidthRightColumn.ToString(), false);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _valueValidator2.ErrorMessage = ValueErrorMessage;
            _valueValidator2.Text = ValueErrorMessage;
            _valueValidator2.Enabled = IsRequiredField;
            _valueValidator2.RenderControl(writer);
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();
        }
    }

    [
    ToolboxData("<{0}:ThreeColumnedPromptDateControlV4 runat=server></{0}:ThreeColumnedPromptDateControlV4"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
        //    DefaultEvent("Submit"),
        //    DefaultProperty("ButtonText"),
    ]
    public class ThreeColumnedPromptDateControlV4 : ThreeColumnedPromptV2 {
        private RegularExpressionValidator _DateValidator;
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Invalid date"),
        Description(
            "Error message for invalid date validator.")
        ]
        public string DateErrorMessage {
            get {
                EnsureChildControls();
                String s = (String)ViewState["DateErrorMessage"];
                return ((s == null) ? "Invalid date" : s);
            }
            set {
                EnsureChildControls();
                ViewState["DateErrorMessage"] = value;
            }
        }
        protected override void hookForOtherStuffConcerningTextArea(HtmlTextWriter writer) {
            _valueTextBox.ReadOnly = true;
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();
            _DateValidator = new RegularExpressionValidator();
            _DateValidator.ID = "dateValidator1bb9";
            _DateValidator.ControlToValidate = _valueTextBox.ID;
            _DateValidator.Display = ValidatorDisplay.Dynamic;
//            _DateValidator.ValidationExpression = @"^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$";
            _DateValidator.ValidationExpression = @"^((((0[1-9]|1[012])(0[1-9]|1\d|2[0-8])|(0[13456789]|1[012])(29|30)|(0[13578]|1[02])31)\d{2})|0229([02468][048]|[13579][26]))|((((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00))))$";
            _DateValidator.SetFocusOnError = true;
            _DateValidator.CssClass = "ValidatorClass";

            this.Controls.Add(_DateValidator);
        }

        protected override void hookForValidators(HtmlTextWriter writer) {
            base.hookForValidators(writer);
            _DateValidator.ErrorMessage = DateErrorMessage;
            _DateValidator.Text = DateErrorMessage;
            _DateValidator.Enabled = true;
            _DateValidator.SetFocusOnError = true;
            _DateValidator.CssClass = "ValidatorClass";
            _DateValidator.ForeColor = System.Drawing.Color.FromName(ValidatorForeColor);
            _DateValidator.RenderControl(writer);

        }

        protected override void hookForOtherStuffInTextArea(HtmlTextWriter writer) {
            base.hookForOtherStuffInTextArea(writer);
            if (WidthTextbox != null && WidthTextbox.Value != 0) {
                _valueTextBox.Width = new Unit(_valueTextBox.Width.Value - 32, UnitType.Pixel);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Href,
                "javascript:show_calendar('forms[0]." + getTextboxName() + "');");
            writer.AddAttribute("onmouseover",
                "window.status='Date Picker';return true;");
            writer.AddAttribute("onmouseout",
                "window.status='';return true;");
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.AddAttribute(HtmlTextWriterAttribute.Src,
                "./images/show-calendar.gif");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "24px");
            writer.AddAttribute(HtmlTextWriterAttribute.Height, "22px");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "top");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
    }
    
    [
    ToolboxData("<{0}:ThreeColumnedPromptV2 runat=server></{0}:ThreeColumnedPromptV2"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
//    DefaultEvent("Submit"),
//    DefaultProperty("ButtonText"),
    ]
    public class ThreeColumnedPromptV2 : CompositeControl {
        private static int jd3 = 0;
        private Label _promptLabel;
        protected TextBox _valueTextBox;
        private RequiredFieldValidator _valueValidator;
        private static readonly object EventTextChanged =
            new object();
        public RequiredFieldValidator MineValidator {
            get {
                return _valueValidator;
            }
        }
        protected string getTextboxName() {
            return _valueTextBox.ClientID.Replace("_", "$");
        }
        [
        Category("Action"),
        Description("Raised when the user keys data into the TextBox.")
        ]
        public event EventHandler TextChanged {
            add {
                Events.AddHandler(EventTextChanged, value);
            }
            remove {
                Events.RemoveHandler(EventTextChanged, value);
            }
        }

        // The method that raises the TextChanged event.
        protected virtual void OnTextChanged(EventArgs e) {
            EventHandler SubmitHandler =
                (EventHandler)Events[EventTextChanged];
            if (SubmitHandler != null) {
                SubmitHandler(this, e);
            }
        }
        // Handles the TextChanged event of the Button and raises
        // the Submit event.
        void _valueTextBox_TextChanged(object sender, EventArgs e) {
            OnTextChanged(e);
        }
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(false)]
        [Localizable(false)]
        public bool InheritTable {
            get {
                EnsureChildControls();
                if (ViewState["InheritTable"] == null) {
                    ViewState["InheritTable"] = false;
                }
                return (bool)ViewState["InheritTable"];
            }
            set {
                EnsureChildControls();
                ViewState["InheritTable"] = value;
            }
        }
        [
        Bindable(true),
        Category("Default"),
        DefaultValue(""),
        Description("The value field.")
        ]
        public virtual string Value
        {
            get
            {
                EnsureChildControls();
                if (ForceUpper) {
                    return _valueTextBox.Text.ToUpper();
                } else {
                    return _valueTextBox.Text;
                }
            }
            set
            {
                EnsureChildControls();
                _valueTextBox.Text = value;
            }
        }
        public virtual string getValue() {
            if (ForceUpper) {
                return _valueTextBox.Text.ToUpper();
            } else {
                return _valueTextBox.Text;
            }
        }


        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("0"),
        Description(
            "TabIndex2")
        ]
        public short TabIndex2 {
            get {
                EnsureChildControls();
                Object o = ViewState["TabIndex2"];
                return ((o == null) ? (short)0 : (short)o);
            }
            set {
                EnsureChildControls();
                ViewState["TabIndex2"] = value;
            }
        }


        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("formtextbox"),
        Description(
            "CSSClass Textbox")
        ]
        public string CSSClass_Textbox {
            get {
                EnsureChildControls();
                String s = (String)ViewState["CSS_Textbox"];
                return ((s == null) ? "formtextbox" : s);
            }
            set {
                EnsureChildControls();
                ViewState["CSS_Textbox"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("false"),
        Description(
            "CSSClass View Only")
        ]
        public bool TextboxViewOnly {
            get {
                EnsureChildControls();
                object obj = ViewState["TextboxViewOnly"];
                return ((obj == null) ? false : (bool)obj);
            }
            set {
                EnsureChildControls();
                ViewState["TextboxViewOnly"] = value;
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue("false"),
        Description(
            "CSSClass View Only")
        ]
        public bool ForceUpper {
            get {
                EnsureChildControls();
                object obj = ViewState["ForceUppercase"];
                return ((obj == null) ? false : (bool)obj);
            }
            set {
                EnsureChildControls();
                ViewState["ForceUppercase"] = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("formlabel"),
        Description(
            "CSSClass Prompt")
        ]
        public string CSSClass_Prompt {
            get {
                EnsureChildControls();
                String s = (String)ViewState["CSSClass_Prompt"];
                return ((s == null) ? "formlabel" : s);
            }
            set {
                EnsureChildControls();
                ViewState["CSSClass_Prompt"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("formalert"),
        Description(
            "CSSClass Alert")
        ]
        public string CSSClass_Alert {
            get {
                EnsureChildControls();
                String s = (String)ViewState["CSSClass_Alert"];
                return ((s == null) ? "formalert" : s);
            }
            set {
                EnsureChildControls();
                ViewState["CSSClass_Alert"] = value;
            }
        }
        public enum MyLayout {
            Horizontal=0,
            Vertical_TextAbove,
            Vertical_TextBelow
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(0),
        Description(
            "Layout")
        ]
        public MyLayout Layout {
            get {
                EnsureChildControls();
                object s = ViewState["Layout"];
                return ((s == null) ? MyLayout.Horizontal : (MyLayout)s);
            }
            set {
                EnsureChildControls();
                ViewState["Layout"] = value;
            }
        }
        
        
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("Field is required"),
        Description(
            "Error message for the value field validator.")
        ]
        public string ValueErrorMessage
        {
            get {
                EnsureChildControls();
                String s = (String)ViewState["ValueErrorMessage"];
                return ((s == null) ? "Field is required" : s);
            }
            set {
                EnsureChildControls();
                ViewState["ValueErrorMessage"] = value;
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description(
            "Is field required?")
        ]
        public bool IsRequiredField {
            get {
                EnsureChildControls();
                object b = ViewState["IsRequiredField"];
                try {
                    if (((bool)b) == false) {
                        _valueValidator.IsValid = true;
                    }
                } catch { }
                return ((b == null) ? false : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["IsRequiredField"] = value;
                if (value == false) {
                    _valueValidator.IsValid = true;
                    _valueValidator.Enabled = false;
                    _valueValidator.EnableClientScript = false;
                } else {
                    _valueValidator.Enabled = true;
                }
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description(
            "Is multiple rows?")
        ]
        public bool IsMulticolumned {
            get {
                EnsureChildControls();
                object b = ViewState["IsMulticolumned"];
                return ((b == null) ? false : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["IsMulticolumned"] = value;
            }
        }
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(false),
        Description(
            "Is a Password?")
        ]
        public bool IsPassword {
            get {
                EnsureChildControls();
                object b = ViewState["IsPassword"];
                return ((b == null) ? false : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["IsPassword"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(133),
        Description("Height Textbox.")
        ]
        public Unit HeightTextbox {
            get {
                EnsureChildControls();
                object w = ViewState["HeightTextbox"];
                return (w == null) ? 133 : (Unit)w;
            }
            set {
                EnsureChildControls();
                ViewState["HeightTextbox"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(133),
        Description("Width Textbox.")
        ]
        public Unit WidthTextbox {
            get {
                EnsureChildControls();
                object w = ViewState["WidthTextbox"];
                return (w == null) ? 133 : (Unit)w;
            }
            set {
                EnsureChildControls();
                ViewState["WidthTextbox"] = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(133),
        Description("Width left column.")
        ]
        public Unit WidthLeftColumn {
            get {
                EnsureChildControls();
                object w = ViewState["WidthLeftColumn"];
                return (w==null)?133:(Unit)w;
            }
            set {
                EnsureChildControls();
                ViewState["WidthLeftColumn"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(133),
        Description("Width right column.")
        ]
        public Unit WidthRightColumn {
            get {
                EnsureChildControls();
                object w = ViewState["WidthRightColumn"];
                return (w == null) ? 133 : (Unit)w;
            }
            set {
                EnsureChildControls();
                ViewState["WidthRightColumn"] = value;
            }
        }
        public Control TextBoxControl {
            get {
                EnsureChildControls();
                return _valueTextBox;
            }
        }
        public String TextBoxID {
            get {
                EnsureChildControls();
                return _valueTextBox.ID;
            }
        }
        public String TextBoxClientID {
            get {
                EnsureChildControls();
                return _valueTextBox.ClientID;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("*"),
        Description("Required indicator.")
        ]
        public string RequiredIndicatorText {
            get {
                EnsureChildControls();
                object s = ViewState["RequiredIndicatorText"];
                return s == null ? "*" : (string)s;
            }
            set {
                EnsureChildControls();
                ViewState["RequiredIndicatorText"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("red"),
        Description("Required indicator color.")
        ]
        public string RequiredIndicatorColor {
            get {
                EnsureChildControls();
                object s = ViewState["RequiredIndicatorColor"];
                string str= s == null ? "red" : ((string)s);
                if(str.Trim().Equals("")) {
                    return "red";
                } else {
                    return str.ToLower();
                }
            }
            set {
                EnsureChildControls();
                ViewState["RequiredIndicatorColor"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("The text for the prompt.")
        ]
        public string PromptLabelText
        {
            get
            {
                EnsureChildControls();
                object s = ViewState["PromptLabelText"];
                return s == null ? "" : (string)s;
            }
            set
            {
                EnsureChildControls();
                ViewState["PromptLabelText"] = value;
            }
        }

        [Bindable(true),
        Category("Behavior"),
        DefaultValue("#FF0000"),
        Localizable(false),
        Description("ErrorMessageColor")
        ]
        public string ValidatorForeColor {
            get {
                EnsureChildControls();
                object obj = ViewState["VFC"];
                return obj == null ? "#FF0000" : (string)obj;
            }
            set {
                EnsureChildControls();
                ViewState["VFC"] = value;
            }
        }

        [Bindable(true),
        Category("Behavior"),
        DefaultValue("valueTextBoxBB"),
        Localizable(false),
        Description("ID")
        ]
        public string JDID {
            get {
                EnsureChildControls();
                object obj = ViewState["JDID"];
                return obj == null ? "valueTextBoxBB" : (string)obj;
            }
            set {
                EnsureChildControls();
                ViewState["JDID"] = value;
            }
        }
        [Bindable(true),
        Category("Behavior"),
        DefaultValue("1"),
        Localizable(false),
        Description("JDVersion")
        ]
        public string JDVersion {
            get {
                EnsureChildControls();
                object obj = ViewState["JDVersion"];
                return obj == null ? "1" : (string)obj;
            }
            set {
                EnsureChildControls();
                ViewState["JDVersion"] = value;
            }
        }

        
        protected override void RecreateChildControls() {
            EnsureChildControls();
        }
        protected override void CreateChildControls() {
            _promptLabel = new Label();
            _valueTextBox = new TextBox();
            _valueTextBox.ID = JDID;
            /*
             * Why do I do the following?  I don't know.  If I don't do it, then whatever I key into a
             * text box gets overwritten by the original values before the "click" event occurs.  I found
             * out to try this by going to www.deja.com.  And it worked.
            */
            _valueTextBox.EnableViewState = false;
            _valueTextBox.TextChanged += new EventHandler(_valueTextBox_TextChanged);

            _valueValidator = new RequiredFieldValidator();
            _valueValidator.ControlToValidate = _valueTextBox.ID;
            _valueValidator.Display = ValidatorDisplay.Dynamic;
            _valueValidator.SetFocusOnError = true;
            _valueValidator.CssClass = CSSClass_Alert;
            _valueValidator.Enabled = IsRequiredField;


            this.Controls.Add(_promptLabel);
            this.Controls.Add(_valueTextBox);
            this.Controls.Add(_valueValidator);
            base.CreateChildControls();
        }

        public void kludgeToForceValidness(bool isValid) {
            _valueValidator.IsValid = isValid;
            if (isValid) {
                _valueValidator.Enabled = false;
            }
        }
        protected virtual void hookForValidators(HtmlTextWriter writer) {
            _valueValidator.ErrorMessage = ValueErrorMessage;
            _valueValidator.Text = ValueErrorMessage;
            _valueValidator.Enabled = IsRequiredField;
            _valueValidator.Display = ValidatorDisplay.Dynamic;
            _valueValidator.SetFocusOnError = true;
            _valueValidator.CssClass = "ValidatorClass";
            _valueValidator.ForeColor = System.Drawing.Color.FromName(ValidatorForeColor);
            if (!IsRequiredField) {
                _valueValidator.IsValid = true;
            }
            _valueValidator.RenderControl(writer);
        }
        protected virtual void hookForOtherStuffInTextArea(HtmlTextWriter writer) {
        }
        protected virtual void hookForOtherStuffConcerningTextArea(HtmlTextWriter writer) {
        }
        protected override void Render(HtmlTextWriter writer) {
            AddAttributesToRender(writer);
            if (JDVersion.Equals("1")) {
                if (!InheritTable) {
                    writer.AddAttribute(
                        HtmlTextWriterAttribute.Cellpadding,
                        "1", false);
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);
                }
            } else {
                if (!InheritTable) {
                    writer.AddAttribute(
                        HtmlTextWriterAttribute.Cellpadding,
                        "0", false);
                    writer.AddAttribute(
                        HtmlTextWriterAttribute.Cellspacing,
                        "0", false);
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);
                }
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            _valueTextBox.Text = Value;
            _promptLabel.Text = PromptLabelText;
            if (IsMulticolumned == true) {
                if (HeightTextbox != null && HeightTextbox.Value != 0) {
                    _valueTextBox.Height = HeightTextbox;
                }
                _valueTextBox.TextMode = TextBoxMode.MultiLine;
                
            } else {
                if (IsPassword == true) {
                    _valueTextBox.TextMode = TextBoxMode.Password;
                } else {
                    _valueTextBox.TextMode = TextBoxMode.SingleLine;
                }
            }
            _valueTextBox.CssClass = CSSClass_Textbox;
            if (TabIndex2 != 0) {
                _valueTextBox.TabIndex = TabIndex2;
            }
            if (Layout.Equals(MyLayout.Horizontal)) {
                if (!InheritTable) {
                    if (WidthLeftColumn != null && WidthLeftColumn.Value != 0) {
                        writer.AddAttribute(
                            HtmlTextWriterAttribute.Width,
                            WidthLeftColumn.ToString(), false);
                    }
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, "right");
                }
                if (IsMulticolumned == true && !InheritTable) {
                    writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CSSClass_Prompt);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                _promptLabel.RenderControl(writer);
                if (IsRequiredField && !_promptLabel.Text.Trim().Trim().Equals("") && !RequiredIndicatorText.Trim().Equals("")) {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "position:relative;top:-5px;color:" + this.RequiredIndicatorColor + ";font-size:8pt;font-weight:bold;");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write(this.RequiredIndicatorText);
                    writer.RenderEndTag();
                }
                if (_promptLabel != null && _promptLabel.Text.Trim().Length > 0) {
                    writer.Write(":");
                }
                writer.RenderEndTag(); // end Td
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CSSClass_Textbox);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                if (WidthTextbox != null && WidthTextbox.Value != 0) {
                    _valueTextBox.Width = WidthTextbox;
                }
                hookForOtherStuffConcerningTextArea(writer);
                if (TextboxViewOnly) {
                    _valueTextBox.CssClass = "formtextboxviewonly";
                    _valueTextBox.ReadOnly = true;
                }
                _valueTextBox.RenderControl(writer);
                hookForOtherStuffInTextArea(writer);
                writer.RenderEndTag(); // end Td
                if (!InheritTable) {
                    if (WidthRightColumn != 0 && WidthRightColumn != null) {
                        writer.AddAttribute(
                            HtmlTextWriterAttribute.Width,
                            WidthRightColumn.ToString(), false);
                    }
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CSSClass_Alert);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                hookForValidators(writer);
                writer.RenderEndTag();  // end Td
            } else {
                if (Layout.Equals(MyLayout.Vertical_TextAbove)) {
                } else { // is MyLayout.Vertical_TextBelow
                    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "3");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, CSSClass_Textbox);
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    if (WidthTextbox != null && WidthTextbox.Value != 0) {
                        _valueTextBox.Width = WidthTextbox;
                    }
                    hookForOtherStuffConcerningTextArea(writer);
                    _valueTextBox.RenderControl(writer);
                    hookForOtherStuffInTextArea(writer);
                    writer.Write("<br />");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, CSSClass_Prompt);
                    if (!InheritTable) {
                        if (WidthLeftColumn != null && WidthLeftColumn.Value != 0) {
                            writer.AddAttribute(
                                HtmlTextWriterAttribute.Width,
                                WidthLeftColumn.ToString(), false);
                        }
                        writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
                        if (IsMulticolumned == true) {
                            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                        }
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    _promptLabel.RenderControl(writer);
                    if (IsRequiredField && !RequiredIndicatorText.Trim().Equals("")) {
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, "position:relative;top:-5px;color:" + this.RequiredIndicatorColor + ";font-size:8pt;font-weight:bold;");
                        writer.RenderBeginTag(HtmlTextWriterTag.Span);
                        writer.Write(this.RequiredIndicatorText);
                        writer.RenderEndTag();
                    }
                    writer.RenderEndTag(); // end Span
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, CSSClass_Alert);
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    hookForValidators(writer);
                    writer.RenderEndTag();  // end Span
                }
            }
            writer.RenderEndTag(); // end Tr
            if (!InheritTable) {
                writer.RenderEndTag(); // end Table
            }
        }
    }
    public abstract class ThreeColumnedGenericControl : CompositeControl {
        private Label _prompt;
        private CustomValidator _customValidator;
        protected abstract void child_CreateControl();
        protected abstract void child_Render(HtmlTextWriter writer);
        protected abstract Control child_Control { get;}
        private static readonly object EventSomthingNotableHappened = new object();

#region Design attributes
        [
        Category("Action"),
        Description("Raised when this control causes an action.")
        ]
        public event EventHandler ActionOccurred {
            add {
                Events.AddHandler(EventSomthingNotableHappened, value);
            }
            remove {
                Events.RemoveHandler(EventSomthingNotableHappened, value);
            }
        }
        // The method that raises the ActionOccurred event.
        protected virtual void OnActionOccurred(EventArgs e) {
            EventHandler SubmitHandler =
                (EventHandler)Events[EventSomthingNotableHappened];
            if (SubmitHandler != null) {
                SubmitHandler(this, e);
            }
        }
        // Handles the Somthing Happened event of the encapsulated control and raises
        // the OnActionOccurred event.
        void _somethingHappened(object sender, EventArgs e) {
            OnActionOccurred(e);
        }

        [Bindable(true)]
        [Category("CC_Behavior")]
        [DefaultValue("")]
        [Localizable(false)]
        public string DatabaseBindingFieldName {
            get {
                EnsureChildControls();
                object s = ViewState["DatabaseBindingFieldName"];
                return s == null ? "" : (string)s;
            }
            set {
                EnsureChildControls();
                ViewState["DatabaseBindingFieldName"] = value;
            }
        }
        [Bindable(true)]
        [Category("CC_Appearance")]
        [DefaultValue("")]
        [Localizable(false)]
        public string PromptText {
            get {
                EnsureChildControls();
                object s = ViewState["PromptText"];
                return s == null ? "" : (string)s;
            }
            set {
                EnsureChildControls();
                ViewState["PromptText"] = value;
            }
        }
        [Bindable(true)]
        [Category("CC_UserControlName")]
        [DefaultValue("")]
        [Localizable(false)]
        public string UserControlName {
            get {
                EnsureChildControls();
                object s = ViewState["UserControlName"];
                return s == null ? "" : (string)s;
            }
            set {
                EnsureChildControls();
                ViewState["UserControlName"] = value;
            }
        }

        [
        Bindable(true),
        Category("CC_Appearance"),
        DefaultValue("generalthreecolumnedcontrolmiddlearea"),
        Description(
            "CSSClass Middle")
        ]
        public string CSSClass_Middle {
            get {
                EnsureChildControls();
                String s = (String)ViewState["CSSClass_Middle"];
                return ((s == null) ? "generalthreecolumnedcontrolmiddlearea" : s);
            }
            set {
                EnsureChildControls();
                ViewState["CSSClass_Middle"] = value;
            }
        }
        [
        Bindable(true),
        Category("CC_Appearance"),
        DefaultValue("formlabel"),
        Description(
            "CSSClass Prompt")
        ]
        public string CSSClass_Prompt {
            get {
                EnsureChildControls();
                String s = (String)ViewState["CSSClass_Prompt"];
                return ((s == null) ? "formlabel" : s);
            }
            set {
                EnsureChildControls();
                ViewState["CSSClass_Prompt"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("formalert"),
        Description(
            "CSSClass Alert")
        ]
        public string CSSClass_Alert {
            get {
                EnsureChildControls();
                String s = (String)ViewState["CSSClass_Alert"];
                return ((s == null) ? "formalert" : s);
            }
            set {
                EnsureChildControls();
                ViewState["CSSClass_Alert"] = value;
            }
        }

        [Bindable(true)]
        [Category("CC_Behavior")]
        [DefaultValue(false)]
        [Localizable(false)]
        public bool IsValidationRequired {
            get {
                EnsureChildControls();
                object b = ViewState["IsValidationRequired"];
                return ((b == null) ? false : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["IsValidationRequired"] = value;
            }
        }

#endregion
        protected override void RecreateChildControls() {
            EnsureChildControls();
        }
        protected override void CreateChildControls() {
            _prompt = new Label();
            Controls.Add(_prompt);
            child_CreateControl();

            if (IsValidationRequired) {
                _customValidator = new CustomValidator();
                _customValidator.ControlToValidate = child_Control.ID;
                _customValidator.Display = ValidatorDisplay.Dynamic;
                _customValidator.SetFocusOnError = true;
                Controls.Add(_customValidator);
            }
            base.CreateChildControls();
        }
        protected override void Render(HtmlTextWriter writer) {
            AddAttributesToRender(writer);
/*            try {
                Control control = Page.LoadControl(UserControlName);
                if (_myInnerControl is HasAnIButton) {
                    ((HasAnIButton)_myInnerControl).myIButtonControl.Click += new EventHandler(_somethingHappened);
                    _panel.DefaultButton = _myInnerControl.ID;
                }
                ((BindsToData)_myInnerControl).heresYourDataFieldName(DatabaseBindingFieldName);
                ((BindsToData)_myInnerControl).heresYourScreenId(this.ID);
                _panel.Controls.Add(_myInnerControl);
                int x = 3;
            } catch { }
 * */
            _prompt.Text = PromptText;
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CSSClass_Prompt);
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // --------------- Begin Td ----------------
            _prompt.RenderControl(writer);
            if (!_prompt.Text.Trim().Equals(string.Empty)) {
                writer.Write(":");
            }
            writer.RenderEndTag(); // ------------------------------------- End Td ------------------
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CSSClass_Middle);
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // --------------- Begin Td ----------------
            child_Render(writer);
            child_Control.EnableViewState = true;
            writer.RenderEndTag(); // ------------------------------------- End Td ------------------
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CSSClass_Alert);
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // --------------- Begin Td ----------------
            if (IsValidationRequired) {
                _customValidator.RenderControl(writer);
            }
            writer.RenderEndTag(); // ------------------------------------- End Td ------------------


            writer.RenderEndTag(); // end Tr
        }

    }
    [
    ToolboxData("<{0}:ThreeColumnedRadioButtonListControl runat=server></{0}:ThreeColumnedRadioButtonListControl"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class ThreeColumnedRadioButtonListControl : ThreeColumnedGenericControl {
        private RadioButtonList _radioButtonList;
        private DataSet _dataSource = null;
        #region Design attributes
        [Bindable(true)]
        [Category("CC_Behavior")]
        [DefaultValue("Name")]
        [Localizable(false)]
        public string DataTextField {
            get {
                EnsureChildControls();
                object b = ViewState["DataTextField"];
                return ((b == null) ? "Name" : (string)b);
            }
            set {
                EnsureChildControls();
                ViewState["DataTextField"] = value;
            }
        }
        [Bindable(true)]
        [Category("CC_Behavior")]
        [DefaultValue("Value")]
        [Localizable(false)]
        public string DataValueField {
            get {
                EnsureChildControls();
                object b = ViewState["DataValueField"];
                return ((b == null) ? "Value" : (string)b);
            }
            set {
                EnsureChildControls();
                ViewState["DataValueField"] = value;
            }
        }
        [
        Bindable(true),
        Category("Layout"),
        DefaultValue(0),
        Description("RepeatDirection")
        ]
        public RepeatDirection RepeatDirection {
            get {
                EnsureChildControls();
                object s = ViewState["RepeatDirection"];
                return ((s == null) ? RepeatDirection.Horizontal : (RepeatDirection)s);
            }
            set {
                EnsureChildControls();
                ViewState["RepeatDirection"] = value;
            }
        }
        #endregion
        public DataSet MyDataSet {
            get {
                EnsureChildControls();
                object obj = ViewState["MyDataSet"];
                return obj == null ? null : (DataSet)obj;
            }
            set {
                EnsureChildControls();
                ViewState["MyDataSet"] = value;
            }
        }
        public int SelectedIndexPrimitive {
            get {
                return _radioButtonList.SelectedIndex;
            }
            set {
                _radioButtonList.SelectedIndex = value;
            }
        }
     
        public string SelectedValue {
            get{
                EnsureChildControls();
                object obj = ViewState["SelectedValueBBHBB"];
                return obj == null ? "0" : obj.ToString();
            } set {
                EnsureChildControls();
                ViewState["SelectedValueBBHBB"] = value;
            }
        }
        
        protected override Control child_Control {
            get { return _radioButtonList; }
        }
        protected override void child_CreateControl() {
            _radioButtonList = new RadioButtonList();
            Controls.Add(_radioButtonList);
        }
        protected override void child_Render(HtmlTextWriter writer) {
            _radioButtonList.DataTextField = DataTextField;
            _radioButtonList.DataValueField = DataValueField;
            _radioButtonList.RepeatDirection=RepeatDirection;
            _radioButtonList.DataSource = MyDataSet;
            _radioButtonList.SelectedValue = SelectedValue;
            _radioButtonList.DataBind();
            _radioButtonList.RenderControl(writer);
        }
    }
    [
    ToolboxData("<{0}:ThreeColumnedCheckBoxControl runat=server></{0}:ThreeColumnedCheckBoxControl"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class ThreeColumnedCheckBoxControl : ThreeColumnedGenericControl {
        private CheckBox _checkBox;
        #region Design attributes
        #endregion
        [Bindable(true)]
        [Category("CC_Misc")]
        [DefaultValue(false)]
        [Localizable(false)]
        public bool Checked {
            get {
                EnsureChildControls();
                object b = ViewState["Checked"];
                return ((b == null) ? false : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["Checked"] = value;
            }
        }
        protected override Control child_Control {
            get { return _checkBox; }
        }
        protected override void child_CreateControl() {
            _checkBox = new CheckBox();
            Controls.Add(_checkBox);
        }
        protected override void child_Render(HtmlTextWriter writer) {
            _checkBox.Checked = Checked;
            _checkBox.RenderControl(writer);
        }
    }
    [
    ToolboxData("<{0}:PromptText runat=server></{0}:PromptText"),
    AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class PromptText : CompositeControl {
        private Label _label;
        #region Design attributes
        [Bindable(true)]
        [Category("CC_Appearance")]
        [DefaultValue("")]
        [Localizable(false)]
        public string Text {
            get {
                EnsureChildControls();
                object b = ViewState["Text"];
                return ((b == null) ? string.Empty : (string)b);
            }
            set {
                EnsureChildControls();
                ViewState["Text"] = value;
            }
        }
        [Bindable(true)]
        [Category("CC_Appearance")]
        [DefaultValue("formlabel")]
        [Localizable(false)]
        public string CSS_Class {
            get {
                EnsureChildControls();
                object b = ViewState["CSS_Class"];
                return ((b == null) ? "formlabel" : (string)b);
            }
            set {
                EnsureChildControls();
                ViewState["CSS_Class"] = value;
            }
        }
        [Bindable(true)]
        [Category("CC_Behavior")]
        [DefaultValue(false)]
        [Localizable(false)]
        public bool IsRequired {
            get {
                EnsureChildControls();
                object b = ViewState["IsRequired"];
                return ((b == null) ? false : (bool)b);
            }
            set {
                EnsureChildControls();
                ViewState["IsRequired"] = value;
            }
        }
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("red"),
        Description("Required indicator color.")
        ]
        public string RequiredIndicatorColor {
            get {
                EnsureChildControls();
                object s = ViewState["RequiredIndicatorColor"];
                string str = s == null ? "red" : ((string)s);
                if (str.Trim().Equals("")) {
                    return "red";
                } else {
                    return str.ToLower();
                }
            }
            set {
                EnsureChildControls();
                ViewState["RequiredIndicatorColor"] = value;
            }
        }
        #endregion
        protected override void RecreateChildControls() {
            EnsureChildControls();
        }
        protected override void CreateChildControls() {
            _label = new Label();
            Controls.Add(_label);
            base.CreateChildControls();
        }
        protected override void Render(HtmlTextWriter writer) {
            AddAttributesToRender(writer);
            /*            try {
                            Control control = Page.LoadControl(UserControlName);
                            if (_myInnerControl is HasAnIButton) {
                                ((HasAnIButton)_myInnerControl).myIButtonControl.Click += new EventHandler(_somethingHappened);
                                _panel.DefaultButton = _myInnerControl.ID;
                            }
                            ((BindsToData)_myInnerControl).heresYourDataFieldName(DatabaseBindingFieldName);
                            ((BindsToData)_myInnerControl).heresYourScreenId(this.ID);
                            _panel.Controls.Add(_myInnerControl);
                            int x = 3;
                        } catch { }
             * */
            _label.Text=Text;
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CSS_Class);
//            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            _label.RenderControl(writer);
            if (!_label.Text.Trim().Equals(string.Empty)) {
                if (IsRequired) {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "position:relative;top:-5px;color:" + RequiredIndicatorColor + ";font-size:8pt;font-weight:bold;");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write("*");
                    writer.RenderEndTag();
                }
                writer.Write(":");
            }
  //          writer.RenderEndTag(); // ------------------------------------- End span ------------------
        }
    }
}


