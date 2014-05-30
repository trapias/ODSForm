<%@ Control Language="vb" AutoEventWireup="false" CodeFile="FormEdit.ascx.vb" Inherits="ODS.DNN.Modules.Form.FormEdit" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<script type="text/javascript">
    jQuery(function ($) {
        $('.dnnActions .confirm').dnnConfirm();
    });
</script>
<asp:Label id="lblHelp" runat="server"></asp:Label>
<div class="dnnForm">
 <fieldset>
     <legend>Edit form field</legend>
     <div class="dnnFormItem">
         <dnn:label id="plCulture" runat="server" controlname="lblCulture" suffix=":" resourcekey="plCulture" helptext=""></dnn:label>
         <asp:Label ID="lblCulture" runat="server"></asp:Label>
     </div>
     <div class="dnnFormItem">
         <dnn:label helptext="" id="plFormType" runat="server" controlname="plFormType" suffix=":" resourcekey="plFormType"></dnn:label>
         <asp:DropDownList id="ddlFormTypes" runat="server" AutoPostBack="True">
				<asp:ListItem Value="1">TextField</asp:ListItem>
				<asp:ListItem Value="2">TextArea</asp:ListItem>
				<asp:ListItem Value="3">DropDownList</asp:ListItem>
				<asp:ListItem Value="4">MultipleSelect</asp:ListItem>
				<asp:ListItem Value="5">Checkbox</asp:ListItem>
				<asp:ListItem Value="6">Label</asp:ListItem>
				<asp:ListItem Value="7">Radiobuttons</asp:ListItem>
				<asp:ListItem Value="8">DNNRichTextEditControl</asp:ListItem>
				<asp:ListItem Value="9">File Upload</asp:ListItem>
                <asp:ListItem Value="10">Hidden Field</asp:ListItem>
			</asp:DropDownList>
     </div>
     <div class="dnnFormItem" id="trMultipleSelectCol" runat="server">
         <dnn:label id="lblMultipleSelectCol" runat="server" controlname="tbFormLabel" suffix=":" resourcekey="lblMultipleSelectCol" helptext=""></dnn:label>
         <asp:DropDownList ID="ddlMultipleSelectCol" runat="server">
             <asp:ListItem Value="0">1 column</asp:ListItem>
             <asp:ListItem Value="2">2 columns</asp:ListItem>
             <asp:ListItem Value="3">3 columns</asp:ListItem>
             <asp:ListItem Value="4">4 columns</asp:ListItem>
         </asp:DropDownList>
     </div>
     <div class="dnnFormItem" id="trFormLabel" runat="server">
         <dnn:label id="plFormLabel" runat="server" controlname="tbFormLabel" suffix=":" resourcekey="plFormLabel" helptext=""></dnn:label>
         <asp:TextBox Runat="server" ID="tbFormLabel" MaxLength="255" />
     </div>
     <div class="dnnFormItem" id="trFieldTitle" runat="server">
         <dnn:label id="lblFormItemTitle" runat="server" controlname="txtFormItemTitle" suffix=":" resourcekey="plFormItemTitle" helptext=""></dnn:label>
         <asp:TextBox Runat="server" ID="txtFormItemTitle" />
     </div>
     <div class="dnnFormItem" id="trWidth" runat="server">
         <dnn:label id="lblWidth" runat="server" controlname="txtWidth" suffix=":" resourcekey="Width" helptext=""></dnn:label>
         <asp:TextBox Runat="server" ID="txtWidth" />
     </div>
     <div class="dnnFormItem" id="trHeight" runat="server">
         <dnn:label id="lblHeight" runat="server" controlname="txtHeight" suffix=":" resourcekey="Height" helptext=""></dnn:label>
         <asp:TextBox Runat="server" ID="txtHeight" />
     </div>
     <div class="dnnFormItem" id="trLabelCSS" runat="server">
        <dnn:label id="lblFormLabelClass" runat="server" controlname="txtFormLabelClass" suffix=":" resourcekey="lblFormLabelClass" helptext=""></dnn:label>
         <asp:TextBox Runat="server" ID="txtFormLabelClass" />
     </div>
     <div class="dnnFormItem" id="trControlCSS" runat="server">
         <dnn:label id="lblCustomClass" runat="server" controlname="txtCustomClass" suffix=":" resourcekey="lblCustomClass" helptext=""></dnn:label>
         <asp:TextBox Runat="server" ID="txtCustomClass" />
     </div>
     <div class="dnnFormItem" id="trFormValues" runat="server">
         <dnn:label id="plFormValues" runat="server" controlname="tbFormValues" suffix=":" resourcekey="plFormValues" helptext=""></dnn:label>
         <asp:TextBox Runat="server" ID="tbFormValues" />
     </div>
     <div class="dnnFormItem" id="trCustomRegex" runat="server">
         <dnn:label id="lblCustomRegex" runat="server" controlname="txtCustomRegex" suffix=":" resourcekey="plCustomRegex" helptext=""></dnn:label>
         <asp:TextBox Runat="server" ID="txtCustomRegex" />
     </div>
     <div class="dnnFormItem" id="trSelectedValues" runat="server">
         <dnn:label id="plSelectedValues" runat="server" controlname="tbSelectedValues" suffix=":" resourcekey="plSelectedValues" helptext=""></dnn:label>
         <asp:TextBox Runat="server" ID="tbSelectedValues" />
     </div>
     <div class="dnnFormItem" id="trRequired" runat="server">
         <dnn:label id="plRequired" runat="server" controlname="plRequired" suffix=":" resourcekey="plRequired" helptext=""></dnn:label>
         <asp:CheckBox id="cbRequired" runat="server"></asp:CheckBox>
     </div>
     <div class="dnnFormItem" id="trAllowValueOverride" runat="server">
         <dnn:label id="lblAllowValueOverride" runat="server" controlname="chkAllowValueOverride" suffix=":" resourcekey="lblAllowValueOverride" helptext=""></dnn:label>
         <asp:CheckBox id="chkAllowValueOverride" runat="server"></asp:CheckBox>
     </div>
     <div class="dnnFormItem" id="trFilePath" runat="server">
         <dnn:label id="lblFilePath" runat="server" controlname="txtFilePath" suffix=":" resourcekey="lblFilePath" helptext=""></dnn:label>
         <asp:DropDownList ID="ddlFilePath" runat="server" CssClass="Normal"></asp:DropDownList>
     </div>
</fieldset>
</div>
<ul class="dnnActions dnnClear">
    <li><asp:linkbutton id="cmdUpdate" CssClass="dnnPrimaryAction" runat="server" BorderStyle="None" resourcekey="cmdUpdate">Update</asp:linkbutton></li>
    <li><asp:linkbutton id="cmdUpdateContinue" CssClass="dnnSecondaryAction" runat="server" BorderStyle="None" resourcekey="cmdUpdateContinue">Update and continue</asp:linkbutton></li>
    <li><asp:linkbutton id="cmdCancel" CssClass="dnnSecondaryAction" runat="server" CausesValidation="False" BorderStyle="None" resourcekey="cmdCancel">Cancel</asp:linkbutton></li>
    <li><asp:linkbutton id="cmdDelete" CssClass="confirm dnnSecondaryAction" runat="server" CausesValidation="False" BorderStyle="None" Visible="False" resourcekey="cmdDelete">Delete</asp:linkbutton></li>
    <li><asp:linkbutton id="lbMoveUp" CssClass="dnnSecondaryAction" runat="server" CausesValidation="False" BorderStyle="None" Visible="False" resourcekey="cmdUp">Move up</asp:linkbutton></li>
    <li><asp:linkbutton id="lbMoveDown" CssClass="dnnSecondaryAction" runat="server" CausesValidation="False" BorderStyle="None" Visible="False" resourcekey="cmdDown">Move Down</asp:linkbutton></li>
</ul>
<dnn:audit id="ctlAudit" runat="server" />
