Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke
Imports DotNetNuke.Instrumentation

Namespace ODS.DNN.Modules.Form.Data

    Public Class SqlDataProvider

        Inherits DataProvider

#Region "Private Members"
		Private Const ProviderType As String = "data"
		Private _providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
		Private _connectionString As String
		Private _providerPath As String
		Private _objectQualifier As String
		Private _databaseOwner As String
#End Region

#Region "Constructors"
		Public Sub New()

			' Read the configuration specific information for this provider
			Dim objProvider As Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Framework.Providers.Provider)

			' Read the attributes for this provider
            If objProvider.Attributes("connectionStringName") <> "" AndAlso _
            System.Configuration.ConfigurationManager.AppSettings(objProvider.Attributes("connectionStringName")) <> "" Then
                _connectionString = System.Configuration.ConfigurationManager.AppSettings(objProvider.Attributes("connectionStringName"))
            Else
                _connectionString = objProvider.Attributes("connectionString")
            End If

			_providerPath = objProvider.Attributes("providerPath")

			_objectQualifier = objProvider.Attributes("objectQualifier")
			If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
				_objectQualifier += "_"
			End If

			_databaseOwner = objProvider.Attributes("databaseOwner")
			If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
				_databaseOwner += "."
			End If

		End Sub
#End Region

#Region "Properties"
		Public ReadOnly Property ConnectionString() As String
			Get
				Return _connectionString
			End Get
		End Property

		Public ReadOnly Property ProviderPath() As String
			Get
				Return _providerPath
			End Get
		End Property

		Public ReadOnly Property ObjectQualifier() As String
			Get
				Return _objectQualifier
			End Get
		End Property

		Public ReadOnly Property DatabaseOwner() As String
			Get
				Return _databaseOwner
			End Get
		End Property
#End Region

#Region "General Public Methods"
		Private Function GetNull(ByVal Field As Object) As Object
			Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

        Private Function GetFullyQualifiedName(ByVal name As String) As String
            Return DatabaseOwner & ObjectQualifier & name
        End Function
#End Region

#Region "ODSFormItem Methods"
        Public Overrides Function GetODSFormItem(ByVal formItemID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormItemGet", formItemID), IDataReader)
        End Function

        Public Overrides Function ListODSFormItem(ByVal portalID As Integer, ByVal moduleid As Integer, ByVal CultureCode As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormItemList", New Object() {portalID, moduleid, CultureCode}), IDataReader)
        End Function

        Public Overrides Function AddODSFormItem(ByVal moduleID As Integer, ByVal portalID As Integer, ByVal formType As Integer, ByVal formValue As String, ByVal formSelectedValue As String, ByVal formLabel As String, ByVal sortValue As Integer, ByVal [optional] As Boolean, ByVal width As Integer, ByVal height As Integer, ByVal CSSClass As String, ByVal CustomRegex As String, ByVal FormItemTitle As String, ByVal FormLabelClass As String, ByVal CultureCode As String, ByVal AllowValueOverride As Boolean, ByVal CustomData As string) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormItemAdd", moduleID, portalID, formType, GetNull(formValue), GetNull(formSelectedValue), GetNull(formLabel), sortValue, [optional], width, height, CSSClass, CustomRegex, FormItemTitle, FormLabelClass, CultureCode, AllowValueOverride, CustomData), Integer)
        End Function

        Public Overrides Sub UpdateODSFormItem(ByVal formItemID As Integer, ByVal moduleID As Integer, ByVal portalID As Integer, ByVal formType As Integer, ByVal formValue As String, ByVal formSelectedValue As String, ByVal formLabel As String, ByVal [optional] As Boolean, ByVal width As Integer, ByVal height As Integer, ByVal CSSClass As String, ByVal CustomRegex As String, ByVal FormItemTitle As String, ByVal FormLabelClass As String, ByVal CultureCode As String, ByVal AllowValueOverride As Boolean, ByVal CustomData As String)
            'dnnlog.debug(formLabel)
            'DnnLog.Debug(GetNull(formLabel))
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormItemUpdate", formItemID, moduleID, portalID, formType, GetNull(formValue), GetNull(formSelectedValue), GetNull(formLabel), [optional], width, height, CSSClass, CustomRegex, FormItemTitle, FormLabelClass, CultureCode, AllowValueOverride, CustomData)
        End Sub

        Public Overrides Sub DeleteODSFormItem(ByVal formItemID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormItemDelete", formItemID)
        End Sub

        Public Overrides Sub ODSFormItemMoveUp(ByVal formitemid As Integer)
            SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormItemMoveUp", formitemid)
        End Sub

        Public Overrides Sub ODSFormItemMoveDown(ByVal formitemid As Integer)
            SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormItemMoveDown", formitemid)
        End Sub

#End Region

#Region "ODSFormSubmission Methods"
        Public Overrides Function GetODSFormSubmission(ByVal formSubmissionID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormSubmissionGet", formSubmissionID), IDataReader)
        End Function

        Public Overrides Function ListODSFormSubmission(ByVal portalid As Integer, ByVal moduleid As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormSubmissionList", portalid, moduleid), IDataReader)
        End Function

        
        Public Overrides Function AddODSFormSubmission(ByVal moduleID As Integer, ByVal portalID As Integer, ByVal submissionDate As DateTime, ByVal submission As String, ByVal submissionXML As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormSubmissionAdd", moduleID, portalID, submissionDate, submission, submissionXML), Integer)
        End Function

        Public Overrides Sub DeleteODSFormSubmission(ByVal formSubmissionID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormSubmissionDelete", formSubmissionID)
        End Sub

        '01.00.05
        Public Overrides Function SearchODSFormSubmission(ByVal portalid As Integer, ByVal moduleid As Integer, ByVal sSearch As String, ByVal iDisplayStart As Integer, ByVal iDisplayLength As Integer, ByVal iSortCol_0 As Integer, ByVal sSortDir_0 As String, ByRef MaxRecords As SqlParameter) As IDataReader

            Dim conn As SqlConnection = New SqlConnection(ConnectionString)
            conn.Open()

            Dim cmd As New SqlCommand(GetFullyQualifiedName("ODSFormSubmissionSearch"), conn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim PPortalID As New SqlParameter("@PortalID", portalid)
            Dim PModuleID As New SqlParameter("@ModuleID", moduleid)
            Dim PsSearch As New SqlParameter("@sSearch", sSearch)
            Dim PiDisplayStart As New SqlParameter("@iDisplayStart", iDisplayStart)
            Dim PiDisplayLength As New SqlParameter("@iDisplayLength", iDisplayLength)
            Dim PiSortCol_0 As New SqlParameter("@iSortCol_0", iSortCol_0)
            Dim PsSortDir_0 As New SqlParameter("@sSortDir_0", sSortDir_0)
            MaxRecords.Direction = ParameterDirection.InputOutput
            MaxRecords.DbType = SqlDbType.Int
            MaxRecords.ParameterName = "@MaxRecords"
            MaxRecords.Value = 0
            MaxRecords.DbType = SqlDbType.Int

            cmd.Parameters.Add(PPortalID)
            cmd.Parameters.Add(PModuleID)
            cmd.Parameters.Add(PsSearch)
            cmd.Parameters.Add(PiDisplayStart)
            cmd.Parameters.Add(PiDisplayLength)
            cmd.Parameters.Add(PiSortCol_0)
            cmd.Parameters.Add(PsSortDir_0)
            cmd.Parameters.Add(MaxRecords)

            Dim dr As IDataReader = cmd.ExecuteReader

            Return CType(dr, IDataReader)

            'Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "ODSFormSubmissionSearch", portalid, moduleid, sSearch, iDisplayStart, iDisplayLength, iSortCol_0, sSortDir_0, MaxRecords), IDataReader)

        End Function

#End Region


    End Class

End Namespace