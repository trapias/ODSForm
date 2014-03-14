
Imports System
Imports System.Data

Namespace ODS.DNN.Modules.Form.Business

    Public Class FormSubmissionInfo

#Region "Private Members"
        Private _formSubmissionID As Integer
        Private _moduleID As Integer
        Private _portalID As Integer
        Private _submissionDate As DateTime
        Private _submission As String
        Private _submissionxml As String
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub

        Public Sub New(ByVal formSubmissionID As Integer, ByVal moduleID As Integer, ByVal portalID As Integer, ByVal submissionDate As DateTime, ByVal submission As String, ByVal submissionxml As String)
            Me.FormSubmissionID = formSubmissionID
            Me.ModuleID = moduleID
            Me.PortalID = portalID
            Me.SubmissionDate = submissionDate
            Me.Submission = submission
            Me.SubmissionXML = submissionxml
        End Sub
#End Region

#Region "Public Properties"
        Public Property FormSubmissionID() As Integer
            Get
                Return _formSubmissionID
            End Get
            Set(ByVal Value As Integer)
                _formSubmissionID = Value
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

        Public Property SubmissionDate() As DateTime
            Get
                Return _submissionDate
            End Get
            Set(ByVal Value As DateTime)
                _submissionDate = Value
            End Set
        End Property

        Public Property Submission() As String
            Get
                Return _submission
            End Get
            Set(ByVal Value As String)
                _submission = Value
            End Set
        End Property

        Public Property SubmissionXML() As String
            Get
                Return _submissionxml
            End Get
            Set(ByVal Value As String)
                _submissionxml = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
