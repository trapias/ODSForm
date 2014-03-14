<%@ Control Language="vb" AutoEventWireup="false" CodeFile="Form.ascx.vb" Inherits="ODS.DNN.Modules.Form.Form" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="cc1" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls"%>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<script type="text/javascript">
    jQuery(function ($) {
        $('a.confirm').dnnConfirm();
    });
</script>
<asp:PlaceHolder ID="plcODS" runat="server"></asp:PlaceHolder>
