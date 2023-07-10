Imports DeveloperCore.DependencyInjection

Public Module Program
    Public Sub Main()
        Container.Shared.AddTransient(Of ITest, ConsoleTest)()
        Container.Shared.AddScoped(Of Counter)()
        Dim thing As New Thing
        Container.Shared.Inject(thing)
        thing.Hi()
        Dim thing1 As New Thing
        Container.Shared.Inject(thing1)
        thing1.Hi()
    End Sub
End Module