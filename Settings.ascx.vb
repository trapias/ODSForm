Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports ODS.DNN.Modules.Form.Business
Imports System.Collections.Generic
Imports System.Reflection

Namespace ODS.DNN.Modules.Form

    Partial Class Settings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Controls"
#End Region

        Public Overrides Sub LoadSettings()
            Try
                If Not Page.IsPostBack Then
                    ' Load settings from TabModuleSettings: specific to this instance
                    'Dim setting1 As String = CType(TabModuleSettings("settingname1"), String)

                    Dim locale As String = ddlLocales.SelectedValue
                    If locale = "" Then locale = System.Threading.Thread.CurrentThread.CurrentCulture.ToString
                    If CType(Settings("EnableLocalization"), Boolean) = False Then locale = "" ' culture independent

                    ' Load settings from ModuleSettings: general for all instances
                    tbEmail.Text = CType(Settings("tbEmail"), String)
                    cbDatabase.Checked = CType(Settings("cbDatabase"), Boolean)
                    cbEmail.Checked = CType(Settings("cbEmail"), Boolean)
                    '01.00.06 suppressed table, use css!
                    'tbWidth.Text = CType(Settings("tbWidth"), String)
                    plClass.Text = CType(Settings("plClass"), String)

                    txtButtonsClass.Text = CType(Settings("ButtonsClass"), String)
                    chkCaptcha.Checked = CType(Settings("Captcha"), Boolean)
                    '01.00.02
                    ddlCaptchaMode.SelectedValue = CType(Settings("CaptchaMode"), String)
                    ddlCaptchaLength.SelectedValue = CType(Settings("CaptchaLength"), String)

                    '01.00.06: suppressed, now use dnnFormRequired css class
                    'imgRequiredFields.FileID = CType(Settings("imgRequiredFields"), String)

                    txtCSSLabels.Text = CType(Settings("CSSLabels"), String)
                    txtCSSCaptcha.Text = CType(Settings("CSSCaptcha"), String)
                    chkNumbersOnly.Checked = CType(Settings("CaptchaNumbers"), Boolean)

                    'chkContactUsers 01.00.05
                    chkContactUsers.Checked = CType(Settings("AllowContactUsers"), Boolean)
                    chkEnableLocalization.Checked = CType(Settings("EnableLocalization"), Boolean)
                    chkMailto.Checked = CType(Settings("AllowMailto"), Boolean)

                    'ConfirmMode
                    Dim ConfirmMode As String = CType(Settings("ConfirmMode"), String)
                    ddlSubmConfMode.SelectedValue = ConfirmMode
                    If ConfirmMode = "1" Then
                        trConfirmMessageTitle.Visible = False
                        trConfirmMessage.Visible = False
                        trConfirmURL.Visible = True
                        tbURL.Url = CType(Settings("tbURL-" & locale), String)
                    Else
                        trConfirmMessageTitle.Visible = True
                        trConfirmMessage.Visible = True
                        trConfirmURL.Visible = False
                        txtConfirmTitle.Text = CType(Settings("ConfirmTitle-" & locale), String)
                        txtConfirmMessage.Text = CType(Settings("ConfirmMessage-" & locale), String)
                    End If

                    chkValSum.Checked = CType(Settings("chkValSum"), Boolean)
                    posValSum.SelectedValue = CType(Settings("posValSum"), String)
                    chkHideReset.Checked = CType(Settings("chkHideReset"), Boolean)

                    'txtSubmitText
                    txtSubmitText.Text = CType(Settings("txtSubmitText-" & locale), String)
                    'txtResetText
                    txtResetText.Text = CType(Settings("txtResetText-" & locale), String)

                    'EmailSender
                    txtEmailSender.Text = CType(Settings("EmailSender"), String)

                    'CustomDomains
                    txtCustomDomains.Text = CType(Settings("CustomDomains"), String)

                    'WebHook
                    chkWebHook.Checked = CType(Settings("enableWebHook"), Boolean)
                    txtWHURL.Text = CType(Settings("WHURL-" & locale), String)

                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New Entities.Modules.ModuleController

                Dim locale As String = ddlLocales.SelectedValue
                If locale = "" Then locale = System.Threading.Thread.CurrentThread.CurrentCulture.ToString
                If CType(Settings("EnableLocalization"), Boolean) = False Then locale = "" ' culture independent

                ' Update ModuleSettings
                objModules.UpdateModuleSetting(ModuleId, "tbEmail", tbEmail.Text)

                'EmailSender
                objModules.UpdateModuleSetting(ModuleId, "EmailSender", txtEmailSender.Text)

                'CustomDomains
                objModules.UpdateModuleSetting(ModuleId, "CustomDomains", txtCustomDomains.Text)

                objModules.UpdateModuleSetting(ModuleId, "plClass", plClass.Text)
                objModules.UpdateModuleSetting(ModuleId, "cbEmail", CStr(cbEmail.Checked))
                objModules.UpdateModuleSetting(ModuleId, "cbDatabase", CStr(cbDatabase.Checked))
                'Captcha
                objModules.UpdateModuleSetting(ModuleId, "Captcha", CStr(chkCaptcha.Checked))
                '01.00.02
                objModules.UpdateModuleSetting(ModuleId, "CaptchaMode", CStr(ddlCaptchaMode.SelectedValue))
                objModules.UpdateModuleSetting(ModuleId, "CaptchaLength", CStr(ddlCaptchaLength.SelectedValue))
                'imgRequiredFields
                '01.00.06: suppressed, now use dnnFormRequired css class
                'objModules.UpdateModuleSetting(ModuleId, "imgRequiredFields", imgRequiredFields.FileID)

                '01.00.06: suppressed default classes
                Dim sButtonsClass As String = "" ' "medium FormButton orange"
                If txtButtonsClass.Text.Trim <> "" Then
                    sButtonsClass = txtButtonsClass.Text
                End If
                objModules.UpdateModuleSetting(ModuleId, "ButtonsClass", sButtonsClass)

                'custom CSS classes
                Dim sCSSLabels As String = txtCSSLabels.Text
                '01.00.06: suppressed default classes
                'If sCSSLabels = "" Then sCSSLabels = "FormLabel" 'default
                objModules.UpdateModuleSetting(ModuleId, "CSSLabels", sCSSLabels)

                'CSSCaptcha
                Dim sCSSCaptcha As String = txtCSSCaptcha.Text
                '01.00.06: suppressed default classes
                'If sCSSCaptcha = "" Then sCSSCaptcha = "NormalRed" 'default
                objModules.UpdateModuleSetting(ModuleId, "CSSCaptcha", sCSSCaptcha)

                'use only numbers as captcha chars?
                objModules.UpdateModuleSetting(ModuleId, "CaptchaNumbers", CStr(chkNumbersOnly.Checked))
                'allow to contact users?
                objModules.UpdateModuleSetting(ModuleId, "AllowContactUsers", CStr(chkContactUsers.Checked))
                'mailto?
                'mailto is limited by checking HTTP_REFERER and email domain
                objModules.UpdateModuleSetting(ModuleId, "AllowMailto", CStr(chkMailto.Checked))

                'titolo modulo
                objModules.UpdateModuleSetting(ModuleId, "ModuleTitle-" & locale, txtModuleTitle.Text)

                'chkEnableLocalization
                objModules.UpdateModuleSetting(ModuleId, "EnableLocalization", CBool(chkEnableLocalization.Checked))

                'confirmation mode 
                objModules.UpdateModuleSetting(ModuleId, "ConfirmMode", CStr(ddlSubmConfMode.SelectedValue))
                objModules.UpdateModuleSetting(ModuleId, "ConfirmTitle-" & locale, txtConfirmTitle.Text)
                objModules.UpdateModuleSetting(ModuleId, "ConfirmMessage-" & locale, txtConfirmMessage.Text)
                objModules.UpdateModuleSetting(ModuleId, "tbURL-" & locale, tbURL.Url)

                'chkValSum
                objModules.UpdateModuleSetting(ModuleId, "chkValSum", CStr(chkValSum.Checked))
                'posValSum
                objModules.UpdateModuleSetting(ModuleId, "posValSum", CStr(posValSum.SelectedValue))
                'chkHideReset
                objModules.UpdateModuleSetting(ModuleId, "chkHideReset", CStr(chkHideReset.Checked))

                'txtSubmitText
                objModules.UpdateModuleSetting(ModuleId, "txtSubmitText-" & locale, txtSubmitText.Text)
                'txtResetText
                objModules.UpdateModuleSetting(ModuleId, "txtResetText-" & locale, txtResetText.Text)

                'WebHook
                objModules.UpdateModuleSetting(ModuleId, "enableWebHook", CStr(chkWebHook.Checked))
                objModules.UpdateModuleSetting(ModuleId, "WHURL-" & locale, txtWHURL.Text)

                chkWebHook.Checked = CType(Settings("enableWebHook"), Boolean)
                txtWHURL.Text = CType(Settings("WHURL-" & locale), String)


                ' Redirect back to the portal home page
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                CaricaLocales()
                Dim locale As String = ddlLocales.SelectedValue
                If locale = "" Then locale = System.Threading.Thread.CurrentThread.CurrentCulture.ToString
                If CType(Settings("EnableLocalization"), Boolean) = False Then locale = "" ' culture independent
                txtModuleTitle.Text = Settings("ModuleTitle-" & locale)
            End If
        End Sub

        Private Sub CaricaLocales()

            ddlLocales.Items.Clear()

            Dim ll As Dictionary(Of String, Locale) = DotNetNuke.Services.Localization.LocaleController.Instance().GetLocales(Me.PortalId)
            For Each l As Locale In ll.Values
                Dim li As New ListItem
                li.Value = l.Code
                li.Text = l.Text

                If l.Code = System.Threading.Thread.CurrentThread.CurrentCulture.ToString Then li.Selected = True

                ddlLocales.Items.Add(li)
            Next

        End Sub

        Protected Sub ddlLocales_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlLocales.SelectedIndexChanged
            Dim locale As String = ddlLocales.SelectedValue
            If locale = "" Then locale = System.Threading.Thread.CurrentThread.CurrentCulture.ToString
            If CType(Settings("EnableLocalization"), Boolean) = False Then locale = "" ' culture independent

            txtModuleTitle.Text = Settings("ModuleTitle-" & locale)
            txtConfirmTitle.Text = CType(Settings("ConfirmTitle-" & locale), String)
            txtConfirmMessage.Text = CType(Settings("ConfirmMessage-" & locale), String)
            txtSubmitText.Text = Settings("txtSubmitText-" & locale)
            txtResetText.Text = Settings("txtResetText-" & locale)
        End Sub

        Protected Sub ddlSubmConfMode_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlSubmConfMode.SelectedIndexChanged

            If ddlSubmConfMode.SelectedValue = "1" Then
                trConfirmMessageTitle.Visible = False
                trConfirmMessage.Visible = False
                trConfirmURL.Visible = True
            Else
                trConfirmMessageTitle.Visible = True
                trConfirmMessage.Visible = True
                trConfirmURL.Visible = False
            End If

        End Sub
    End Class

End Namespace
