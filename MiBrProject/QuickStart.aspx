<%@ Page MasterPageFile="~/Site1.Master" Language="C#" AutoEventWireup="true" CodeBehind="QuickStart.aspx.cs" Inherits="MiBrProject.QuickStart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align:center;margin:20px;">
    <h2>Just 3 easy steps!</h2>
                                <asp:Panel ID="PanelAddALocationControl" runat="server" CssClass="collapsePanelHeader" Height="30px">
                                    <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                        <div style="float: left;">
                                            Step 1 - CREATE LOCATIONS</div>
                                        <div style="float: left; margin-left: 40px; font-size:small;">
                                            <asp:Label ID="LabelAddALocation" runat="server"></asp:Label>
                                        </div>
                                        <div style="float: right; vertical-align: middle;">
                                            <asp:ImageButton CausesValidation="False" ID="ImageAddALocationControl" runat="server" ImageUrl="~/images/expand_blue.jpg" AlternateText="(Show details...)" />
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="PanelAddALocation" runat="server">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td style="width:193px;">
                                    <asp:Image ID="Image1" ImageUrl="~/images/AddALocation.JPG" Height="327" Width="190" ImageAlign="Left" runat="server" />
                                            </td>
                                            <td>
                                    <p class="quick-start-verbiage">
                                    Press the Location tab and then press Menu and then New Location.  Don't forget to press Confirm when you're done.</p>
                                    <p>Eventually, you'll have lots of Locations.
                                    </p>
                                            </td>
                                            <td style="width: 330px;">
                                    <asp:Image ID="Image2" ImageUrl="~/images/UpdateALocation.JPG" Height="327" Width="322" ImageAlign="Right" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="PanelAddALocation"
                                    ExpandControlID="PanelAddALocationControl" CollapseControlID="PanelAddALocationControl" TextLabelID="LabelAddALocation"
                                    ImageControlID="ImageAddALocationControl" CollapsedText="Show details..." ExpandedText="Hide details..."
                                    ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg" Collapsed="true"
                                    SuppressPostBack="True" Enabled="True" />
                                <asp:Panel ID="PanelAddANeedControl" runat="server" CssClass="collapsePanelHeader" Height="30px">
                                    <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                        <div style="float: left;">
                                            Step 2 - CREATE A NEED</div>
                                        <div style="float: left; margin-left: 40px; font-size:small;">
                                            <asp:Label ID="LabelAddANeed" runat="server"></asp:Label>
                                        </div>
                                        <div style="float: right; vertical-align: middle;">
                                            <asp:ImageButton CausesValidation="False" ID="ImageAddANeedControl" runat="server" ImageUrl="~/images/expand_blue.jpg" AlternateText="(Show details...)" />
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="PanelAddANeed" runat="server">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td style="width:193px;">
                                    <asp:Image ID="ImageAddANeed" ImageUrl="~/images/AddANeed.JPG" Height="327" Width="190" ImageAlign="Left" runat="server" />
                                            </td>
                                            <td>
                                    <p class="quick-start-verbiage">
                                    At the Need tab, press Menu, and then Add Need.  Key in the Item, and optionally a description.  Press Work With Locations.  Again, don't forget to press Confirm when you're done.
                                    </p>
                                            </td>
                                            <td  style="width:193px;">
                                    <asp:Image ID="Image3" ImageUrl="~/images/UpdateANeed.JPG" Height="327" Width="190" ImageAlign="Right" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="cpeAddANeed" runat="server" TargetControlID="PanelAddANeed"
                                    ExpandControlID="PanelAddANeedControl" CollapseControlID="PanelAddANeedControl" TextLabelID="LabelAddANeed"
                                    ImageControlID="ImageAddANeedControl" CollapsedText="Show details..." ExpandedText="Hide details..."
                                    ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg" Collapsed="true"
                                    SuppressPostBack="True" Enabled="True" />
                                <asp:Panel ID="PanelAssociateANeedControl" runat="server" CssClass="collapsePanelHeader" Height="30px">
                                    <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                        <div style="float: left;">
                                            Step 3 - ASSOCIATE THE NEED WITH ONE OR MORE LOCATIONS</div>
                                        <div style="float: left; margin-left: 40px; font-size:small;">
                                            <asp:Label ID="LabelAssociateANeed" runat="server"></asp:Label>
                                        </div>
                                        <div style="float: right; vertical-align: middle;">
                                            <asp:ImageButton CausesValidation="False" ID="ImageAssociateANeedControl" runat="server" ImageUrl="~/images/expand_blue.jpg" AlternateText="(Show details...)" />
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="PanelAssociateANeed" runat="server">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td style="width:193px;">
                                    <asp:Image ID="Image4" ImageUrl="~/images/AssociateNeedWithLocations.JPG" Height="327" Width="190" ImageAlign="Left" runat="server" />
                                            </td>
                                            <td>
                                    <p class="quick-start-verbiage">
                                    Check whichever Locations have what you need.  That's it!  Now, whenever you get near any of the designated Locations, you'll receive a notification:
                                    </p>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="padding-top:25px;" cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td>
                                    <asp:Image ID="Image5" ImageUrl="~/images/Notification1.JPG" Height="327" Width="190" ImageAlign="Left" runat="server" />
                                            </td>
                                            <td>
                                    <asp:Image ID="Image6" ImageUrl="~/images/Notification2.JPG" Height="327" Width="190" ImageAlign="Left" runat="server" />
                                            </td>
                                            <td>
                                    <asp:Image ID="Image7" ImageUrl="~/images/Notification3.JPG" Height="327" Width="190" ImageAlign="Left" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="PanelAssociateANeed"
                                    ExpandControlID="PanelAssociateANeedControl" CollapseControlID="PanelAssociateANeedControl" TextLabelID="LabelAssociateANeed"
                                    ImageControlID="ImageAssociateANeedControl" CollapsedText="Show details..." ExpandedText="Hide details..."
                                    ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg" Collapsed="true"
                                    SuppressPostBack="True" Enabled="True" />
</div>
</asp:Content>