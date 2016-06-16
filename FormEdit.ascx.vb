Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports ODS.DNN.Modules.Form.Business
Imports DotNetNuke
Imports System.Collections.Generic
Imports System.Reflection
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Framework.JavaScriptLibraries

Namespace ODS.DNN.Modules.Form

    Partial Class FormEdit
        Inherits Entities.Modules.PortalModuleBase

#Region "Private Members"
        Private FormItemID As Integer
        Private LocalizeForm As Boolean = False
        Private cultureCode As String = Nothing
        Private libName As String = "ODS.DNN.Modules.Form.FormEdit"
#End Region

#Region "Event Handlers"

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            If Not Page.IsPostBack Then
                JavaScript.RequestRegistration(CommonJs.jQuery)
                JavaScript.RequestRegistration(CommonJs.jQueryUI)
                JavaScript.RequestRegistration(CommonJs.DnnPlugins)
            End If
            LocalizeForm = CType(Settings("EnableLocalization"), Boolean)
            If LocalizeForm Then cultureCode = System.Threading.Thread.CurrentThread.CurrentCulture.ToString
        End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                Dim objCtlForm As New FormItemController
                Dim objForm As New FormItemInfo

                ' Determine FormItemID
                If Not (Request.Params("FormItemID") Is Nothing) Then
                    FormItemID = Int32.Parse(Request.Params("FormItemID"))

                    'DELETE action
                    If Request("action") = "delete" Then
                        objCtlForm.Delete(FormItemID)
                        Response.Redirect(NavigateURL()) ' back to form
                    End If

                    'MOVEDOWN
                    If Request("action") = "movedown" Then
                        objCtlForm.MoveDown(FormItemID)
                        Response.Redirect(NavigateURL()) ' back to form
                    End If

                    'MOVEUP
                    If Request("action") = "moveup" Then
                        objCtlForm.MoveUp(FormItemID)
                        Response.Redirect(NavigateURL()) ' back to form
                    End If

                Else
                    FormItemID = Null.NullInteger()
                    cmdUpdateContinue.Visible = False
                End If

                If Not Page.IsPostBack Then
                    'Set heltptext and hide controls
                    '<- skaper problemer å skjule rader tydligvis...

                    'cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem", LocalResourceFile) & "');")
                    If Not Null.IsNull(FormItemID) Then
                        objForm = objCtlForm.Get(FormItemID)
                        If Not objForm Is Nothing Then

                            lblCulture.Text = objForm.Culture

                            'Select correct formtype
                            Me.ddlFormTypes.Items(objForm.FormType - 1).Selected = True

                            '01.00.02: CustomClass
                            Me.txtCustomClass.Text = objForm.CSSClass
                            Me.txtFormLabelClass.Text = objForm.FormLabelClass
                            'Add form values
                            Me.tbFormLabel.Text = objForm.FormLabel
                            Me.tbFormValues.Text = objForm.FormValue
                            Me.tbSelectedValues.Text = objForm.FormSelectedValue
                            Me.cbRequired.Checked = Not objForm.Optional

                            ''01.00.01: Control Width
                            If Not objForm.Width = Null.NullInteger Then
                                Me.txtWidth.Text = objForm.Width
                            End If
                            '01.00.01: Control Height
                            If Not objForm.Height = Null.NullInteger Then
                                Me.txtHeight.Text = objForm.Height
                            End If

                            cmdDelete.Visible = True
                            lbMoveUp.Visible = True
                            lbMoveDown.Visible = True

                            If objForm.FormType = FormItem.TextBox Then
                                txtCustomRegex.Text = objForm.CustomRegex
                            End If

                            txtFormItemTitle.Text = objForm.FormItemTitle

                            chkAllowValueOverride.Checked = objForm.AllowValueOverride
                            chkAllowValueOverride.Text = Localization.GetString("chkAllowValueOverride", Me.LocalResourceFile) & "dnn_ctr" & Me.ModuleId & "_Form_ctl_" & FormItemID

                            Select Case objForm.FormType
                                Case FormItem.MultipleSelect
                                    '01.00.08 split view into columns
                                    'CustomData contains ddlMultipleSelectCol=nCols
                                    If objForm.CustomData <> String.Empty Then
                                        Dim rv() As String = objForm.CustomData.Split(";")
                                        Dim nv() As String = rv(0).Split("=")
                                        Select Case nv(0)
                                            Case "ddlMultipleSelectCol"
                                                'LoggerSource.Instance.GetLogger(libName).Debug(nv(0) & " = " & nv(1))
                                                Dim iCols As Integer = Integer.Parse(nv(1))
                                                ddlMultipleSelectCol.SelectedValue = iCols
                                        End Select
                                    End If

                                Case FormItem.DropDownList
                                    If objForm.FormValue.ToString.StartsWith("[SQL]") Then
                                        cbisDBQuery.Checked = True
                                        Me.tbFormValues.Text = objForm.FormValue.Substring(5)
                                    End If
                            End Select


                        Else ' security violation attempt to access item not related to this Module
                            Response.Redirect(NavigateURL(), True)
                        End If
                    Else
                        'adding new field
                        If LocalizeForm Then
                            lblCulture.Text = cultureCode
                        Else
                            lblCulture.Text = "Culture independent"
                        End If

                    End If


                    ChangeType()

                    If objForm.FormType = FormItem.FileUpload AndAlso objForm.FormValue <> String.Empty Then
                        ddlFilePath.SelectedValue = objForm.FormValue
                    End If
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click, cmdUpdateContinue.Click
            Try
                ' Only Update if the Entered Data is Valid
                If Page.IsValid = True Then
                    Dim objForm As New FormItemInfo

                    objForm = CType(CBO.InitializeObject(objForm, GetType(FormItemInfo)), FormItemInfo)

                    'bind text values to object
                    objForm.PortalID = MyBase.PortalId
                    objForm.FormItemID = FormItemID
                    objForm.ModuleID = ModuleId
                    objForm.FormLabel = Me.tbFormLabel.Text & ""
                    If ddlFormTypes.SelectedValue = FormItem.DropDownList Then
                        If cbisDBQuery.Checked Then
                            objForm.FormValue = "[SQL]" & Me.tbFormValues.Text & ""
                        Else
                            objForm.FormValue = Me.tbFormValues.Text & ""
                        End If
                    Else
                        objForm.FormValue = Me.tbFormValues.Text & ""
                    End If
                    objForm.FormSelectedValue = Me.tbSelectedValues.Text & ""
                    objForm.FormType = CInt(Me.ddlFormTypes.SelectedValue)
                    objForm.Optional = Not Me.cbRequired.Checked
                    objForm.AllowValueOverride = chkAllowValueOverride.Checked

                        'destination folder for FileUpload fields
                        If ddlFormTypes.SelectedValue = FormItem.FileUpload Then
                            objForm.FormValue = ddlFilePath.SelectedValue
                        ElseIf ddlFormTypes.SelectedValue = FormItem.TextBox Then
                            objForm.CustomRegex = txtCustomRegex.Text
                        End If

                        ''01.00.01: controls width and height
                        Try
                            objForm.Width = txtWidth.Text
                        Catch ex As Exception
                            objForm.Width = Null.NullInteger
                        End Try
                        Try
                            objForm.Height = txtHeight.Text
                        Catch ex As Exception
                            objForm.Height = Null.NullInteger
                        End Try

                        '01.00.02: custom css class for control
                        objForm.CSSClass = Me.txtCustomClass.Text

                        '01.00.05
                        objForm.FormItemTitle = txtFormItemTitle.Text
                        objForm.FormLabelClass = txtFormLabelClass.Text ' custom css class for label

                        '01.00.08 split multiselect in columns
                        'CustomData contains ddlMultipleSelectCol=nCols
                        Select Case objForm.FormType

                            Case FormItem.MultipleSelect
                                objForm.CustomData = "ddlMultipleSelectCol=" & ddlMultipleSelectCol.SelectedValue & ";"

                            Case Else
                                objForm.CustomData = Null.NullString
                        End Select

                        Dim objCtlForm As New FormItemController
                        If Null.IsNull(FormItemID) Then
                            objForm.Culture = cultureCode
                            objCtlForm.Add(objForm)
                        Else
                            'don't update culture code, issues with ifinity friendlyurl to be solved (gets default language) 01.00.05
                            'objForm.Culture = cultureCode
                            objCtlForm.Update(objForm)
                        End If

                        If sender Is cmdUpdateContinue Then
                            Response.Redirect(EditUrl("FormItemID", FormItemID), True)
                        End If

                        ' Redirect back to the portal home page
                        Response.Redirect(NavigateURL(), True)
                    End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click
            Try
                If Not Null.IsNull(FormItemID) Then
                    Dim objCtlForm As New FormItemController
                    objCtlForm.Delete(FormItemID)
                End If

                ' Redirect back to the portal home page
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
#End Region

        Private Sub ddlFormTypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFormTypes.SelectedIndexChanged
            ChangeType()
        End Sub


        Private Sub lbMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbMoveUp.Click
            Try
                If Not Null.IsNull(FormItemID) Then
                    Dim objCtlForm As New FormItemController
                    objCtlForm.MoveUp(FormItemID)
                End If

                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub lbMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbMoveDown.Click
            Try
                If Not Null.IsNull(FormItemID) Then
                    Dim objCtlForm As New FormItemController
                    objCtlForm.MoveDown(FormItemID)
                End If

                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        'hides irrellvant rows. Shows correct helptext
        Sub ChangeType()
            Me.trFormLabel.Visible = True
            Me.trFormValues.Visible = False
            Me.trSelectedValues.Visible = True
            Me.trRequired.Visible = True
            trFilePath.Visible = False '01.00.03: default is hidden
            trCustomRegex.Visible = False
            trAllowValueOverride.Visible = False
            Me.trMultipleSelectCol.Visible = False
            Me.trisDBQuery.Visible = False

            Select Case CInt(Me.ddlFormTypes.SelectedValue)
                'Checkbox
                Case FormItem.Checkbox
                    Me.trSelectedValues.Visible = False
                    Me.trRequired.Visible = True '01.00.03
                    lblHelp.Text = Localization.GetString("HelpCheckbox.Text", LocalResourceFile)
                    trWidth.Visible = False
                    trHeight.Visible = False
                    trAllowValueOverride.Visible = True
                    'DropDownList
                Case FormItem.DropDownList
                    lblHelp.Text = Localization.GetString("HelpDropDownList.Text", LocalResourceFile)
                    trWidth.Visible = False
                    trHeight.Visible = False
                    Me.trFormValues.Visible = True
                    trAllowValueOverride.Visible = True
                    Me.trisDBQuery.Visible = True
                'Label
                Case FormItem.Label
                    Me.trRequired.Visible = False
                    Me.trFormLabel.Visible = True
                    Me.trSelectedValues.Visible = False
                    lblHelp.Text = Localization.GetString("HelpLabel.Text", LocalResourceFile)
                    trWidth.Visible = False
                    trHeight.Visible = False
                    Me.trFormValues.Visible = True
                    'MultipleSelect
                Case FormItem.MultipleSelect
                    lblHelp.Text = Localization.GetString("HelpMultipleSelect.Text", LocalResourceFile)
                    trWidth.Visible = False
                    trHeight.Visible = False
                    Me.trFormValues.Visible = True
                    trAllowValueOverride.Visible = True
                    Me.trMultipleSelectCol.Visible = True
                    'TextArea
                Case FormItem.TextArea
                    Me.trSelectedValues.Visible = False
                    lblHelp.Text = Localization.GetString("HelpTextArea.Text", LocalResourceFile)
                    trWidth.Visible = True
                    trHeight.Visible = True
                    Me.trFormValues.Visible = True
                Case FormItem.TextBox
                    Me.trSelectedValues.Visible = False
                    lblHelp.Text = Localization.GetString("HelpTextBox.Text", LocalResourceFile)
                    trWidth.Visible = True
                    trHeight.Visible = False
                    trCustomRegex.Visible = True
                    trAllowValueOverride.Visible = True
                    Me.trFormValues.Visible = True
                    'RadioButtonList
                Case FormItem.RadioButtonList
                    lblHelp.Text = Localization.GetString("HelpRadioButtonList.Text", LocalResourceFile)
                    trWidth.Visible = False
                    trHeight.Visible = False
                    Me.trFormValues.Visible = True
                    trAllowValueOverride.Visible = True
                    'HTML Editor
                Case FormItem.DNNRichTextEditControl
                    Me.trSelectedValues.Visible = False
                    lblHelp.Text = Localization.GetString("HelpDNNRichTextEditControl.Text", LocalResourceFile)
                    trWidth.Visible = True
                    trHeight.Visible = True
                    Me.trFormValues.Visible = True
                Case FormItem.FileUpload
                    Me.trSelectedValues.Visible = False
                    trFilePath.Visible = True
                    lblHelp.Text = Localization.GetString("HelpFileUpload.Text", LocalResourceFile)
                    trWidth.Visible = False
                    trHeight.Visible = False
                    'popola ddlFilePath
                    ddlFilePath.Items.Clear() '01.00.02
                    Dim folders As System.Collections.Generic.List(Of IFolderInfo) = FolderManager.Instance().GetFolders(UserController.GetCurrentUserInfo, "")  ' FileSystemUtils.GetFoldersByUser(PortalId, True, True, "")
                    For Each folder As IFolderInfo In folders
                        Dim FolderItem As New ListItem
                        If folder.FolderPath = Null.NullString Then
                            FolderItem.Text = "PORTAL ROOT"
                        Else
                            FolderItem.Text = folder.FolderPath
                        End If
                        FolderItem.Value = folder.FolderID
                        ddlFilePath.Items.Add(FolderItem)
                    Next

                Case FormItem.HiddenField
                    Me.trSelectedValues.Visible = False
                    lblHelp.Text = Localization.GetString("HiddenField.Text", LocalResourceFile)
                    trWidth.Visible = False
                    trHeight.Visible = False
                    trRequired.Visible = False
                    trLabelCSS.Visible = False
                    trControlCSS.Visible = False
                    Me.trFormValues.Visible = True
                    trFieldTitle.Visible = False
            End Select
        End Sub
    End Class



End Namespace
