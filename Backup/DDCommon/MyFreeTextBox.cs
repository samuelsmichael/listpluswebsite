using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DDCommon {
    [ToolboxData("<{0}:MyFreeTextBox runat=server></{0}:MyFreeTextBox>")]
    public class MyFreeTextBox : FreeTextBoxControls.FreeTextBox {
        protected override void Render(HtmlTextWriter writer) {
            base.Render(writer);
        }
    }
}