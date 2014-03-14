<%@ Control Language="vb" AutoEventWireup="false" CodeFile="Settings.ascx.vb" Inherits="ODS.DNN.Modules.Form.Settings" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="cc1" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="UrlControl" Src="~/controls/UrlControl.ascx"%>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<table cellspacing="0" cellpadding="2" border="0" width="100%" summary="ODS Form Settings">
    <tr valign="top">
		<td class="SubHead" width="175"><dnn:label id="lblModuleTitle" runat="server" controlname="txtModuleTitle" suffix=":" resourcekey="lblModuleTitle"></dnn:label></td>
		<td valign="bottom">
            <asp:DropDownList ID="ddlLocales" runat="server" AutoPostBack="true"></asp:DropDownList>
			<asp:TextBox Runat="server" ID="txtModuleTitle" Width="100%" />
		</td>
	</tr>
    <tr valign="top">
		<td class="SubHead" width="175"><dnn:label id="lblEnableLocalization" runat="server" controlname="chkEnableLocalization" suffix=":" resourcekey="lblEnableLocalization"></dnn:label></td>
		<td valign="bottom">
            <asp:CheckBox ID="chkEnableLocalization" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="175" valign="top"><dnn:label id="plSendMode" runat="server" controlname="plSendMode" suffix=":" resourcekey="plSendMode"
				helptext="How to save the submissons"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox id="cbEmail" runat="server" Text="Send e-mail" resourcekey="cbEmail"></asp:CheckBox><br/>
			<asp:CheckBox id="cbDatabase" runat="server" Text="Save in database" resourcekey="cbDatabase"></asp:CheckBox><br />
            <asp:CheckBox id="chkContactUsers" runat="server" Text="Allow to contact users" resourcekey="ContactUsers"></asp:CheckBox><br />
            <asp:CheckBox id="chkMailto" runat="server" Text="Allow dynamic mailto recipient" resourcekey="AllowMailto"></asp:CheckBox>
		</td>
	</tr>
    <tr valign="top">
		<td class="SubHead" width="175"><dnn:label id="lblEmailSender" runat="server" controlname="txtEmailSender" suffix=":" resourcekey="EmailSender"
				helptext="Adresses to use as email sender"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox Runat="server" ID="txtEmailSender" Width="100%" />
		</td>
	</tr>
	<tr valign="top">
		<td class="SubHead" width="175"><dnn:label id="plEmail" runat="server" controlname="plEmail" suffix=":" resourcekey="plEmail"
				helptext="Adresses to send the form. Enter the e-mail adresses separated by ;"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox Runat="server" ID="tbEmail" Width="100%" />
		</td>
	</tr>
     <tr valign="top">
		<td class="SubHead" width="175"><dnn:label id="lblCustomDomains" runat="server" controlname="txtCustomDomains" suffix=":" resourcekey="CustomDomains"
				helptext="Custom domain names to validate mailto automation, semicolon (;) separated"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox Runat="server" ID="txtCustomDomains" Width="100%" />
		</td>
	</tr>
	<tr valign="top">
		<td class="SubHead" width="175"><dnn:label id="lblClass" runat="server" controlname="plClass" suffix=":" resourcekey="plClass"
				helptext="Custom css class"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox Runat="server" ID="plClass" />
		</td>
	</tr>
    <tr valign="top">
		<td class="SubHead" width="175"><dnn:label id="lblSubmitText" runat="server" controlname="txtSubmitText" suffix=":" resourcekey="lblSubmitText"
				helptext="Custom css class"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox Runat="server" ID="txtSubmitText" />
		</td>
	</tr>
    <tr valign="top">
		<td class="SubHead" width="175"><dnn:label id="lblResetText" runat="server" controlname="txtResetText" suffix=":" resourcekey="lblResetText"
				helptext="Custom css class"></dnn:label></td>
		<td valign="bottom">
			<asp:TextBox Runat="server" ID="txtResetText" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="175" valign="top"><dnn:label id="lblCaptcha" runat="server" controlname="chkCaptcha" suffix=":" resourcekey="lblCaptcha"
				helptext="Use Captcha?"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox id="chkCaptcha" runat="server" Text="Enable Captcha" resourcekey="lblCaptcha"></asp:CheckBox>
			<asp:DropDownList ID="ddlCaptchaMode" runat="server" CssClass="Normal">
			<asp:ListItem Value="0" Text="Little"></asp:ListItem>
			<asp:ListItem Value="1" Text="Medium"></asp:ListItem>
			<asp:ListItem Value="2" Text="Large"></asp:ListItem>
			</asp:DropDownList>
			<asp:DropDownList ID="ddlCaptchaLength" runat="server" CssClass="Normal">
			<asp:ListItem Value="5" Text="5 characters"></asp:ListItem>
			<asp:ListItem Value="6" Text="6 characters"></asp:ListItem>
			<asp:ListItem Value="7" Text="7 characters"></asp:ListItem>
			<asp:ListItem Value="8" Text="8 characters"></asp:ListItem>
			<asp:ListItem Value="9" Text="9 characters"></asp:ListItem>
			<asp:ListItem Value="10" Text="10 characters"></asp:ListItem>
			</asp:DropDownList>
			<asp:CheckBox ID="chkNumbersOnly" runat="server" Text="Numbers only?" />
		</td>
	</tr>
	<tr valign="top">
		<td class="SubHead" width="175"><dnn:label id="lblValSum" runat="server" controlname="chkValSum" suffix=":" resourcekey="lblValSum"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkValSum" runat="server" />
            <asp:DropDownList ID="posValSum" runat="server" CssClass="Normal">
			<asp:ListItem Value="0" Text="Top"></asp:ListItem>
			<asp:ListItem Value="1" Text="Bottom"></asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
    <tr valign="top">
		<td class="SubHead" width="175"><dnn:label id="lblHideReset" runat="server" controlname="chkValSum" suffix=":" resourcekey="lblHideReset"></dnn:label></td>
		<td valign="bottom">
			<asp:CheckBox ID="chkHideReset" runat="server" />
		</td>
	</tr>
    <tr>
        <td colspan="2"><dnn:SectionHead ID="submConfirmSectionHead" CssClass="Head" runat="server" Text="Submission Confirmation" Section="submConfirmSection" IsExpanded="false"></dnn:SectionHead></td>
    </tr>
    <tr>
        <td colspan="2">
            <table id="submConfirmSection" runat="server">
            <tr valign="top">
		        <td class="SubHead" width="175"><dnn:label id="lblConfirmSubmission" runat="server" controlname="lblConfirmSubmission" suffix=":" resourcekey="lblConfirmSubmission"></dnn:label></td>
		        <td valign="bottom">
			        <asp:DropDownList ID="ddlSubmConfMode" runat="server" CssClass="Normal" AutoPostBack="true">
                    <asp:ListItem Value="0" Text="Message"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Redirect to URL"></asp:ListItem>
                    </asp:DropDownList>
		        </td>
	        </tr>
            <tr id="trConfirmMessageTitle" runat="server">
                <td class="SubHead" width="175"><dnn:label id="lblConfirmMessageTitle" runat="server" controlname="lblConfirmMessageTitle" suffix=":" resourcekey="lblConfirmMessageTitle"></dnn:label></td>
                <td>
                    <asp:TextBox ID="txtConfirmTitle" runat="server" CssClass="Normal"></asp:TextBox>
                </td>
            </tr>
            <tr id="trConfirmMessage" runat="server">
                <td class="SubHead" width="175"><dnn:label id="lblConfirmMessage" runat="server" controlname="lblConfirmMessage" suffix=":" resourcekey="lblConfirmMessage"></dnn:label></td>
                <td>
                    <dnn:texteditor ID="txtConfirmMessage" runat="server"></dnn:texteditor>
                </td>
            </tr>
            <tr id="trConfirmURL" runat="server">
                <td class="SubHead" width="175"><dnn:label id="plURL" runat="server" controlname="plURL" suffix=":" resourcekey="plURL" helptext="Redirect URL after form is submitted."></dnn:label></td>
		        <td valign="bottom">
			        <dnn:UrlControl ID="tbURL" runat="server" ShowFiles="false" ShowDatabase="false" ShowImages="false" ShowLog="false" />
		        </td>
            </tr>
            </table>
        </td>
    </tr>
	<tr>
        <td colspan="2"><dnn:SectionHead ID="secCustomCSSHead" CssClass="Head" runat="server" Text="Custom CSS Classes" Section="secCustomCSS" IsExpanded="false"></dnn:SectionHead></td>
    </tr>
      <tr>
        <td colspan="2">
            <table id="secCustomCSS" runat="server">
            <tr valign="top">
		        <td class="SubHead" width="175"><dnn:label id="lblButtonsClass" runat="server" controlname="lblButtonsClass" suffix=":" resourcekey="lblButtonsClass"
				        helptext="Enter the CSS Class to be used for buttons. Default is 'medium FormButton orange'"></dnn:label></td>
		        <td valign="bottom">
			        <asp:TextBox Runat="server" ID="txtButtonsClass" />
		        </td>
	        </tr>
            <tr valign="top">
		        <td class="SubHead" width="175"><dnn:label id="lblCSSLabels" runat="server" controlname="txtCSSLabels" suffix=":" resourcekey="lblCSSLabels"
				        helptext="CSS class for labels"></dnn:label></td>
		        <td valign="bottom">
                    <asp:TextBox Runat="server" ID="txtCSSLabels" />
		        </td>
	        </tr>
	        <tr valign="top">
		        <td class="SubHead" width="175"><dnn:label id="lblCSSCaptcha" runat="server" controlname="txtCSSCaptcha" suffix=":" resourcekey="lblCSSCaptcha"
				        helptext="CSS class for Captcha"></dnn:label></td>
		        <td valign="bottom">
                    <asp:TextBox Runat="server" ID="txtCSSCaptcha" />
		        </td>
	        </tr>
            </table>
        </td>
        </tr>
</table>
