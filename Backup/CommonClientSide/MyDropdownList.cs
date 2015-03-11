using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CommonClientSide {
    [ToolboxData("<{0}:MyDropdownList runat=server></{0}:MyDropdownList>")]
    public class MyDropdownList : DropDownList {
        protected override void RenderContents(HtmlTextWriter writer) {
            if (Items != null) {
                bool selected = false;

                foreach (ListItem listItem in Items) {
                    writer.WriteBeginTag("option");

                    if (listItem.Selected) {
                        if (selected)
                            //throw new HttpException(HttpRuntime.FormatResourceString("Cannot_Multiselect_In_DropDownList"));
                            throw new HttpException("Cannot multiselect in DropDownList."); //TODO: this should be language independent
                        selected = true;
                        writer.WriteAttribute("selected", "selected", false);
                    }
                    writer.WriteAttribute("value", listItem.Value, true);

                    listItem.Attributes.Render(writer); // This is the added code
                    //TODO: What happens if "value" or "selected" are in listItem.Attributes?

                    writer.Write('>');
                    HttpUtility.HtmlEncode(listItem.Text, writer);

                    writer.WriteEndTag("option");
                    writer.WriteLine();
                }
            }
        }
    }
}
