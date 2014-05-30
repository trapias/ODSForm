Imports System
Imports System.Data
Imports DotNetNuke.Framework
Imports ODS.DNN.Modules.Form.Data
Imports DotNetNuke.Common.Utilities
Imports System.Collections

Namespace ODS.DNN.Modules.Form.Business

    Public Class FormItemController

#Region "Public Methods"
        Public Function [Get](ByVal formItemID As Integer) As FormItemInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetODSFormItem(formItemID), GetType(FormItemInfo)), FormItemInfo)

        End Function

        Public Function List(ByVal portalid As Integer, ByVal moduleid As Integer, ByVal CultureCode As String) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().ListODSFormItem(portalid, moduleid, CultureCode), GetType(FormItemInfo))

        End Function

        Public Function Add(ByVal objODSFormItem As FormItemInfo) As Integer

            Return CType(DataProvider.Instance().AddODSFormItem(objODSFormItem.ModuleID, objODSFormItem.PortalID, objODSFormItem.FormType, objODSFormItem.FormValue, objODSFormItem.FormSelectedValue, objODSFormItem.FormLabel, objODSFormItem.SortValue, objODSFormItem.Optional, objODSFormItem.Width, objODSFormItem.Height, objODSFormItem.CSSClass, objODSFormItem.CustomRegex, objODSFormItem.FormItemTitle, objODSFormItem.FormLabelClass, objODSFormItem.Culture, objODSFormItem.AllowValueOverride, objODSFormItem.CustomData), Integer)

        End Function

        Public Sub Update(ByVal objODSFormItem As FormItemInfo)

            DataProvider.Instance().UpdateODSFormItem(objODSFormItem.FormItemID, objODSFormItem.ModuleID, objODSFormItem.PortalID, objODSFormItem.FormType, objODSFormItem.FormValue, objODSFormItem.FormSelectedValue, objODSFormItem.FormLabel, objODSFormItem.Optional, objODSFormItem.Width, objODSFormItem.Height, objODSFormItem.CSSClass, objODSFormItem.CustomRegex, objODSFormItem.FormItemTitle, objODSFormItem.FormLabelClass, objODSFormItem.Culture, objODSFormItem.AllowValueOverride, objODSFormItem.CustomData)

        End Sub

        Public Sub Delete(ByVal formItemID As Integer)

            DataProvider.Instance().DeleteODSFormItem(formItemID)

        End Sub

        Public Sub MoveUp(ByVal formitemid As Integer)
            DataProvider.Instance.ODSFormItemMoveUp(formitemid)
        End Sub

        Public Sub MoveDown(ByVal formitemid As Integer)
            DataProvider.Instance.ODSFormItemMoveDown(formitemid)
        End Sub

#End Region

#Region "Optional Interfaces"

#End Region

    End Class

End Namespace
