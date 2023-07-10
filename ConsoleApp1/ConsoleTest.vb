Imports DeveloperCore.DependencyInjection

Public Class ConsoleTest
    Implements ITest

    <Inject>
    Public Property Counter As Counter

    Public Sub Hi() Implements ITest.Hi
        Counter.Increment()
        Console.WriteLine("Hi")
        Console.WriteLine(Counter.Count)
    End Sub
End Class