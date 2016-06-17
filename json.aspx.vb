Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports ODS.DNN.Modules.Form.Business
Imports Newtonsoft.Json ' http://james.newtonking.com/projects/json/help/
Imports DotNetNuke.Instrumentation

Namespace ODS.DNN.Modules.Form

    Partial Class json
        Inherits DotNetNuke.Framework.CDefault

        Private libName As String = "ODS.DNN.Modules.Form.JSON"

        'Public Sub MyLog(ByVal logMessage As String)
        '    DnnLog.Debug(logMessage)
        '    'Dim w As IO.StreamWriter = IO.File.AppendText(MapPath("~/DesktopModules/Form/json.log"))
        '    'w.WriteLine("{0} {1} :{2}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString(), logMessage)
        '    'w.Flush()
        '    'w.Close()
        'End Sub

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then

                    'filter access to know users only (todo: improve this!)
                    If Not Request.IsAuthenticated Then
                        Return
                    End If

                    Dim ps As PortalSettings = DotNetNuke.Common.GetPortalSettings()

                    Response.Clear()
                    Response.Buffer = False
                    Response.ContentType = "text/plain"

                    Select Case Request("cmd")

                        Case "1" 'List Submissions for module or module&locale
                            Dim mid As String = Request("mid")
                            Dim loc As String = Request("loc")
                            Dim search As String = Request("sSearch")
                            Dim start As Integer = Request("iDisplayStart")
                            Dim pageSize As Integer = Request("iDisplayLength")
                            Dim iSortCol_0 As Integer = Request("iSortCol_0")
                            Dim sSortDir_0 As String = Request("sSortDir_0")
                            Dim MaxRecords As Integer = 0
                            Dim pid As Integer = ps.PortalId
                            If Request("PortalID") <> "" Then
                                pid = Request("portalid")
                            End If

                            Dim oSCont As New FormSubmissionController
                            Dim ss As ArrayList = oSCont.SearchODSFormSubmission(pid, mid, search, start, pageSize, iSortCol_0, sSortDir_0, MaxRecords)

                            'prepara dati, cfr http://datatables.net/usage/server-side
                            'http://james.newtonking.com/projects/json-net.aspx
                            'http://james.newtonking.com/projects/json/help/
                            'http://www.csharp-examples.net/string-format-datetime/
                            Dim dc As New Newtonsoft.Json.Converters.IsoDateTimeConverter
                            dc.DateTimeFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern & " " _
                                & System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern
                            Dim out As String = JsonConvert.SerializeObject(ss, dc)
                            Dim pref As String = "{ ""sEcho"":" & Request("sEcho") & ", ""iTotalRecords"":" & ss.Count & ", ""iTotalDisplayRecords"":" & MaxRecords & ","
                            Response.Write(pref & " ""aaData"": " & out & " }")

                        Case Else
                            Response.Write("[{""Status"":""ERR_CMD_UNKNOWN""}]")
                    End Select

                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try


        End Sub
    End Class

End Namespace