Public Class ConsoleTest
    Implements ITest

    Private ReadOnly _counter As Counter

    Public Sub New(counter As Counter)
        _counter = counter
    End Sub

    Public Sub Hi() Implements ITest.Hi
        _counter.Increment()
        Console.WriteLine("Hi")
        Console.WriteLine(_counter.Count)
    End Sub
End Class