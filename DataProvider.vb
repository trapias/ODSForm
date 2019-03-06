Imports System
Imports System.Web.Caching
Imports System.Reflection

Namespace ODS.DNN.Modules.Form.Data

    Public MustInherit Class DataProvider

#Region "Shared/Static Methods"
        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("data", "ODS.DNN.Modules.Form.Data", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function
#End Region
        '---------------------------------------------------------------------
        ' TODO Declare DAL methods. Should be implemented in each DAL DataProvider
        ' Use CodeSmith templates to generate this code
        '---------------------------------------------------------------------

#Region "ODSFormItem Methods"
        Public MustOverride Function GetODSFormItem(ByVal formItemID As Integer) As IDataReader
        Public MustOverride Function ListODSFormItem(ByVal portalid As Integer, ByVal moduleid As Integer, ByVal CultureCode As String) As IDataReader
        Public MustOverride Function AddODSFormItem(ByVal moduleID As Integer, ByVal portalID As Integer, ByVal formType As Integer, ByVal formValue As String, ByVal formSelectedValue As String, ByVal formLabel As String, ByVal sortValue As Integer, ByVal [optional] As Boolean, ByVal width As Integer, ByVal height As Integer, ByVal CSSClass As String, ByVal CustomRegex As String, ByVal FormItemTitle As String, ByVal FormLabelClass As String, ByVal CultureCode As String, ByVal AllowValueOverride As Boolean, ByVal CustomData As String, ByVal WebhookFieldName As String) As Integer
        Public MustOverride Sub UpdateODSFormItem(ByVal formItemID As Integer, ByVal moduleID As Integer, ByVal portalID As Integer, ByVal formType As Integer, ByVal formValue As String, ByVal formSelectedValue As String, ByVal formLabel As String, ByVal [optional] As Boolean, ByVal width As Integer, ByVal height As Integer, ByVal CSSClass As String, ByVal CustomRegex As String, ByVal FormItemTitle As String, ByVal FormLabelClass As String, ByVal CultureCode As String, ByVal AllowValueOverride As Boolean, ByVal CustomData As String, ByVal WebhookFieldName As String)
        Public MustOverride Sub DeleteODSFormItem(ByVal formItemID As Integer)
        Public MustOverride Sub ODSFormItemMoveUp(ByVal formItemID As Integer)
        Public MustOverride Sub ODSFormItemMoveDown(ByVal formItemID As Integer)
#End Region

#Region "ODSFormSubmission Methods"
        Public MustOverride Function GetODSFormSubmission(ByVal formSubmissionID As Integer) As IDataReader
        Public MustOverride Function ListODSFormSubmission(ByVal portalid As Integer, ByVal moduleid As Integer) As IDataReader
        Public MustOverride Function AddODSFormSubmission(ByVal moduleID As Integer, ByVal portalID As Integer, ByVal submissionDate As DateTime, ByVal submission As String, ByVal submissionXML As String) As Integer
        Public MustOverride Sub DeleteODSFormSubmission(ByVal formSubmissionID As Integer)
        '01.00.05
        Public MustOverride Function SearchODSFormSubmission(ByVal portalid As Integer, ByVal moduleid As Integer, ByVal sSearch As String, ByVal iDisplayStart As Integer, ByVal iDisplayLength As Integer, ByVal iSortCol_0 As Integer, ByVal sSortDir_0 As String, ByRef MaxRecords As SqlParameter) As IDataReader
#End Region

    End Class

End Namespace
