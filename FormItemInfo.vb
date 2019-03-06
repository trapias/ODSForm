Imports System
Imports System.Xml

Namespace ODS.DNN.Modules.Form.Business

    Public Class FormItemInfo

#Region "Private Members"
        Dim _formItemID As Integer
        Dim _moduleID As Integer
        Dim _portalID As Integer
        Dim _formType As Integer
        Dim _formValue As String
        Dim _formSelectedValue As String
        Dim _formLabel As String
        Dim _sortValue As Integer
        Dim _optional As Boolean
        Dim _width As Integer
        Dim _height As Integer
        Dim _class As String
        Dim _formLabelClass As String
        Dim _CustomRegex As String
        Dim _FormItemTitle As String
        Dim _culture As String
        Dim _AllowValueOverride As Boolean
        Dim _custom As String
        Dim _WebhookFieldName As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub

        Public Sub New(ByVal formItemID As Integer, ByVal moduleID As Integer, ByVal portalID As Integer, ByVal formType As Integer, ByVal formValue As String, ByVal formSelectedValue As String, ByVal formLabel As String, ByVal sortValue As Integer, ByVal [optional] As Boolean, ByVal CSSClass As String, ByVal LabelClass As String, ByVal CustomRegex As String, ByVal FormItemTitle As String, ByVal Culture As String, ByVal allowValueOverride As Boolean)
            Me.FormItemID = formItemID
            Me.ModuleID = moduleID
            Me.PortalID = portalID
            Me.FormType = formType
            Me.FormValue = formValue
            Me.FormSelectedValue = formSelectedValue
            Me.FormLabel = formLabel
            Me.SortValue = sortValue
            Me.Optional = [optional]
            Me.CSSClass = CSSClass
            Me.FormLabelClass = LabelClass
            Me.CustomRegex = CustomRegex
            Me.FormItemTitle = FormItemTitle
            Me.Culture = Culture
            Me.AllowValueOverride = allowValueOverride
        End Sub
#End Region

#Region "Public Properties"
        Public Property FormItemID() As Integer
            Get
                Return _formItemID
            End Get
            Set(ByVal Value As Integer)
                _formItemID = Value
            End Set
        End Property

        Public Property [Optional]() As Boolean
            Get
                Return _optional
            End Get
            Set(ByVal Value As Boolean)
                _optional = Value
            End Set
        End Property

        Public Property ModuleID() As Integer
            Get
                Return _moduleID
            End Get
            Set(ByVal Value As Integer)
                _moduleID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _portalID
            End Get
            Set(ByVal Value As Integer)
                _portalID = Value
            End Set
        End Property

        Public Property FormType() As Integer
            Get
                Return _formType
            End Get
            Set(ByVal Value As Integer)
                _formType = Value
            End Set
        End Property

        Public Property FormValue() As String
            Get
                Return _formValue
            End Get
            Set(ByVal Value As String)
                _formValue = Value
            End Set
        End Property

        Public Property FormSelectedValue() As String
            Get
                Return _formSelectedValue
            End Get
            Set(ByVal Value As String)
                _formSelectedValue = Value
            End Set
        End Property

        Public Property FormLabel() As String
            Get
                Return _formLabel
            End Get
            Set(ByVal Value As String)
                _formLabel = Value
            End Set
        End Property

        Public Property SortValue() As Integer
            Get
                Return _sortValue
            End Get
            Set(ByVal Value As Integer)
                _sortValue = Value
            End Set
        End Property

        Public Property Width() As Integer
            Get
                Return _width
            End Get
            Set(ByVal value As Integer)
                _width = value
            End Set
        End Property

        Public Property Height() As Integer
            Get
                Return _height
            End Get
            Set(ByVal value As Integer)
                _height = value
            End Set
        End Property

        Public Property CSSClass() As String
            Get
                Return _class
            End Get
            Set(ByVal value As String)
                _class = value
            End Set
        End Property

        Public Property FormLabelClass() As String
            Get
                Return _formLabelClass
            End Get
            Set(ByVal Value As String)
                _formLabelClass = Value
            End Set
        End Property

        Public Property CustomRegex() As String
            Get
                Return _CustomRegex
            End Get
            Set(ByVal value As String)
                _CustomRegex = value
            End Set
        End Property

        Public Property FormItemTitle() As String
            Get
                Return _FormItemTitle
            End Get
            Set(ByVal value As String)
                _FormItemTitle = value
            End Set
        End Property

        Public Property Culture() As String
            Get
                Return _culture
            End Get
            Set(ByVal Value As String)
                _culture = Value
            End Set
        End Property

        Public Property AllowValueOverride() As Boolean
            Get
                Return _AllowValueOverride
            End Get
            Set(ByVal Value As Boolean)
                _AllowValueOverride = Value
            End Set
        End Property

        ''' <summary>
        ''' Store custom values for field types
        ''' e.g. ddlMultipleSelectColumns.columns=2
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CustomData() As String
            Get
                Return _custom
            End Get
            Set(ByVal Value As String)
                _custom = Value
            End Set
        End Property

        Public Property WebhookFieldName() As String
            Get
                Return _WebhookFieldName
            End Get
            Set(ByVal Value As String)
                _WebhookFieldName = Value
            End Set
        End Property

#End Region

#Region "Methods"

        Public Function ToXML(ByVal cultureCode As String) As XmlDocument
            Dim d As XmlDocument = New XmlDocument()
            'Try

            Dim decl As XmlDeclaration = d.CreateXmlDeclaration("1.0", Nothing, Nothing)
            Dim sRoot As String = "<FormItem/>"
            d.LoadXml(sRoot)

            Dim root As XmlElement
            root = d.DocumentElement

            'FormItemID
            Dim id As XmlAttribute = d.CreateAttribute("FormItemID")
            id.Value = Me.FormItemID
            root.Attributes.Append(id)

            'moduleid
            Dim moduleid As XmlAttribute = d.CreateAttribute("ModuleID")
            moduleid.Value = Me.ModuleID
            root.Attributes.Append(moduleid)

            'portalid
            Dim portalid As XmlAttribute = d.CreateAttribute("PortalID")
            portalid.Value = Me.PortalID
            root.Attributes.Append(portalid)

            'FormType
            Dim FormType As XmlAttribute = d.CreateAttribute("FormType")
            FormType.Value = Me.FormType
            root.Attributes.Append(FormType)

            'FormValue
            Dim FormValue As XmlAttribute = d.CreateAttribute("FormValue")
            FormValue.Value = Me.FormValue
            root.Attributes.Append(FormValue)

            'FormSelectedValue
            Dim FormSelectedValue As XmlAttribute = d.CreateAttribute("FormSelectedValue")
            FormSelectedValue.Value = Me.FormSelectedValue
            root.Attributes.Append(FormSelectedValue)

            'FormLabel
            Dim FormLabel As XmlAttribute = d.CreateAttribute("FormLabel")
            FormLabel.Value = Me.FormLabel
            root.Attributes.Append(FormLabel)

            'SortValue
            Dim SortValue As XmlAttribute = d.CreateAttribute("SortValue")
            SortValue.Value = Me.SortValue
            root.Attributes.Append(SortValue)

            'Optional
            Dim fOptional As XmlAttribute = d.CreateAttribute("Optional")
            fOptional.Value = Me.Optional
            root.Attributes.Append(fOptional)

            'CSSClass
            Dim CSSClass As XmlAttribute = d.CreateAttribute("CSSClass")
            CSSClass.Value = Me.CSSClass
            root.Attributes.Append(CSSClass)

            'Width
            Dim Width As XmlAttribute = d.CreateAttribute("Width")
            Width.Value = Me.Width
            root.Attributes.Append(Width)

            'Height
            Dim Height As XmlAttribute = d.CreateAttribute("Height")
            Height.Value = Me.Height
            root.Attributes.Append(Height)

            'CustomRegex
            Dim CustomRegex As XmlAttribute = d.CreateAttribute("CustomRegex")
            CustomRegex.Value = Me.CustomRegex
            root.Attributes.Append(CustomRegex)

            'FormItemTitle
            Dim FormItemTitle As XmlAttribute = d.CreateAttribute("FormItemTitle")
            FormItemTitle.Value = Me.FormItemTitle
            root.Attributes.Append(FormItemTitle)

            'FormLabelClass
            Dim FormLabelClass As XmlAttribute = d.CreateAttribute("FormLabelClass")
            FormLabelClass.Value = Me.FormLabelClass
            root.Attributes.Append(FormLabelClass)

            'CultureCode
            Dim Culture As XmlAttribute = d.CreateAttribute("Culture")
            Culture.Value = cultureCode
            root.Attributes.Append(Culture)

            'AllowValueOverride
            Dim bAllowValueOverride As XmlAttribute = d.CreateAttribute("AllowValueOverride")
            bAllowValueOverride.Value = Me.AllowValueOverride
            root.Attributes.Append(bAllowValueOverride)

            'WebhookFieldName
            Dim WebhookFieldName As XmlAttribute = d.CreateAttribute("WebhookFieldName")
            WebhookFieldName.Value = Me.WebhookFieldName
            root.Attributes.Append(WebhookFieldName)

            'valore submission
            Dim valore As XmlElement = d.CreateElement("SubmissionValue")
            root.AppendChild(valore)

            '    MyLog(d.OuterXml)

            'Catch ex As Exception
            '    MyLog(ex.Message)
            '    MyLog(ex.StackTrace)
            'End Try

            Return d

        End Function

#End Region

        'Public Sub MyLog(ByVal logMessage As String)
        '    Dim w As IO.StreamWriter = IO.File.AppendText("E:\inetpub\wwwroot\stage.jacobacci.com\DesktopModules\Form\toxml.log")
        '    w.WriteLine("{0} {1} :{2}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString(), logMessage)
        '    w.Flush()
        '    w.Close()
        'End Sub

    End Class

End Namespace
