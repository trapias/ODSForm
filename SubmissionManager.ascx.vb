Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports ODS.DNN.Modules.Form.Business
Imports DotNetNuke.Framework.JavaScriptLibraries

Namespace ODS.DNN.Modules.Form

    Partial Class SubmissionManager
        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    JavaScript.RequestRegistration(CommonJs.jQuery)
                    JavaScript.RequestRegistration(CommonJs.jQueryUI)
                    JavaScript.RequestRegistration(CommonJs.DnnPlugins)
                End If


            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try

        End Sub
    End Class

End Namespace

