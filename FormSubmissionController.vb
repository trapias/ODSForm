Imports System
Imports System.Data
Imports DotNetNuke.Framework
Imports ODS.DNN.Modules.Form.Data

Namespace ODS.DNN.Modules.Form.Business

    Public Class FormSubmissionController
       
#Region "Public Methods"
        Public Function GetODSFormSubmission(ByVal formSubmissionID As Integer) As FormSubmissionInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetODSFormSubmission(formSubmissionID), GetType(FormSubmissionInfo)), FormSubmissionInfo)
        End Function

        Public Function ListODSFormSubmission(ByVal portalid As Integer, ByVal moduleid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().ListODSFormSubmission(portalid, moduleid), GetType(FormSubmissionInfo))
        End Function

        Public Function AddODSFormSubmission(ByVal objODSFormSubmission As FormSubmissionInfo) As Integer
            Return CType(DataProvider.Instance().AddODSFormSubmission(objODSFormSubmission.ModuleID, objODSFormSubmission.PortalID, objODSFormSubmission.SubmissionDate, objODSFormSubmission.Submission, objODSFormSubmission.SubmissionXML), Integer)
        End Function


        Public Sub DeleteODSFormSubmission(ByVal formSubmissionID As Integer)
            DataProvider.Instance().DeleteODSFormSubmission(formSubmissionID)
        End Sub

        '01.00.05
        Public Function SearchODSFormSubmission(ByVal portalid As Integer, ByVal moduleid As Integer, ByVal sSearch As String, ByVal iDisplayStart As Integer, ByVal iDisplayLength As Integer, ByVal iSortCol_0 As Integer, ByVal sSortDir_0 As String, ByRef NumRecords As Integer) As ArrayList

            Dim MaxRecords As New SqlParameter
            Dim elenco As ArrayList = CBO.FillCollection(DataProvider.Instance().SearchODSFormSubmission(portalid, moduleid, sSearch, iDisplayStart, iDisplayLength, iSortCol_0, sSortDir_0, MaxRecords), GetType(FormSubmissionInfo))
            NumRecords = MaxRecords.Value
            Return elenco

        End Function
        
#End Region

    End Class

End Namespace
