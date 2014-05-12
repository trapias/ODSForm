<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SubmissionManager.ascx.vb" Inherits="ODS.DNN.Modules.Form.SubmissionManager" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<dnn:DnnCssInclude ID="DnnCssInclude1" runat="server" FilePath="~/DesktopModules/Form/js/DataTables-1.9.2/media/css/jquery.dataTables.css"></dnn:DnnCssInclude>
<dnn:DnnCssInclude ID="DnnCssInclude2" runat="server" FilePath="~/DesktopModules/Form/js/DataTables-1.9.2/extras/TableTools/media/css/TableTools_JUI.css"></dnn:DnnCssInclude>
<dnn:DnnCssInclude ID="DnnCssInclude3" runat="server" FilePath="~/DesktopModules/Form/js/ui/css/smoothness/jquery-ui-1.8.21.custom.css"></dnn:DnnCssInclude>
<dnn:DnnJsInclude ID="DnnJsInclude5" runat="server" FilePath="~/DesktopModules/Form/js/DataTables-1.9.2/media/js/jquery.dataTables.min.js"></dnn:DnnJsInclude>
<dnn:DnnJsInclude ID="DnnJsInclude1" runat="server" FilePath="~/DesktopModules/Form/js/DataTables-1.9.2/extras/TableTools/media/js/TableTools.min.js"></dnn:DnnJsInclude>
<script type="text/javascript">
    $(function () {
        $('#<%=submissions.ClientID %>').dataTable({
            "bProcessing": true,
            "bServerSide": true,
            "sAjaxSource": '<%=ResolveURL("~/DesktopModules/Form/json.aspx?cmd=1&mid=" & me.ModuleID)%>',
            "bJQueryUI": true,
            "bPaginate": true,
            "bDestroy": true,
            "sPaginationType": "full_numbers",
            "sDom": 'T<"clear">lfrtip',
            "oTableTools": {
                "sSwfPath": "<%=ResolveURL("~/DesktopModules/Form/js/DataTables-1.9.2/extras/TableTools/media/swf/copy_csv_xls_pdf.swf")%>",
            },
            "aoColumns": [
               { "mDataProp": "SubmissionDate", sTitle: "Date"}, 
               {
                   "mDataProp": "SubmissionXML", sTitle: "Submission",
                   "fnRender": function (o, val) {
                       if (val == undefined || val == "")
                return "";
              var x =  $.parseXML(val);
              $x = $(x);
              var sentto = $x.find("SentToUser").text();
                       var ss = "";
              var clt = "";
                       $x.find("FormItem").each(function ()
              {
                clt = $(this).attr("Culture");
                ss +=  "<b>" + $(this).attr("FormLabel") + ":</b> " + $(this).text() + "<br />";
              });

              if (sentto == "") {
                           if (clt != undefined && clt!='')
                  return "<b>Locale:</b> " + clt + "<br/>" + ss;
                           else return ss;
              }
              else {
                  return "<b>Sent to:</b> " + sentto + "<br/>" + "<b>Locale:</b> " + clt + "<br/>" + ss;
              }
                   }
               }
               ],
            "aaSorting": [[0, "desc"]]
        });
    });
</script>
<table id="submissions" runat="server">
</table>
