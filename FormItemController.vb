Imports System
Imports System.Data
Imports DotNetNuke.Framework
Imports ODS.DNN.Modules.Form.Data
Imports DotNetNuke.Common.Utilities
Imports System.Collections
Imports System.Xml
Imports DotNetNuke.Instrumentation

Namespace ODS.DNN.Modules.Form.Business

    Public Class FormItemController
        Implements DotNetNuke.Entities.Modules.IPortable
        'Inherits DotNetNuke.Entities.Modules.ModuleSearchBase

#Region "Public Methods"
        Public Function [Get](ByVal formItemID As Integer) As FormItemInfo

            'Return CType(CBO.FillObject(DataProvider.Instance().GetODSFormItem(formItemID), GetType(FormItemInfo)), FormItemInfo)
            Return CBO.FillObject(Of FormItemInfo)(DataProvider.Instance().GetODSFormItem(formItemID))

        End Function

        Public Function List(ByVal portalid As Integer, ByVal moduleid As Integer, ByVal CultureCode As String) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().ListODSFormItem(portalid, moduleid, CultureCode), GetType(FormItemInfo))

        End Function

        Public Function Add(ByVal objODSFormItem As FormItemInfo) As Integer

            Return CType(DataProvider.Instance().AddODSFormItem(objODSFormItem.ModuleID, objODSFormItem.PortalID, objODSFormItem.FormType, objODSFormItem.FormValue, objODSFormItem.FormSelectedValue, objODSFormItem.FormLabel, objODSFormItem.SortValue, objODSFormItem.Optional, objODSFormItem.Width, objODSFormItem.Height, objODSFormItem.CSSClass, objODSFormItem.CustomRegex, objODSFormItem.FormItemTitle, objODSFormItem.FormLabelClass, objODSFormItem.Culture, objODSFormItem.AllowValueOverride, objODSFormItem.CustomData, objODSFormItem.WebhookFieldName), Integer)

        End Function

        Public Sub Update(ByVal objODSFormItem As FormItemInfo)

            DataProvider.Instance().UpdateODSFormItem(objODSFormItem.FormItemID, objODSFormItem.ModuleID, objODSFormItem.PortalID, objODSFormItem.FormType, objODSFormItem.FormValue, objODSFormItem.FormSelectedValue, objODSFormItem.FormLabel, objODSFormItem.Optional, objODSFormItem.Width, objODSFormItem.Height, objODSFormItem.CSSClass, objODSFormItem.CustomRegex, objODSFormItem.FormItemTitle, objODSFormItem.FormLabelClass, objODSFormItem.Culture, objODSFormItem.AllowValueOverride, objODSFormItem.CustomData, objODSFormItem.WebhookFieldName)

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
        'Public Overrides Function GetModifiedSearchDocuments(moduleInfo As Entities.Modules.ModuleInfo, beginDateUtc As Date) As IList(Of Search.Entities.SearchDocument)

        'End Function

        Public Function ExportModule(ModuleID As Integer) As String Implements Entities.Modules.IPortable.ExportModule

            DnnLogger.GetLogger("ODSForm").Debug("Export content of module " & ModuleID)

            Dim strXML As New StringBuilder()
            Dim settings As New XmlWriterSettings()
            settings.Indent = True
            settings.OmitXmlDeclaration = True
            Dim Writer As XmlWriter = XmlWriter.Create(strXML, settings)

            'read module settings
            Dim cultureCode As String = Nothing
            'Dim ModInfo As DotNetNuke.Entities.Modules.ModuleInfo = New DotNetNuke.Entities.Modules.ModuleController().GetModule(ModuleID, Nothing, True)
            Dim ModInfo As DotNetNuke.Entities.Modules.ModuleInfo = New DotNetNuke.Entities.Modules.ModuleController().GetModule(ModuleID)
            If ModInfo Is Nothing Then
                DnnLogger.GetLogger("ODSForm").Error("CANNOT GET MODULE " & ModuleID)
            End If
            Try
                Dim bLocalize As Boolean = CType(ModInfo.ModuleSettings("EnableLocalization"), Boolean)
                If bLocalize Then cultureCode = System.Threading.Thread.CurrentThread.CurrentCulture.ToString
            Catch ex As Exception
                DnnLogger.GetLogger("ODSForm").Error(ex.Message)
            End Try

            Dim oSCont As New FormItemController
            Dim items As ArrayList = oSCont.List(PortalSettings.Current.PortalId, ModuleID, cultureCode)

            DnnLogger.GetLogger("ODSForm").Debug("Export " & items.Count & " form items")
            If items.Count > 0 Then
                Writer.WriteStartElement("FormItems")
                Dim o As FormItemInfo
                For Each o In items

                    Writer.WriteStartElement("FormItem")
                    Writer.WriteAttributeString("FormItemID", o.FormItemID)
                    Writer.WriteAttributeString("Optional", o.Optional)
                    Writer.WriteAttributeString("FormType", o.FormType)
                    Writer.WriteAttributeString("FormLabel", o.FormLabel)
                    Writer.WriteAttributeString("SortValue", o.SortValue)
                    Writer.WriteAttributeString("Width", o.Width)
                    Writer.WriteAttributeString("Height", o.Height)
                    Writer.WriteAttributeString("CSSClass", o.CSSClass)
                    Writer.WriteAttributeString("FormLabelClass", o.FormLabelClass)
                    Writer.WriteAttributeString("FormItemTitle", o.FormItemTitle)
                    Writer.WriteAttributeString("Culture", o.Culture)
                    Writer.WriteAttributeString("AllowValueOverride", o.AllowValueOverride)

                    Writer.WriteElementString("FormValue", o.FormValue)
                    Writer.WriteElementString("FormSelectedValue", o.FormSelectedValue)
                    Writer.WriteElementString("CustomRegex", o.CustomRegex)
                    Writer.WriteElementString("CustomData", o.CustomData)
                    'WebhookFieldName
                    Writer.WriteElementString("WebhookFieldName", o.WebhookFieldName)

                    Writer.WriteEndElement()
                Next
                Writer.WriteEndElement()
                Writer.Close()

            End If
            Return strXML.ToString
        End Function

        Public Sub ImportModule(ModuleID As Integer, Content As String, Version As String, UserID As Integer) Implements Entities.Modules.IPortable.ImportModule

            Dim n As XmlNode
            Dim ns As XmlNode = DotNetNuke.Common.Globals.GetContent(Content, "FormItems")

            'DnnLogger.GetLogger("ODSForm").Debug("IMPORT " & ns.OuterXml)

            For Each n In ns.SelectNodes("FormItem")
                Dim f As New FormItemInfo
                f.ModuleID = ModuleID
                'DnnLogger.GetLogger("ODSForm").Debug("IMPORT formitem " & n.OuterXml)

                f.FormItemTitle = n.Attributes("FormItemTitle").Value
                f.Optional = n.Attributes("Optional").Value
                f.FormType = n.Attributes("FormType").Value
                f.FormLabel = n.Attributes("FormLabel").Value
                f.SortValue = n.Attributes("SortValue").Value
                f.Width = n.Attributes("Width").Value
                f.Height = n.Attributes("Height").Value
                f.CSSClass = n.Attributes("CSSClass").Value
                f.FormLabelClass = n.Attributes("FormLabelClass").Value
                f.FormItemTitle = n.Attributes("FormItemTitle").Value
                f.Culture = n.Attributes("Culture").Value
                f.AllowValueOverride = n.Attributes("AllowValueOverride").Value

                f.FormValue = n.Item("FormValue").InnerText
                f.FormSelectedValue = n.Item("FormSelectedValue").InnerText
                f.CustomRegex = n.Item("CustomRegex").InnerText
                f.CustomData = n.Item("CustomData").InnerText
                'WebhookFieldName
                f.WebhookFieldName = n.Item("WebhookFieldName").InnerText

                DnnLogger.GetLogger("ODSForm").Debug("ADD item w FormLabel=" & f.FormLabel)
                Add(f)


            Next
        End Sub

#End Region

        
    End Class

End Namespace
