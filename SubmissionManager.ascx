<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SubmissionManager.ascx.vb" Inherits="ODS.DNN.Modules.Form.SubmissionManager" %>
<script src="<%=ResolveURL("~/DesktopModules/Form/js/DataTables-1.9.2/media/js/jquery.dataTables.min.js")%>" type="text/javascript"></script>
<script src="<%=ResolveURL("~/DesktopModules/Form/js/DataTables-1.9.2/extras/TableTools/media/js/TableTools.min.js")%>" type="text/javascript"></script>
<link href="<%=ResolveURL("~/DesktopModules/Form/js/DataTables-1.9.2/media/css/jquery.dataTables.css")%>" rel="stylesheet" type="text/css" />
<link href="<%=ResolveURL("~/DesktopModules/Form/js/DataTables-1.9.2/extras/TableTools/media/css/TableTools_JUI.css")%>" rel="stylesheet" type="text/css" />
<link href="<%=ResolveURL("~/DesktopModules/Form/js/ui/css/smoothness/jquery-ui-1.8.21.custom.css")%>" rel="stylesheet" type="text/css" />
<script src="<%=ResolveURL("~/DesktopModules/Form/js/ui/jquery-ui-1.8.21.custom.min.js")%>" type="text/javascript"></script>
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
