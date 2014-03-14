Imports Microsoft.VisualBasic
Imports System.Web.UI.WebControls

Namespace ODS.DNN.Modules.Form

    Public Class CheckBoxListValidator
        Inherits BaseValidator

        Protected Overrides Function EvaluateIsValid() As Boolean
            Dim isValid As Boolean = False

            Dim cb As CheckBoxList = FindControl(Me.ControlToValidate)
            For Each li As ListItem In cb.Items
                If li.Selected = True Then
                    isValid = True
                End If
            Next

            Return isValid

        End Function

        Protected Overrides Function ControlPropertiesValid() As Boolean
            If Me.ControlToValidate <> String.Empty Then
                Return True
            Else
                Return False
            End If

        End Function
    End Class

    ''' <summary>
    ''' '01.00.03: Checkbox validator
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CheckBoxValidator
        Inherits BaseValidator

        Protected Overrides Function EvaluateIsValid() As Boolean
            Dim isValid As Boolean = False

            Dim cb As CheckBox = FindControl(Me.ControlToValidate)
            isValid = cb.Checked

            Return isValid

        End Function

        Protected Overrides Function ControlPropertiesValid() As Boolean
            If Me.ControlToValidate <> String.Empty Then
                Return True
            Else
                Return False
            End If

        End Function
    End Class

End Namespace