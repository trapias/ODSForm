<%@ Control Language="vb" AutoEventWireup="false" Codefile="FormView.ascx.vb" Inherits="ODS.DNN.Modules.Form.FormView" %>
<p>
	<asp:DropDownList id="ddlSubmissions" runat="server" AutoPostBack="True"></asp:DropDownList>&nbsp;<br/>
	<br/>
	<asp:Label id="lblSubmission" runat="server"></asp:Label><br/>
	<br/>
	<asp:linkbutton id="cmdCancel" runat="server" CausesValidation="False" CssClass="CommandButton"
		BorderStyle="None" resourcekey="cmdCancel">Cancel</asp:linkbutton>&nbsp;
	<asp:linkbutton id="cmdDelete" runat="server" Visible="True" CausesValidation="False" CssClass="CommandButton"
		BorderStyle="None" resourcekey="cmdDelete">Delete</asp:linkbutton>
</p>