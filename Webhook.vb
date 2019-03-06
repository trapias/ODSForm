Imports System.IO
Imports System.Net
Imports DotNetNuke.Instrumentation
Imports Microsoft.VisualBasic

Namespace ODS.DNN.Modules.Form
    Public Class Webhook

        Public Function Post(url As String, postData As String) As String
            Dim rc As String = String.Empty
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Dim request As WebRequest = WebRequest.Create(url)
            request.Method = "POST"
            'Dim postData As String = "This is a test that posts this string to a Web server."
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As WebResponse = request.GetResponse()
            LoggerSource.Instance.GetLogger("Webhook").Debug("Webhook Response: " & (CType(response, HttpWebResponse)).StatusCode & " = " & (CType(response, HttpWebResponse)).StatusDescription)
            dataStream = response.GetResponseStream()
            Dim reader As StreamReader = New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            'Console.WriteLine(responseFromServer)
            rc = responseFromServer
            reader.Close()
            dataStream.Close()
            response.Close()

            Return rc
        End Function
    End Class

End Namespace

