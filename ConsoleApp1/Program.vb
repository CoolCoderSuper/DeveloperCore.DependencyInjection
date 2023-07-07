Imports DeveloperCore.DependencyInjection

Public Module Program
    Public Sub Main()
        Services.AddTransient(Of ITest, ConsoleTest)()
        Services.AddScoped(Of Counter)()
        Dim thing As New Thing
        thing.CreateServices()
        thing.Hi()
        Dim thing1 As New Thing
        thing1.CreateServices()
        thing1.Hi()
    End Sub
End Module