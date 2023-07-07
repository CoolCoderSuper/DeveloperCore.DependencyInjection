Imports System.Runtime.CompilerServices

Public Module ServiceExtensions
    <Extension>
    Public Sub CreateServices(obj As Object)
        Dim t As Type = obj.GetType()
        For Each prop In t.GetProperties()
            Dim attr = prop.GetCustomAttributes(GetType(InjectAttribute), False)
            If attr.Length > 0 Then
                Dim scoped As New Dictionary(Of Type, Object)
                Dim propType As Type = prop.PropertyType
                Dim propValue As Object = Services.GetService(propType, scoped)
                prop.SetValue(obj, propValue)
            End If
        Next
    End Sub
End Module