Imports DeveloperCore.DependencyInjection

Public Module Program
    Public Sub Main()
        Container.Shared.AddTransient(Of ITest, ConsoleTest)()
        Container.Shared.AddScoped(Of Counter)()
        Dim thing As Thing = Container.Shared.Create(Of Thing)
        Container.Shared.Inject(thing)
        thing.Hi()
        Dim thing1 As Thing = Container.Shared.Create(Of Thing)
        thing1.Hi()
    End Sub
End Module