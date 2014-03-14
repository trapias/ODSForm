Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports ODS.DNN.Modules.Form.Business
Imports DotNetNuke.Instrumentation

Namespace ODS.DNN.Modules.Form

    Partial Class FormView
        Inherits Entities.Modules.PortalModuleBase

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If Not Page.IsPostBack Then
                Dim oSCont As New FormSubmissionController
                Me.ddlSubmissions.DataSource = oSCont.ListODSFormSubmission(MyBase.PortalId, MyBase.ModuleId)
                ddlSubmissions.DataTextField = "SubmissionDate"
                ddlSubmissions.DataValueField = "FormSubmissionID"
                Me.ddlSubmissions.DataBind()

                If ddlSubmissions.Items.Count > 0 Then
                    Me.lblSubmission.Text = GetSubmission(CInt(Me.ddlSubmissions.SelectedValue))
                Else
                    Me.cmdDelete.Visible = False
                    Me.ddlSubmissions.Visible = False
                    lblSubmission.Text = Localization.GetString("NoItems.Text", LocalResourceFile)
                End If
            End If
        End Sub

        Private Function GetSubmission(ByVal ID As Integer) As String
            Dim oInfo As FormSubmissionInfo
            Dim oCtl As New FormSubmissionController
            oInfo = oCtl.GetODSFormSubmission(ID)
            Return "<pre>" & oInfo.Submission & "</pre>"
        End Function

        Private Sub ddlSubmissions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSubmissions.SelectedIndexChanged
            Me.lblSubmission.Text = GetSubmission(CInt(Me.ddlSubmissions.SelectedValue))
        End Sub

        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
            Dim oCtl As New FormSubmissionController
            oCtl.DeleteODSFormSubmission(CInt(Me.ddlSubmissions.SelectedValue))
            Response.Redirect(Request.RawUrl)
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
    End Class
End Namespace
