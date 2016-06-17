<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SubmissionManager.ascx.vb" Inherits="ODS.DNN.Modules.Form.SubmissionManager" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%--<dnn:DnnCssInclude ID="DnnCssInclude1" runat="server" FilePath="~/DesktopModules/Form/js/DataTables-1.9.2/media/css/jquery.dataTables.css" Priority="150"></dnn:DnnCssInclude>
<dnn:DnnCssInclude ID="DnnCssInclude2" runat="server" FilePath="~/DesktopModules/Form/js/DataTables-1.9.2/extras/TableTools/media/css/TableTools_JUI.css" Priority="151"></dnn:DnnCssInclude>
<dnn:DnnCssInclude ID="DnnCssInclude3" runat="server" FilePath="~/DesktopModules/Form/js/ui/css/smoothness/jquery-ui-1.8.21.custom.css" Priority="152"></dnn:DnnCssInclude>
<dnn:DnnJsInclude ID="DnnJsInclude5" runat="server" FilePath="~/DesktopModules/Form/js/DataTables-1.9.2/media/js/jquery.dataTables.min.js" Priority="150"></dnn:DnnJsInclude>
<dnn:DnnJsInclude ID="DnnJsInclude1" runat="server" FilePath="~/DesktopModules/Form/js/DataTables-1.9.2/extras/TableTools/media/js/TableTools.min.js" Priority="151"></dnn:DnnJsInclude>--%>
<dnn:DnnCssInclude ID="dt" runat="server" FilePath="//cdn.datatables.net/1.10.6/css/jquery.dataTables.min.css" Priority="150"></dnn:DnnCssInclude>
<dnn:DnnCssInclude ID="DnnCssInclude1" runat="server" FilePath="//cdn.datatables.net/plug-ins/1.10.6/integration/bootstrap/3/dataTables.bootstrap.css" Priority="151"></dnn:DnnCssInclude>
<dnn:DnnCssInclude ID="tt" runat="server" FilePath="//cdn.datatables.net/tabletools/2.2.4/css/dataTables.tableTools.css" Priority="152"></dnn:DnnCssInclude>
<dnn:DnnJsInclude ID="dtjs" runat="server" FilePath="//cdn.datatables.net/1.10.6/js/jquery.dataTables.min.js" Priority="150"></dnn:DnnJsInclude>
<dnn:DnnJsInclude ID="datatables" runat="server" FilePath="//cdn.datatables.net/plug-ins/1.10.6/integration/bootstrap/3/dataTables.bootstrap.js" Priority="151"></dnn:DnnJsInclude>
<dnn:DnnJsInclude ID="ttjs" runat="server" FilePath="//cdn.datatables.net/tabletools/2.2.4/js/dataTables.tableTools.min.js" Priority="152"></dnn:DnnJsInclude>
<script type="text/javascript">
    $(function () {

        var colonne = [], valori = [];
        var co1 = { 'title': 'SumbmissionDate' };
        colonne.push(co1);
        var co2 = { 'title': 'Culture' };
        colonne.push(co2);

        //load data and build columns, cfr http://datatables.net/examples/data_sources/js_array.html
        $.get('<%=ResolveURL("~/DesktopModules/Form/json.aspx?cmd=1&mid=" & me.ModuleID)%>&portalid=<%=Me.PortalId%>&sEcho=1&iDisplayLength=10&sSearch=', function (data) {

            //console.log('DATA: ' + data);
            var j = JSON.parse(data);
            //console.log('TOTAL:' + j.iTotalRecords);

            //step1: columns
            for (n = 0; n < j.aaData.length; n++) {
                var x = $.parseXML(j.aaData[n].SubmissionXML);
                $x = $(x);
                $x.find("FormItem").each(function () {

                    //TextBox = 1
                    //TextArea = 2
                    //DropDownList = 3
                    //MultipleSelect = 4
                    //Checkbox = 5
                    //Label = 6
                    //RadioButtonList = 7
                    //DNNRichTextEditControl = 8
                    //FileUpload = 9
                    //HiddenField = 10
                   // if ($(this).attr("FormType") != 6) {
                        if (n === 0) {
                            var co = { 'title': $(this).attr("FormLabel") };
                            colonne.push(co);
                        }
                    //}
                });
            }
            //for (i = 0; i < colonne.length; i++) {
            //    console.log('COL ' + JSON.stringify( colonne[i]) );
            //}

            //step2: values
            for (n = 0; n < j.aaData.length; n++) {
                var x = $.parseXML(j.aaData[n].SubmissionXML);
                $x = $(x);

                var pushcol = 2;
                var va = [];
                $x.find("FormItem").each(function () {
                    //console.log(this);
                    va[0] = j.aaData[n].SubmissionDate;
                    va[1] = $(this).attr('Culture');

                    var item = $(this).find("SubmissionValue");
                    for (i = pushcol; i < colonne.length; i++) {
                        if ($(this).attr("FormLabel") == colonne[i].title) {
                            //console.log(n + ") PARSE " + $(this).attr("FormLabel") + ' = ' + item.text() + ' PUSH COLUMN ' + pushcol);
                            va[pushcol] = item.text();
                            pushcol++;
                        }
                    }

                    if (va.length == colonne.length)
                        valori.push(va);

                });
            }

            //for (i = 0; i < valori.length; i++) {
            //    console.log('VAL ' + JSON.stringify(valori[i]));
            //}

            $('#<%=submissions.ClientID %>').dataTable({
                columns: colonne,
                data: valori,
                "bPaginate": true,
                "sPaginationType": "full_numbers",
                "sDom": 'T<"clear">lfrtip',
                "oTableTools": {
                    "sSwfPath": "<%=ResolveUrl("~/DesktopModules/Form/js/DataTables-1.10.6/extensions/TableTools/swf/copy_csv_xls_pdf.swf")%>",
                    "aButtons": [
                    {
                        "sExtends": "csv",
                        "sFieldSeperator": "<%IIf(System.Threading.Thread.CurrentThread.CurrentCulture.Name.StartsWith("it"), ";", ",")%>;"
                    }, "xls", "pdf", "print"]
                },
                "aaSorting": [[0, "desc"]]
            });
        });

    });
</script>
<table id="submissions" runat="server">
</table>
