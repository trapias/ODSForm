Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports ODS.DNN.Modules.Form.Business
Imports DotNetNuke
Imports DotNetNuke.Services.FileSystem
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Xml
Imports DotNetNuke.Instrumentation
Imports System.Text.RegularExpressions
Imports DotNetNuke.Framework.JavaScriptLibraries
Imports System.Net

Namespace ODS.DNN.Modules.Form

    ''' <summary>
    ''' 01.00.08
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class Form
        Inherits Entities.Modules.PortalModuleBase
        Implements Entities.Modules.IActionable

        Private hasValidators As Boolean = False
        Private LocalizeForm As Boolean = False
        Private cultureCode As String = Nothing
        Private libName As String = "ODS.DNN.Modules.Form.Form"

#Region "Event Handlers"

        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

            'If Not Page.IsPostBack Then
            JavaScript.RequestRegistration(CommonJs.jQuery)
                JavaScript.RequestRegistration(CommonJs.jQueryUI)
                JavaScript.RequestRegistration(CommonJs.DnnPlugins)
            'End If
            
                LocalizeForm = CType(Settings("EnableLocalization"), Boolean)
                If LocalizeForm Then cultureCode = System.Threading.Thread.CurrentThread.CurrentCulture.ToString

                Dim AllowContactUsers As Boolean = CType(Settings("AllowContactUsers"), Boolean)
                If AllowContactUsers AndAlso Request("userid") <> "" Then

                    Try
                        Dim u As UserInfo = UserController.GetUserById(Me.PortalId, Request("userid"))
                        If Not u Is Nothing Then

                            Me.ModuleConfiguration.ModuleTitle = Localization.GetString("ContactUser", LocalResourceFile) & " " & u.DisplayName

                        End If
                    Catch ex As Exception
                    End Try
                Else

                    Dim localizedTitle As String = Settings("ModuleTitle-" & cultureCode)
                    If localizedTitle <> "" Then
                        Me.ModuleConfiguration.ModuleTitle = localizedTitle
                    End If

                End If

                'AllowMailto
                Dim AllowMailto As Boolean = CType(Settings("AllowMailto"), Boolean)
                If AllowMailto AndAlso ValidateMailto() Then
                    Me.ModuleConfiguration.ModuleTitle = Localization.GetString("ContactUser", LocalResourceFile) & " " & Request("mailto")
                End If

        End Sub

        Private Function ValidateMailto() As Boolean
            Dim isvalid As Boolean = False
            Dim m As String = Request("mailto")
            If m Is Nothing Or m Is String.Empty Then
                Return False
            Else
                'mailto is limited by checking HTTP_REFERER and email domain

                'validate HTTP_REFERER
                Dim ref As String = Request.ServerVariables("HTTP_REFERER")
                If ref Is Nothing Then ref = ""
                If ref.ToLower.StartsWith("https://") Then ref = ref.Substring(8)
                If ref.ToLower.StartsWith("http://") Then ref = ref.Substring(7)

                Dim pa As Dictionary(Of String, PortalAliasInfo) = DotNetNuke.Entities.Portals.Internal.TestablePortalAliasController.Instance.GetPortalAliases()
                If Not isvalid Then
                    'validate against portal aliases
                    For Each sAlias As PortalAliasInfo In pa.Values
                        If ref.ToLower.StartsWith(sAlias.HTTPAlias) Then
                            isvalid = True
                        End If
                    Next
                End If

                If Not isvalid Then
                    MyLog("INVALID HTTP_REFERER: " & ref)
                    Return False
                End If

                'validate mailto address
                If Not m.Contains("@") Then
                    MyLog("INVALID mailto address: " & m)
                    Return False
                End If
                Dim emailDomain As String, p As Integer
                p = m.IndexOf("@")
                emailDomain = m.Substring(p + 1)

                'validate against custom domain list, if given
                If CType(Settings("CustomDomains"), String) <> "" Then
                    For Each domName As String In CType(Settings("CustomDomains"), String).Split(";")

                        If domName.ToLower.EndsWith(emailDomain.ToLower) Then
                            Return True
                        ElseIf domName.ToLower.StartsWith("www." & emailDomain.ToLower) Then
                            'MyLog("VALID HTTPAlias: " & sAlias.HTTPAlias & " vs " & emailDomain)
                            Return True
                        Else
                            MyLog("INVALID Custom Domain: " & domName & " vs " & emailDomain)
                            isvalid = False
                        End If

                    Next
                End If

                For Each sAlias As PortalAliasInfo In pa.Values
                    If sAlias.HTTPAlias.ToLower.EndsWith(emailDomain.ToLower) Then
                        Return True
                    ElseIf sAlias.HTTPAlias.ToLower.StartsWith("www." & emailDomain.ToLower) Then
                        'MyLog("VALID HTTPAlias: " & sAlias.HTTPAlias & " vs " & emailDomain)
                        Return True
                    Else
                        MyLog("INVALID HTTPAlias: " & sAlias.HTTPAlias & " vs " & emailDomain)
                        isvalid = False
                    End If
                Next
                End If

                Return isvalid

        End Function

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
              
                ' If Not Page.IsPostBack Then
              
                '01.00.06 set form class plClass
                Dim formDIV As New HtmlGenericControl("div")
                formDIV.Attributes.Add("class", "dnnForm")
                Dim plClass As String = CStr(Settings("plClass"))
                If plClass <> "" Then
                    formDIV.Attributes("class") &= " " & plClass
                End If

                plcODS.Controls.Add(formDIV)

                Dim aSubmit As New LinkButton
                aSubmit.ValidationGroup = "ods" & Me.ModuleId
                aSubmit.CausesValidation = True

                Dim aReset As New LinkButton
                AddHandler aSubmit.Click, AddressOf Me.cmdSubmit_Click
                AddHandler aReset.Click, AddressOf Me.cmdReset_Click

                Dim sClass As String = CType(Settings("ButtonsClass"), String)
                If sClass Is Nothing Then sClass = String.Empty

                If sClass = String.Empty Then
                    aSubmit.CssClass = "dnnPrimaryAction"
                    aReset.CssClass = "dnnSecondaryAction"
                Else
                    aSubmit.CssClass = sClass
                    aReset.CssClass = sClass
                End If

                aSubmit.Text = Localization.GetString("cmdSubmit", LocalResourceFile)
                aReset.Text = Localization.GetString("cmdReset", LocalResourceFile)

                If Not cultureCode Is Nothing Then
                    'form localization enabled
                    If Settings("txtSubmitText-" & cultureCode) <> "" Then
                        aSubmit.Text = Settings("txtSubmitText-" & cultureCode)
                    End If

                    If Settings("txtResetText-" & cultureCode) <> "" Then
                        aReset.Text = Settings("txtResetText-" & cultureCode)
                    End If
                Else
                    'localization not enabled
                    If Settings("txtSubmitText-") <> "" Then
                        aSubmit.Text = Settings("txtSubmitText-")
                    End If

                    If Settings("txtResetText-") <> "" Then
                        aReset.Text = Settings("txtResetText-")
                    End If

                End If

                'load/reload controls 
                Dim fieldset As New HtmlGenericControl("fieldset")
                formDIV.Controls.Add(fieldset)

                Dim o As Object
                Dim oInfo As FormItemInfo

                'Get list of formcontrols
                Dim oController As New FormItemController
                Dim al As ArrayList = oController.List(MyBase.PortalId, MyBase.ModuleId, cultureCode)

                    'hide module if not fields are configured
                If al.Count = 0 AndAlso Not IsEditable Then
                    'plcODS.Controls.Remove(formDIV)
                    Me.Visible = False
                    Exit Sub
                End If

                'Iterate formcontols
                For Each o In al
                    oInfo = CType(o, FormItemInfo)

                    Dim div As New HtmlGenericControl("div")
                    div.Attributes.Add("class", "dnnFormItem" & IIf(oInfo.FormLabelClass <> "", " " & oInfo.FormLabelClass, ""))
                    fieldset.Controls.Add(div)

                    Dim divLabel As New HtmlGenericControl("span")
                    divLabel.Attributes("class") = "dnnFormLabel" & IIf(oInfo.FormLabelClass <> "", " " & oInfo.FormLabelClass, "")
                    div.Controls.Add(divLabel)

                    'EDIT field link
                    Dim lblEdit As New Label
                    ' lblEdit.CssClass = "dnnFormLabel"
                    lblEdit.Text = "<a title='Edit' href=" & MyBase.EditUrl("FormItemID", CStr(oInfo.FormItemID)) & "><img src=""" & ResolveUrl("~/icons/sigma/Edit_16X16_Standard.png") & """ border=0></a>"

                    'DELETE field link
                    Dim pars() As String = {"FormItemID=" & CStr(oInfo.FormItemID), "mid=" & Me.ModuleId, "action=delete"}
                    Dim delURL As String = NavigateURL("Edit", pars)
                    Dim lblDel As New Label
                    lblDel.Text = "<a title='Delete' class='confirm' href=" & delURL & "><img src=""" & ResolveUrl("~/icons/sigma/Delete_16X16_Standard.png") & """ border=0></a>"

                    'MOVEUP field link
                    Dim parsUp() As String = {"FormItemID=" & CStr(oInfo.FormItemID), "mid=" & Me.ModuleId, "action=moveup"}
                    Dim upURL As String = NavigateURL("Edit", parsUp)
                    Dim lblUp As New Label
                    lblUp.Text = "<a title='Move UP' href=" & upURL & "><img src=""" & ResolveUrl("~/icons/sigma/Up_16X16_Standard.png") & """ border=0></a>"

                    'MOVEDOWN field link
                    Dim parsDn() As String = {"FormItemID=" & CStr(oInfo.FormItemID), "mid=" & Me.ModuleId, "action=movedown"}
                    Dim dnURL As String = NavigateURL("Edit", parsDn)
                    Dim lblDn As New Label
                    lblDn.Text = "<a title='Move DOWN' href=" & dnURL & "><img src=""" & ResolveUrl("~/icons/sigma/Dn_16X16_Standard.png") & """ border=0></a>"

                    'If oInfo.FormType = FormItem.Label Then

                    '    'div.Attributes("class") &= " dnnTooltip"

                    '    If IsEditable Then
                    '        'td2.Controls.AddAt(0, lblEdit)
                    '        divLabel.Controls.AddAt(0, lblEdit)
                    '        divLabel.Controls.AddAt(1, lblDel)
                    '        divLabel.Controls.AddAt(2, lblUp)
                    '        divLabel.Controls.AddAt(3, lblDn)
                    '    End If
                    'Else

                    'div.Attributes("class") &= " dnnTooltip"

                    If IsEditable Then
                        divLabel.Controls.Add(lblEdit)
                        divLabel.Controls.Add(lblDel)
                        divLabel.Controls.Add(lblUp)
                        divLabel.Controls.Add(lblDn)
                    End If

                    'create label
                    Dim lbl As New Label
                        '01.00.02: custom css class for labels
                        Dim sCustomLabelClass As String = oInfo.FormLabelClass 'CType(Settings("CSSLabels"), String)
                        If sCustomLabelClass <> String.Empty Then
                            'label with custom css class
                            'lbl.CssClass = "dnnFormLabel " & sCustomLabelClass
                            lbl.CssClass = sCustomLabelClass
                            lbl.Text = oInfo.FormLabel
                        Else
                        'lbl.CssClass = "dnnFormLabel"
                        'default: use resx
                        '<span class=FormLabel>[FormLabel]:</span>
                        If oInfo.FormLabel <> String.Empty Then
                            lbl.Text = Replace(Localization.GetString("FormLabel.Text", LocalResourceFile), "[FormLabel]", oInfo.FormLabel)
                        End If

                    End If

                        'tooltip
                        If oInfo.FormItemTitle.Trim <> "" Then
                            lbl.ToolTip = oInfo.FormItemTitle
                        End If

                        If oInfo.FormType = FormItem.HiddenField Then
                            If IsEditable Then
                                divLabel.Controls.Add(lbl)
                            Else
                                div.Controls.Remove(divLabel)
                            End If
                        Else
                            divLabel.Controls.Add(lbl)
                        End If

                        ' End If

                        'DNN token-replace parser
                        'cfr http://www.dnnsoftware.com/wiki/Page/Tokens
                        'es: [Tab:TabName]
                        Dim dnnsafetokenreplace As New Regex("(\[([^: ]*):([^:/ ]*)\])", RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.Compiled)
                    Dim str As New MatchEvaluator(AddressOf DNNTokenReplace)

                    'Create controls by type
                    Select Case oInfo.FormType

                        'TextBox
                        Case FormItem.TextBox
                            Dim tb As TextBox = New TextBox
                            'tb.Text = oInfo.FormValue & ""
                            tb.Text = dnnsafetokenreplace.Replace(oInfo.FormValue & "", str)
                            tb.ID = "ctl_" & oInfo.FormItemID
                            '01.00.02: custom css class for fields
                            tb.CssClass = IIf(oInfo.CSSClass = String.Empty, "FormTextBox", oInfo.CSSClass)
                            If Not oInfo.Width = Null.NullInteger Then
                                tb.Width = oInfo.Width
                                'Else
                                '   tb.Width = 40 'default Width
                            End If
                            'td2.Controls.Add(tb)
                            div.Controls.Add(tb)
                            If oInfo.Optional = False Then AddRequiredFieldValidator(oInfo, tb, div)
                            'custom validator?
                            If oInfo.CustomRegex.Trim <> "" Then
                                AddRegexFieldValidator(oInfo, tb, div, oInfo.CustomRegex.Trim)
                            End If
                            If oInfo.FormItemTitle.Trim <> "" Then
                                tb.ToolTip = oInfo.FormItemTitle.Trim
                            End If
                            'placeholder 01.00.06
                            tb.Attributes.Add("placeholder", oInfo.FormItemTitle)

                            If oInfo.AllowValueOverride = True Then
                                Dim qsparam As String = Request("dnn_ctr" & Me.ModuleId & "_Form_ctl_" & oInfo.FormItemID)
                                If Not qsparam Is Nothing Then
                                    If qsparam <> "" Then
                                        tb.Text = Request("dnn_ctr" & Me.ModuleId & "_Form_ctl_" & oInfo.FormItemID)
                                    End If
                                End If
                            End If

                            'TextArea
                        Case FormItem.TextArea
                            Dim tb As TextBox = New TextBox
                            'tb.Text = oInfo.FormValue & ""
                            tb.Text = dnnsafetokenreplace.Replace(oInfo.FormValue & "", str)

                            tb.TextMode = TextBoxMode.MultiLine
                            If Not oInfo.Width = Null.NullInteger Then
                                tb.Columns = oInfo.Width
                            Else
                                tb.Columns = 40 'default columns
                            End If
                            If Not oInfo.Height = Null.NullInteger Then
                                tb.Rows = oInfo.Height
                            Else
                                tb.Rows = 10 'default rows
                            End If
                            tb.ID = "ctl_" & oInfo.FormItemID
                            tb.CssClass = IIf(oInfo.CSSClass = String.Empty, "FormTextArea", oInfo.CSSClass)
                            'td2.Controls.Add(tb)
                            div.Controls.Add(tb)

                            If oInfo.Optional = False Then AddRequiredFieldValidator(oInfo, tb, div)
                            If oInfo.FormItemTitle.Trim <> "" Then
                                tb.ToolTip = oInfo.FormItemTitle.Trim
                            End If
                            'placeholder 01.00.06
                            tb.Attributes.Add("placeholder", oInfo.FormItemTitle)

                            'Label
                        Case FormItem.Label
                            Dim lblVal As Label = New Label
                            'lbl.Text = oInfo.FormLabel & ""
                            lblVal.Text = dnnsafetokenreplace.Replace(oInfo.FormValue & "", str)
                            lblVal.ID = "ctl_" & oInfo.FormItemID
                            'lbl.CssClass = "dnnFormLabel"
                            lblVal.CssClass &= IIf(oInfo.CSSClass = String.Empty, "", " " & oInfo.CSSClass)
                            'td2.Controls.Add(lbl)
                            div.Controls.Add(lblVal)
                            If oInfo.FormItemTitle.Trim <> "" Then
                                lblVal.ToolTip = oInfo.FormItemTitle.Trim
                            End If

                            'DropDownList
                        Case FormItem.DropDownList
                            Dim ddl As DropDownList = New DropDownList
                            '1.00.10 allow db queries
                            If oInfo.FormValue.ToString.StartsWith("[SQL]") Then
                                Dim conn As New SqlConnection(DotNetNuke.Common.Utilities.Config.GetConnectionString())
                                Try
                                    conn.Open()
                                    Dim cmd As SqlCommand = conn.CreateCommand
                                    cmd.CommandTimeout = 60
                                    cmd.CommandText = oInfo.FormValue.ToString.Substring(5)
                                    If Not cmd.CommandText Is String.Empty Then

                                        Dim dr As SqlDataReader = cmd.ExecuteReader
                                        Dim dt As DataTable = New DataTable
                                        dt.Columns.Add("ID")
                                        dt.Columns.Add("VALUE")
                                        Dim emptyRow As DataRow = dt.NewRow
                                        dt.Rows.Add(emptyRow)
                                        Do While dr.Read
                                            Dim r As DataRow = dt.NewRow
                                            If TypeOf (dr(0)) Is DateTime Then
                                                r(0) = dr(0).ToShortDateString
                                            Else
                                                r(0) = dr(0)
                                            End If
                                            If TypeOf (dr(1)) Is DateTime Then
                                                r(1) = dr(1).ToShortDateString
                                            Else
                                                r(1) = dr(1)
                                            End If
                                            dt.Rows.Add(r)
                                        Loop
                                        ddl.DataSource = dt
                                        ddl.DataValueField = "ID"
                                        ddl.DataTextField = "VALUE"
                                        ddl.DataBind()
                                    End If
                                Catch ex As Exception
                                    LoggerSource.Instance.GetLogger(libName).Error(ex.Message)
                                    Dim lblErr As Label = New Label
                                    lblErr.Text = "ERROR IN QUERY: " & ex.Message
                                    lblErr.ForeColor = Color.Red
                                    div.Controls.Add(lblErr)
                                Finally
                                    conn.Close()
                                End Try

                                '1.00.11 cascading dropdowns
                                If oInfo.CustomData <> String.Empty Then
                                    ddl.AutoPostBack = True
                                    AddHandler ddl.SelectedIndexChanged, AddressOf Me.ddlCascading_SelectedIndexChanged
                                End If

                            Else
                                Dim list As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))
                                Dim oD As Object
                                Dim s1() As String = Split(oInfo.FormValue & "", ";")
                                oD = s1
                                For Each s As String In s1
                                    If s.Contains("=") Then
                                        Dim s2() As String = s.Split("=")
                                        oD = New KeyValuePair(Of String, String)(s2(0), s2(1))
                                    Else
                                        oD = New KeyValuePair(Of String, String)(s, s)
                                    End If
                                    list.Add(oD)
                                Next

                                For Each ob As KeyValuePair(Of String, String) In list
                                    ddl.Items.Add(New ListItem(ob.Value, ob.Key))
                                Next

                                ' ddl.DataSource = Split(oInfo.FormValue & "", ";")

                                ddl.DataBind()

                            End If
                            ddl.ID = "ctl_" & oInfo.FormItemID
                            ddl.CssClass = IIf(oInfo.CSSClass = String.Empty, "FormDropDownList", oInfo.CSSClass)

                            If oInfo.AllowValueOverride = True Then
                                Dim qsparam As String = Request("dnn_ctr" & Me.ModuleId & "_Form_ctl_" & oInfo.FormItemID)
                                If Not qsparam Is Nothing Then
                                    If qsparam <> "" Then
                                        If oInfo.FormValue.Contains(qsparam) Then
                                            oInfo.FormSelectedValue = Request("dnn_ctr" & Me.ModuleId & "_Form_ctl_" & oInfo.FormItemID)
                                        End If
                                    End If
                                End If
                            End If

                            'Select correct startvalue
                            For Each item As ListItem In ddl.Items
                                If item.Value = oInfo.FormSelectedValue Then
                                    item.Selected = True
                                End If
                            Next

                            'td2.Controls.Add(ddl)
                            div.Controls.Add(ddl)

                            If oInfo.Optional = False Then AddRequiredFieldValidator(oInfo, ddl, div)

                            If oInfo.FormItemTitle.Trim <> "" Then
                                ddl.ToolTip = oInfo.FormItemTitle.Trim
                            End If

                            'MultipleSelect
                        Case FormItem.MultipleSelect
                            Dim rbl As New CheckBoxList

                            '01.00.08 split view into columns
                            If oInfo.CustomData <> String.Empty Then
                                Dim rv() As String = oInfo.CustomData.Split(";")
                                Dim nv() As String = rv(0).Split("=")
                                Select Case nv(0)
                                    Case "ddlMultipleSelectCol"
                                        Dim iCols As Integer = Integer.Parse(nv(1))
                                        If iCols > 1 Then
                                            rbl.RepeatColumns = iCols
                                        End If
                                End Select
                            End If

                            rbl.DataSource = Split(oInfo.FormValue & "", ";")
                            rbl.DataBind()
                            rbl.ID = "ctl_" & oInfo.FormItemID
                            rbl.CssClass = IIf(oInfo.CSSClass = String.Empty, "FormMultipleSelect", oInfo.CSSClass)

                            If oInfo.AllowValueOverride = True Then
                                Dim qsparam As String = Request("dnn_ctr" & Me.ModuleId & "_Form_ctl_" & oInfo.FormItemID)
                                If Not qsparam Is Nothing Then
                                    If qsparam <> "" Then
                                        If oInfo.FormValue.Contains(qsparam) Then
                                            oInfo.FormSelectedValue = Request("dnn_ctr" & Me.ModuleId & "_Form_ctl_" & oInfo.FormItemID)
                                        End If
                                    End If
                                End If
                            End If


                            'Select correct startvalue
                            For Each item As ListItem In rbl.Items
                                If item.Value = oInfo.FormSelectedValue Then
                                    item.Selected = True
                                End If
                            Next

                            'td2.Controls.Add(rbl)
                            div.Controls.Add(rbl)

                            If oInfo.Optional = False Then AddRequiredFieldValidator(oInfo, rbl, div)

                            If oInfo.FormItemTitle.Trim <> "" Then
                                rbl.ToolTip = oInfo.FormItemTitle.Trim
                            End If

                            'Checkbox
                        Case FormItem.Checkbox
                            Dim cb As CheckBox = New CheckBox
                            Try
                                cb.Checked = CType(oInfo.FormValue, Boolean)
                            Catch ec As Exception
                                'Do nothing
                                LoggerSource.Instance.GetLogger(libName).Warn(ec.Message)
                            End Try

                            If oInfo.AllowValueOverride = True Then
                                Dim qsparam As String = Request("dnn_ctr" & Me.ModuleId & "_Form_ctl_" & oInfo.FormItemID)
                                If Not qsparam Is Nothing Then
                                    If qsparam <> "" Then
                                        Select Case qsparam.ToLower
                                            Case "1", "yes", "true", "on"
                                                cb.Checked = True
                                        End Select
                                    End If
                                End If
                            End If

                            cb.ID = "ctl_" & oInfo.FormItemID
                            cb.CssClass = IIf(oInfo.CSSClass = String.Empty, "FormCheckbox", oInfo.CSSClass)
                            'td2.Controls.Add(cb)
                            div.Controls.Add(cb)

                            'no validator needed (on|off)
                            '01.00.03: if marked mandatory add validator (must be ON to submit form)
                            If oInfo.Optional = False Then AddRequiredFieldValidator(oInfo, cb, div)

                            If oInfo.FormItemTitle.Trim <> "" Then
                                cb.ToolTip = oInfo.FormItemTitle.Trim
                            End If

                            'RadioButtonList
                        Case FormItem.RadioButtonList
                            Dim rbl As New RadioButtonList
                            rbl.DataSource = Split(oInfo.FormValue & "", ";")
                            rbl.DataBind()
                            rbl.ID = "ctl_" & oInfo.FormItemID
                            rbl.CssClass = IIf(oInfo.CSSClass = String.Empty, "FormRadioButtonList", oInfo.CSSClass)

                            If oInfo.AllowValueOverride = True Then
                                Dim qsparam As String = Request("dnn_ctr" & Me.ModuleId & "_Form_ctl_" & oInfo.FormItemID)
                                If Not qsparam Is Nothing Then
                                    If qsparam <> "" Then
                                        If oInfo.FormValue.Contains(qsparam) Then
                                            oInfo.FormSelectedValue = Request("dnn_ctr" & Me.ModuleId & "_Form_ctl_" & oInfo.FormItemID)
                                        End If
                                    End If
                                End If
                            End If

                            'Select correct startvalue
                            For Each item As ListItem In rbl.Items
                                If item.Value = oInfo.FormSelectedValue Then
                                    item.Selected = True
                                End If
                            Next

                            'td2.Controls.Add(rbl)
                            div.Controls.Add(rbl)

                            If oInfo.Optional = False Then AddRequiredFieldValidator(oInfo, rbl, div)
                            If oInfo.FormItemTitle.Trim <> "" Then
                                rbl.ToolTip = oInfo.FormItemTitle.Trim
                            End If

                        Case FormItem.DNNRichTextEditControl
                            'DNN editor 

                            'Dim ed As DotNetNuke.UI.UserControls.TextEditor = CType(LoadControl("~/controls/texteditor.ascx"), DotNetNuke.UI.UserControls.TextEditor)
                            'ed.ID = "ctl_" & oInfo.FormItemID
                            ''ed.Text = oInfo.FormValue & ""

                            Dim ed As New DotNetNuke.UI.WebControls.DNNRichTextEditControl()  'DotNetNuke.Web.UI.WebControls.DnnEditor()
                            ed.ID = "ctl_" & oInfo.FormItemID
                            'ed.Value = oInfo.FormValue & ""
                            ed.Value = dnnsafetokenreplace.Replace(oInfo.FormValue & "", str)

                            'ed.Content = oInfo.FormValue & ""

                            If Not oInfo.Width = Null.NullInteger Then
                                ed.Width = oInfo.Width
                            Else
                                ed.Width = 400 'default Width
                            End If
                            If Not oInfo.Height = Null.NullInteger Then
                                ed.Height = oInfo.Height
                            Else
                                ed.Height = 300 'default Height
                            End If
                            'td2.Controls.Add(ed)

                            If oInfo.Optional = False Then AddRequiredFieldValidator(oInfo, ed, div)
                            div.Controls.Add(ed)


                        Case FormItem.FileUpload
                            'File Upload
                            Dim tb As FileUpload = New FileUpload
                            tb.ID = "ctl_" & oInfo.FormItemID
                            tb.CssClass = IIf(oInfo.CSSClass = String.Empty, "FormFileUpload", oInfo.CSSClass)
                            'td2.Controls.Add(tb)
                            div.Controls.Add(tb)
                            If oInfo.Optional = False Then AddRequiredFieldValidator(oInfo, tb, div)
                            If oInfo.FormItemTitle.Trim <> "" Then
                                tb.ToolTip = oInfo.FormItemTitle.Trim
                            End If

                        Case FormItem.HiddenField
                            Dim tb As HiddenField = New HiddenField
                            'tb.Value = oInfo.FormValue & ""
                            tb.Value = dnnsafetokenreplace.Replace(oInfo.FormValue & "", str)

                            tb.ID = "ctl_" & oInfo.FormItemID
                            div.Controls.Add(tb)

                            If oInfo.AllowValueOverride = True Then
                                Dim qsparam As String = Request("dnn_ctr" & Me.ModuleId & "_Form_ctl_" & oInfo.FormItemID)
                                If Not qsparam Is Nothing Then
                                    If qsparam <> "" Then
                                        tb.Value = Request("dnn_ctr" & Me.ModuleId & "_Form_ctl_" & oInfo.FormItemID)
                                    End If
                                End If
                            End If

                    End Select

                Next

                'captcha
                If Settings("Captcha") = True Then

                    Dim sCSSCaptcha As String = CType(Settings("CSSCaptcha"), String)
                    Dim captcha As New MSCaptcha.CaptchaControl()

                    'Dim sCSSCaptcha As String = CType(Settings("CSSCaptcha"), String)
                    'Dim captcha As DotNetNuke.UI.WebControls.CaptchaControl = New DotNetNuke.UI.WebControls.CaptchaControl
                    captcha.ID = "captcha_" & Me.ModuleId
                    'captcha.Text = Localization.GetString("CaptchaText", Me.LocalResourceFile)
                    captcha.ToolTip = Localization.GetString("CaptchaText", Me.LocalResourceFile)
                    'captcha.ErrorMessage = "<div style='float:left' class='" & sCSSCaptcha & "'>" & Localization.GetString("CaptchaError", LocalResourceFile) & "</div>"
                    'captcha.ErrorMessage = "<div class='" & sCSSCaptcha & "'>" & Localization.GetString("CaptchaError", LocalResourceFile) & "</div>"
                    captcha.CustomValidatorErrorMessage = "<div class='" & sCSSCaptcha & "'>" & Localization.GetString("CaptchaError", LocalResourceFile) & "</div>"


                    '01.00.02: number of captcha characters (default 6)
                    Dim chars As Integer = 6
                    Try
                        chars = Integer.Parse(Settings("CaptchaLength"))
                    Catch ex As Exception
                    End Try
                    captcha.CaptchaLength = chars

                    '01.00.02: captcha dimensions
                    Select Case Settings("CaptchaMode")
                        Case "2" 'large
                            captcha.CaptchaWidth = 350 '300
                            captcha.CaptchaHeight = 150

                        Case "1" 'medium
                            captcha.CaptchaWidth = 300 '200
                            captcha.CaptchaHeight = 100

                        Case Else
                            'little (default)
                            captcha.CaptchaWidth = 220 ' 150
                            captcha.CaptchaHeight = 80
                    End Select

                    '01.00.02: use only numbers as characters for Captcha
                    Dim numOnly As Boolean = CType(Settings("CaptchaNumbers"), Boolean)
                    If numOnly = True Then captcha.CaptchaChars = "01234567890"

                    Dim divCaptcha As New HtmlGenericControl("div")
                    divCaptcha.ID = "divCaptcha" & Me.ModuleId
                    divCaptcha.Attributes("class") = "dnnFormItem msCaptcha " & sCSSCaptcha 'dnnTooltip
                    Dim lblc As New Label
                    lblc.CssClass = "dnnFormLabel"
                    lblc.Text = Localization.GetString("Captcha.Text", LocalResourceFile)
                    lblc.ToolTip = lblc.Text

                    Dim txtCaptchaAnswer As New TextBox
                    txtCaptchaAnswer.ID = "txtCaptchaAnswer" & Me.ModuleId
                    divCaptcha.Controls.Add(lblc)
                    divCaptcha.Controls.Add(txtCaptchaAnswer)
                    divCaptcha.Controls.Add(captcha)
                    fieldset.Controls.Add(divCaptcha)

                End If

                    'validation summary 
                    If hasValidators = True AndAlso CType(Settings("chkValSum"), Boolean) = True Then
                    Dim vs As New ValidationSummary
                        vs.ValidationGroup = "ods" & Me.ModuleId
                        vs.DisplayMode = ValidationSummaryDisplayMode.BulletList
                        vs.HeaderText = Localization.GetString("ValidationSummaryTitle", LocalResourceFile)
                        vs.CssClass = "dnnFormValidationSummary " ' "ValidationSummary"
                        'vs.EnableClientScript = true
                        'position: top or bottom
                        If Settings("posValSum") = "0" Then
                            fieldset.Controls.AddAt(0, vs)
                        Else
                            fieldset.Controls.Add(vs)
                        End If
                    End If

                    'action buttons (submit, reset)
                    Dim ulActions As New HtmlGenericControl("ul")
                    ulActions.Attributes("class") = "dnnActions dnnClear"
                    formDIV.Controls.Add(ulActions)

                Dim liSubmit As New HtmlGenericControl("li")
                liSubmit.Controls.Add(aSubmit)
                ulActions.Controls.Add(liSubmit)

                    'hide reset button?
                    If CType(Settings("chkHideReset"), Boolean) = True Then
                        aReset.Visible = False
                    Else
                        Dim liReset As New HtmlGenericControl("li")
                        liReset.Controls.Add(aReset)
                        ulActions.Controls.Add(liReset)
                    End If

                    formDIV.Controls.Add(ulActions)

                    ' End If


                    ' FREE FORM SUBMISION
                    Dim odsModuleID As String = Request("odsid") & String.Empty
                    If odsModuleID = Me.ModuleId.ToString Then
                        'filter submissions for this form only, so that we can have multiple forms on a page and let them be used for free forms posts
                        Dim odsAction As String = Request("odsaction") & String.Empty
                        ' odsaction submit
                        If odsAction.ToLower = "submit" Then
                            Page.Validate()
                            cmdSubmit_Click(Nothing, EventArgs.Empty)
                        End If
                    End If


            Catch ex As Exception
                LoggerSource.Instance.GetLogger(libName).Error(ex.Message)

                ProcessModuleLoadException(Me, ex)
            End Try

        End Sub
#End Region

#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Localization.GetString(Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl(), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
                'Actions.Add(GetNextActionID, Localization.GetString("ViewSubmissions", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl("ViewSubmissions"), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
                '01.00.05: SubmissionManager
                Actions.Add(GetNextActionID, Localization.GetString("ViewSubmissions", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl("SubmissionManager"), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property
#End Region

        Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles cmdSubmit.Click
            Try
               
                If Not Page.IsValid Then
                    LoggerSource.Instance.GetLogger(libName).Debug("FORM INVALID, REJECT SUBMISSION")
                    Exit Sub
                End If

                'captcha
                If Settings("Captcha") = True Then

                    'mscaptcha
                    Dim captcha As MSCaptcha.CaptchaControl = CType(FindControl("captcha_" & Me.ModuleId), MSCaptcha.CaptchaControl)
                    Dim txtCaptchaAnswer As TextBox = CType(FindControl("txtCaptchaAnswer" & Me.ModuleId), TextBox)
                   
                    Try
                        captcha.ValidateCaptcha(txtCaptchaAnswer.Text)
                    Catch ex As Exception
                        LoggerSource.Instance.GetLogger(libName).Error(ex.Message)
                        LoggerSource.Instance.GetLogger(libName).Error(ex.StackTrace)
                    End Try


                    If Not captcha.UserValidated Then

                        'Dim captcha As DotNetNuke.UI.WebControls.CaptchaControl = CType(FindControl("captcha_" & Me.ModuleId), DotNetNuke.UI.WebControls.CaptchaControl)
                        'If Not captcha.IsValid Then
                        LoggerSource.Instance.GetLogger(libName).Warn("INVALID CAPTCHA, REJECT SUBMISSION")
                        Dim sCSSCaptcha As String = CType(Settings("CSSCaptcha"), String)
                        'captcha.ErrorMessage = "<div style='float:left' class='" & sCSSCaptcha & "'>" & Localization.GetString("CaptchaError", LocalResourceFile) & "</div>"

                        Dim divCaptcha As HtmlGenericControl = FindControl("divCaptcha" & Me.ModuleId)
                        Dim captchaError As New Label
                        Dim divMsg As New HtmlGenericControl("div")
                        divMsg.Attributes("class") = "dnnFormMessage dnnFormError"
                        captchaError.Text = "<div style='float:left' class='" & sCSSCaptcha & "'>" & Localization.GetString("CaptchaError", LocalResourceFile) & "</div>"
                        divMsg.Controls.Add(captchaError)

                        divCaptcha.Controls.Add(divMsg)
                        'plcODS.Controls.Add(divMsg)

                        Exit Sub
                    End If
                End If

                Dim u As UserInfo = Nothing
                Dim postdata As String = String.Empty
                Dim oInfo As FormItemInfo
                Dim AllowContactUsers As Boolean = CType(Settings("AllowContactUsers"), Boolean)
                If AllowContactUsers AndAlso Request("userid") <> "" Then
                    u = UserController.GetUserById(Me.PortalId, Request("userid"))
                End If

                'Get list of formcontrols
                Dim oController As New FormItemController
                Dim al As ArrayList = oController.List(MyBase.PortalId, MyBase.ModuleId, cultureCode)

                Dim sz As String = String.Empty
                Dim szVal As String = String.Empty
                Dim szValWebHook As String = String.Empty
                Dim sHtmlMailBody As String = String.Empty
                Dim bValid As Boolean = Page.IsValid ' True 'is all required fields filled
                Dim d As New XmlDocument()
                Dim root As XmlElement = d.CreateElement("Submission")
                d.AppendChild(root)


                'Iterate formcontols
                For Each o As Object In al
                    oInfo = CType(o, FormItemInfo)
                    szVal = ""
                    szValWebHook = szVal

                    'Create the control and extract submitted value
                    Select Case oInfo.FormType

                        Case FormItem.TextBox, FormItem.TextArea
                            Dim tb As TextBox = CType(FindControl("ctl_" & oInfo.FormItemID), TextBox)
                            szVal = tb.Text
                            'DNN token-replace parser
                            'cfr http://www.dnnsoftware.com/wiki/Page/Tokens
                            'es: [Tab:TabName]
                            Dim dnnsafetokenreplace As New Regex("(\[([^: ]*):([^:/ ]*)\])", RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.Compiled)
                            Dim str As New MatchEvaluator(AddressOf DNNTokenReplace)
                            tb.Text = dnnsafetokenreplace.Replace(tb.Text, str)
                            szVal = tb.Text

                            If Not oInfo.Optional Then
                                If Len(Trim(tb.Text)) = 0 Then
                                    bValid = False
                                End If
                            End If
                            szValWebHook = szVal

                        Case FormItem.DropDownList
                            Dim ddl As DropDownList = CType(FindControl("ctl_" & oInfo.FormItemID), DropDownList)
                            szVal = ddl.SelectedItem.Text
                            szValWebHook = ddl.SelectedItem.Value

                            If Not oInfo.Optional Then
                                If Len(Trim(ddl.SelectedValue)) = 0 Then
                                    bValid = False
                                End If
                            End If
                        Case FormItem.RadioButtonList
                            Dim rbl As RadioButtonList = CType(FindControl("ctl_" & oInfo.FormItemID), RadioButtonList)
                            If rbl.SelectedItem Is Nothing Then
                                szVal = ""
                            Else
                                szVal = rbl.SelectedItem.Text
                            End If

                            If Not oInfo.Optional Then
                                If Len(Trim(szVal)) = 0 Then
                                    bValid = False
                                End If
                            End If
                            szValWebHook = szVal

                        Case FormItem.MultipleSelect
                            Dim rbl As CheckBoxList = CType(FindControl("ctl_" & oInfo.FormItemID), CheckBoxList)
                            If rbl.SelectedItem Is Nothing Then
                                szVal = ""
                            Else
                                szVal = ""
                                'szVal = rbl.SelectedItem.Text
                                For Each i As ListItem In rbl.Items
                                    If i.Selected = True Then
                                        szVal &= i.Text & ","
                                        szValWebHook &= i.Value & ","
                                    End If
                                Next
                                If szVal.EndsWith(",") Then szVal = szVal.Substring(0, szVal.Length - 1)
                                If szValWebHook.EndsWith(",") Then szValWebHook = szValWebHook.Substring(0, szValWebHook.Length - 1)
                            End If

                            If Not oInfo.Optional Then
                                If Len(Trim(szVal)) = 0 Then
                                    bValid = False
                                End If
                            End If

                        Case FormItem.Checkbox
                            Dim cb As CheckBox = CType(FindControl("ctl_" & oInfo.FormItemID), CheckBox)

                            If Not cb Is Nothing Then
                                'AL: oInfo.FormValue is "True" or "False" to enable or disable checkbox at pageload
                                szVal = cb.Checked.ToString
                                'If cb.Checked Then
                                '    szVal = oInfo.FormValue
                                'End If
                            End If
                            szValWebHook = szVal

                        Case FormItem.DNNRichTextEditControl
                            Dim ed As DotNetNuke.UI.WebControls.DNNRichTextEditControl = CType(FindControl("ctl_" & oInfo.FormItemID), DotNetNuke.UI.WebControls.DNNRichTextEditControl)

                            'Dim ed As DotNetNuke.UI.UserControls.TextEditor = CType(FindControl("ctl_" & oInfo.FormItemID), DotNetNuke.UI.UserControls.TextEditor)

                            'Dim ed As DotNetNuke.Web.UI.WebControls.DnnEditor = CType(FindControl("ctl_" & oInfo.FormItemID), DotNetNuke.Web.UI.WebControls.DnnEditor)
                            szVal = ed.Value
                            'szVal = ed.Text
                            If Not oInfo.Optional Then
                                If Len(Trim(szVal)) = 0 Then
                                    bValid = False
                                End If
                            End If
                            szValWebHook = szVal

                        Case FormItem.FileUpload
                            Dim ed As FileUpload = CType(FindControl("ctl_" & oInfo.FormItemID), FileUpload)
                            szVal = ed.FileName
                            If szVal <> String.Empty Then
                                Dim folderID As String = oInfo.FormValue
                                Dim fo As New FolderController
                                'Dim myFolder As FolderInfo = fo.GetFolderInfo(Me.PortalId, oInfo.FormValue)
                                Dim myFolder As FolderInfo = FolderManager.Instance().GetFolder(oInfo.FormValue)

                                If myFolder Is Nothing Then
                                    'MyLog("ERROR, cannot find folder " & oInfo.FormValue)
                                    szVal = "ERROR, cannot find folder " & oInfo.FormValue
                                Else
                                    Try
                                        MyLog("save to " & myFolder.PhysicalPath & szVal)
                                        'append suffix + extension
                                        Dim p As Integer = szVal.LastIndexOf(".")
                                        szVal = szVal.Substring(0, p) & "." & Now.Ticks.ToString & "." & szVal.Substring(p + 1)
                                        MyLog("save as " & myFolder.PhysicalPath & szVal)
                                        ed.SaveAs(myFolder.PhysicalPath & szVal)
                                    Catch ex As Exception
                                        MyLog("ERROR, cannot save file " & oInfo.FormValue & ": " & ex.Message)
                                        szVal = "" ' "ERROR, cannot save file " & oInfo.FormValue & " " & ex.Message
                                    End Try

                                    Try
                                        'give full URL to file to allow download
                                        Dim uri As New Uri(HttpContext.Current.Request.Url.AbsoluteUri)
                                        szVal = "<a href='" & uri.Scheme + Uri.SchemeDelimiter + uri.Host & Me.PortalSettings.HomeDirectory & myFolder.FolderPath & szVal & "'>" & ed.FileName & "</a>"
                                    Catch ex As Exception
                                        szVal = "ERROR, cannot parse file " & oInfo.FormValue & " " & ex.Message
                                    End Try
                                End If


                            End If
                            szValWebHook = szVal

                        Case FormItem.HiddenField
                            Dim tb As HiddenField = CType(FindControl("ctl_" & oInfo.FormItemID), HiddenField)
                            szVal = tb.Value

                            'DNN token-replace parser
                            'cfr http://www.dnnsoftware.com/wiki/Page/Tokens
                            'es: [Tab:TabName]
                            Dim dnnsafetokenreplace As New Regex("(\[([^: ]*):([^:/ ]*)\])", RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.Compiled)
                            Dim str As New MatchEvaluator(AddressOf DNNTokenReplace)
                            tb.Value = dnnsafetokenreplace.Replace(tb.Value, str)
                            szVal = tb.Value
                            szValWebHook = szVal

                    End Select

                    If CBool(Settings("enableWebHook")) AndAlso Not String.IsNullOrEmpty(oInfo.WebhookFieldName) Then
                        If postdata <> String.Empty Then
                            postdata &= "&"
                        End If
                        postdata &= IIf(String.IsNullOrEmpty(oInfo.WebhookFieldName), oInfo.FormItemID, oInfo.WebhookFieldName.Trim()) & "=" & HttpUtility.UrlEncode(szValWebHook)
                    End If

                    'Add label to formitem value
                    ' If oInfo.FormType <> FormItem.Label Then
                    sz = sz & "*** " & oInfo.FormLabel & " ***" & vbCrLf & szVal & vbCrLf & vbCrLf & vbCrLf

                        sHtmlMailBody &= "<b>" & oInfo.FormLabel & "</b>:<br/>" & szVal & "<br/><br/>"

                    ' End If

                    Dim sXML As XmlDocument = oInfo.ToXML(cultureCode)
                    Dim ele As XmlElement = sXML.GetElementsByTagName("SubmissionValue")(0)
                    Dim cdata As XmlCDataSection = sXML.CreateCDataSection(szVal)
                    ele.AppendChild(cdata)
                    'ele.InnerText = szVal
                    root.AppendChild(d.ImportNode(sXML.FirstChild, True))
                Next

                'if AllowContactUsers enabled save the user who was contacted
                If Not u Is Nothing Then
                    sz = sz & "*** " & "Sent To User" & " ***" & vbCrLf & u.DisplayName & " (" & u.Email & ")" & vbCrLf & vbCrLf & vbCrLf
                    sHtmlMailBody &= "<b>" & "Sent To User" & "</b>:<br/>" & u.DisplayName & " (" & u.Email & ")" & "<br/><br/>"
                    Dim sentTO As XmlElement = d.CreateElement("SentToUser")
                    sentTO.InnerText = u.DisplayName
                    Dim uid As XmlAttribute = d.CreateAttribute("userid")
                    uid.Value = u.UserID
                    sentTO.Attributes.Append(uid)
                    root.AppendChild(sentTO)
                End If

                Dim AllowMailto As Boolean = CType(Settings("AllowMailto"), Boolean), doMailTo As Boolean = False
                If AllowMailto AndAlso ValidateMailto() Then
                    doMailTo = True
                    sz = sz & "*** " & "Sent To User" & " ***" & vbCrLf & Request("mailto") & vbCrLf & vbCrLf & vbCrLf
                    sHtmlMailBody &= "<b>" & "Sent To User" & "</b>:<br/>" & Request("mailto") & "<br/><br/>"
                    Dim sentTO As XmlElement = d.CreateElement("SentToUser")
                    sentTO.InnerText = Request("mailto")
                    root.AppendChild(sentTO)
                End If

                'Send mail
                Dim bEmailFailed As Boolean
                If CBool(Settings("cbEmail")) Then
                    Dim szEmails As String = CStr(Settings("tbEmail"))
                    Dim EmailSender As String = CStr(Settings("EmailSender"))
                    If EmailSender Is String.Empty Then
                        EmailSender = Split(szEmails, ";")(0)
                    End If
                    'Find subject in email
                    Dim szSubject As String = Localization.GetString("EMailSubject.Text", LocalResourceFile)
                    szSubject = Replace(szSubject, "[ModuleTitle]", MyBase.ModuleConfiguration.ModuleTitle)
                    szSubject = Replace(szSubject, "[PortalName]", MyBase.PortalSettings.PortalName)

                    'Send email
                    Try
                        LoggerSource.Instance.GetLogger(libName).Debug("Sending email to " & szEmails)

                        'plain text
                        'DotNetNuke.Services.Mail.Mail.SendMail(Split(szEmails, ";")(0), szEmails, "", szSubject, sz, "", "", "", "", "", "")
                        'html
                        DotNetNuke.Services.Mail.Mail.SendMail(EmailSender, szEmails, "", szSubject, sHtmlMailBody, "", "HTML", "", "", "", "")

                    Catch ex As Exception
                        LoggerSource.Instance.GetLogger(libName).Error(ex.Message)
                        'mail not sent
                        bEmailFailed = True
                    End Try
                End If

                Dim bDbFailed As Boolean
                If CBool(Settings("cbDatabase")) Then
                    Dim oSInfo As New FormSubmissionInfo
                    oSInfo.ModuleID = MyBase.ModuleId
                    oSInfo.PortalID = MyBase.PortalId
                    oSInfo.Submission = sz
                    oSInfo.SubmissionXML = d.OuterXml
                    oSInfo.SubmissionDate = Date.Now

                    Dim oSController As New FormSubmissionController
                    oSController.AddODSFormSubmission(oSInfo)
                End If

                ' POST to WebHook
                Try
                    LoggerSource.Instance.GetLogger(libName).Info("enableWebHook: " & Settings("enableWebHook"))
                    If CBool(Settings("enableWebHook")) Then
                        Dim whURL As String = CType(Settings("WHURL-" & cultureCode), String)
                        If String.IsNullOrEmpty(whURL) Then
                            LoggerSource.Instance.GetLogger(libName).Error("Cannot POST to WebHook, please configure URL")
                        Else
                            Dim h As New Webhook
                            LoggerSource.Instance.GetLogger(libName).Info("Posting submission to " & whURL)
                            LoggerSource.Instance.GetLogger(libName).Debug("DATA: " & postdata)
                            Dim pr As String = h.Post(whURL, postdata)
                            LoggerSource.Instance.GetLogger(libName).Info("POST RESULT: " & pr)
                        End If

                    End If
                Catch er As Exception
                    LoggerSource.Instance.GetLogger(libName).Error("Cannot post to Webhook: " & er.Message)
                    LoggerSource.Instance.GetLogger(libName).Error(er.StackTrace)
                End Try

                'contact user mode
                If Not u Is Nothing Then

                    Dim szEmails As String = u.Email
                    Dim EmailSender As String = CStr(Settings("EmailSender"))
                    If EmailSender Is String.Empty Then
                        Dim szEmailsSettings As String = CStr(Settings("tbEmail"))
                        EmailSender = Split(szEmailsSettings, ";")(0)
                    End If

                    'Find subject in email
                    Dim szSubject As String = Localization.GetString("EMailSubject.Text", LocalResourceFile)
                    szSubject = Replace(szSubject, "[ModuleTitle]", MyBase.ModuleConfiguration.ModuleTitle)
                    szSubject = Replace(szSubject, "[PortalName]", MyBase.PortalSettings.PortalName)

                    'Send email
                    Try
                        LoggerSource.Instance.GetLogger(libName).Debug("USER mode: Sending email to " & szEmails)
                        'plain text
                        'DotNetNuke.Services.Mail.Mail.SendMail(Split(szEmails, ";")(0), szEmails, "", szSubject, sz, "", "", "", "", "", "")
                        'html
                        DotNetNuke.Services.Mail.Mail.SendMail(EmailSender, szEmails, "", szSubject, sHtmlMailBody, "", "HTML", "", "", "", "")

                    Catch ex As Exception
                        LoggerSource.Instance.GetLogger(libName).Error(ex.Message)
                        'mail not sent
                        bEmailFailed = True
                    End Try

                End If

                'mailto mode
                If doMailTo Then

                    Dim szEmails As String = Request("mailto")

                    'Find subject in email
                    Dim szSubject As String = Localization.GetString("EMailSubject.Text", LocalResourceFile)
                    szSubject = Replace(szSubject, "[ModuleTitle]", MyBase.ModuleConfiguration.ModuleTitle)
                    szSubject = Replace(szSubject, "[PortalName]", MyBase.PortalSettings.PortalName)

                    Dim EmailSender As String = CStr(Settings("EmailSender"))
                    If EmailSender Is String.Empty Then
                        Dim szEmailsSettings As String = CStr(Settings("tbEmail"))
                        EmailSender = Split(szEmailsSettings, ";")(0)
                    End If

                    'Send email
                    Try
                        LoggerSource.Instance.GetLogger(libName).Debug("MAILTO mode: Sending email to " & szEmails)
                        'plain text
                        'DotNetNuke.Services.Mail.Mail.SendMail(Split(szEmails, ";")(0), szEmails, "", szSubject, sz, "", "", "", "", "", "")
                        'html
                        DotNetNuke.Services.Mail.Mail.SendMail(EmailSender, szEmails, "", szSubject, sHtmlMailBody, "", "HTML", "", "", "", "")

                    Catch ex As Exception
                        'mail not sent
                        bEmailFailed = True
                    End Try

                End If

                'Send error message if form not received by mail or database
                plcODS.Controls.Clear() 'hide form

                If bEmailFailed And bDbFailed Then
                    Dim litODS As New Label
                    litODS.Text = Localization.GetString("SubmitError.Text", LocalResourceFile)
                    litODS.CssClass = "dnnFormError"
                    plcODS.Controls.Add(litODS)
                Else
                    '01.00.06
                    Dim ConfirmMode As String = CType(Settings("ConfirmMode"), String)
                    If ConfirmMode = "1" Then

                        Dim rdURL As String = Trim(CStr(Settings("tbURL-" & cultureCode))) & ""

                        LoggerSource.Instance.GetLogger(libName).Debug("rdURL=" & rdURL)

                        If rdURL <> "" Then

                            'tabid o full url?
                            If IsNumeric(rdURL) Then
                                Response.Redirect(NavigateURL(Integer.Parse(rdURL)))
                            Else
                                Response.Redirect(rdURL)
                            End If

                        End If

                    Else
                        Dim ConfirmTitle As String = CType(Settings("ConfirmTitle-" & cultureCode), String)
                        Dim ConfirmMessage As String = CType(Settings("ConfirmMessage-" & cultureCode), String)

                        If ConfirmTitle <> "" Then
                            Me.ModuleConfiguration.ModuleTitle = ConfirmTitle
                        End If

                        Dim divMsg As New HtmlGenericControl("div")
                        divMsg.Attributes("class") = "dnnFormMessage dnnFormSuccess"
                        Dim litODS As New Label
                        If ConfirmMessage <> "" Then
                            litODS.Text = HttpUtility.HtmlDecode(ConfirmMessage)
                        Else
                            litODS.Text = Localization.GetString("SubmitSuccess.Text", LocalResourceFile)
                        End If
                        divMsg.Controls.Add(litODS)
                        plcODS.Controls.Add(divMsg)

                    End If

                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try

        End Sub

        ''' <summary>
        ''' Reset Form (reload page)
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub cmdReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles cmdReset.Click
            Response.Redirect(NavigateURL(TabId))
        End Sub

        'Private Sub AddRequiredFieldValidator(ByVal oInfo As FormItemInfo, ByVal ctl As WebControl, ByVal td As HtmlTableCell)
        Private Sub AddRequiredFieldValidator(ByVal oInfo As FormItemInfo, ByVal ctl As WebControl, ByVal td As HtmlGenericControl)
            '01.00.06 DNN UX
            ctl.CssClass &= " dnnFormRequired"

            Select Case oInfo.FormType

                Case FormItem.MultipleSelect
                    Dim val As New CheckBoxListValidator
                    val.ValidationGroup = "ods" & Me.ModuleId
                    val.ControlToValidate = ctl.ID
                    val.CssClass = "dnnFormMessage dnnFormError"
                    val.ErrorMessage = oInfo.FormLabel & "&nbsp;" & Localization.GetString("FieldRequired", LocalResourceFile)
                    val.EnableClientScript = False
                    val.Display = ValidatorDisplay.Dynamic
                    td.Controls.Add(val)

                    '01.00.03: validate Checkbox
                Case FormItem.Checkbox
                    Dim val As New CheckBoxValidator
                    val.ValidationGroup = "ods" & Me.ModuleId
                    val.ControlToValidate = ctl.ID
                    val.CssClass = "dnnFormMessage dnnFormError"
                    val.ErrorMessage = oInfo.FormLabel & "&nbsp;" & Localization.GetString("FieldRequired", LocalResourceFile)
                    val.EnableClientScript = False
                    val.Display = ValidatorDisplay.Dynamic
                    td.Controls.Add(val)

                Case Else
                    Dim val As New RequiredFieldValidator()
                    val.ValidationGroup = "ods" & Me.ModuleId
                    val.ControlToValidate = ctl.ID
                    val.CssClass = "dnnFormMessage dnnFormError"
                    val.ErrorMessage = oInfo.FormLabel & "&nbsp;" & Localization.GetString("FieldRequired", LocalResourceFile)
                    val.EnableClientScript = True
                    val.Display = ValidatorDisplay.Dynamic
                    td.Controls.Add(val)
            End Select

            hasValidators = True

        End Sub

        'Private Sub AddRegexFieldValidator(ByVal oInfo As FormItemInfo, ByVal ctl As Control, ByVal td As HtmlTableCell, ByVal exp As String)
        Private Sub AddRegexFieldValidator(ByVal oInfo As FormItemInfo, ByVal ctl As Control, ByVal td As HtmlGenericControl, ByVal exp As String)
            Try

                'LoggerSource.Instance.GetLogger(libName).Debug("AddRegexFieldValidator for " & ctl.ID & ", " & exp)
                Dim val As New RegularExpressionValidator
                val.ValidationGroup = "ods" & Me.ModuleId
                val.ValidationExpression = exp

                'disable client side validation to avoid js errors
                val.EnableClientScript = False
                'val.ValidationExpression = "/" & exp & "/"

                'LoggerSource.Instance.GetLogger(libName).Debug("EXP=" & val.ValidationExpression)

                val.ControlToValidate = ctl.ID
                val.CssClass = "dnnFormMessage dnnFormError"
                val.ErrorMessage = oInfo.FormLabel & "&nbsp;" & Localization.GetString("RegexFailed", LocalResourceFile)
                td.Controls.Add(val)

                hasValidators = True

            Catch ex As Exception
                LoggerSource.Instance.GetLogger(libName).Error("Invalid regex: " & exp)
            End Try

        End Sub


        Public Sub MyLog(ByVal logMessage As String)
            LoggerSource.Instance.GetLogger(libName).Debug(logMessage)
        End Sub

        Private Function DNNTokenReplace(m As Match) As String
            Dim tk As DotNetNuke.Services.Tokens.TokenReplace = New DotNetNuke.Services.Tokens.TokenReplace()
            tk.ModuleId = Me.ModuleId
            Return tk.ReplaceEnvironmentTokens(m.Value)
        End Function

        ''' <summary>
        ''' cascading dropdowns: perform sql query to populate linked dropdown
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub ddlCascading_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim ddl As DropDownList = sender
            Dim value As String = ddl.SelectedValue
            'MyLog(ddl.ID & " changed to " & value)
            'get id of child from customdata, and run query
            Dim sID() As String = ddl.ID.ToString().Split("_")
            'MyLog("sid0=" & sID(0))
            'MyLog("sid1=" & sID(1))
            Dim myID As String = sID(1)
            Dim ct As FormItemController = New FormItemController
            Dim oi As FormItemInfo = ct.Get(myID)
            If oi Is Nothing Then
                MyLog("Could not load control " & myID)
                Return
            End If
            If oi.CustomData.Contains(":") Then

                'SQL query customdata: childID:sqlquery
                Dim vv() As String = oi.CustomData.Split(":")
                Dim sql As String = vv(1).Replace("[ID]", value)
                'MyLog("child is id " & vv(0))
                'MyLog("sql = " & sql)

                Dim child As DropDownList = FindControl("ctl_" & vv(0))
                If child Is Nothing Then
                    MyLog("could not load child DDL")
                    Return
                Else
                    Dim conn As New SqlConnection(DotNetNuke.Common.Utilities.Config.GetConnectionString())
                    Try
                        conn.Open()
                        Dim cmd As SqlCommand = conn.CreateCommand
                        cmd.CommandTimeout = 60
                        cmd.CommandText = sql
                        Dim dr As SqlDataReader = cmd.ExecuteReader
                        Dim dt As DataTable = New DataTable
                        dt.Columns.Add("ID")
                        dt.Columns.Add("VALUE")
                        Dim emptyRow As DataRow = dt.NewRow
                        dt.Rows.Add(emptyRow)
                        Do While dr.Read
                            Dim r As DataRow = dt.NewRow
                            If TypeOf (dr(0)) Is DateTime Then
                                r(0) = dr(0).ToShortDateString
                            Else
                                r(0) = dr(0)
                            End If
                            If TypeOf (dr(1)) Is DateTime Then
                                r(1) = dr(1).ToShortDateString
                            Else
                                r(1) = dr(1)
                            End If
                            dt.Rows.Add(r)
                        Loop
                        child.DataSource = dt
                        child.DataValueField = "ID"
                        child.DataTextField = "VALUE"
                        child.DataBind()
                    Catch ex As Exception
                        LoggerSource.Instance.GetLogger(libName).Error(ex.Message)
                        Dim lblErr As Label = New Label
                        lblErr.Text = "ERROR IN QUERY: " & ex.Message
                        lblErr.ForeColor = Color.Red
                        child.Parent.Controls.Add(lblErr)
                    Finally
                        conn.Close()
                    End Try

                End If
                'Else
                '    'cascading Non sql?
                '    Dim child As DropDownList = FindControl("ctl_" & oi.CustomData)
                '    If child Is Nothing Then
                '        MyLog("could not load child DDL")
                '        Return
                '    Else

                '    End If
            End If

        End Sub

    End Class

End Namespace