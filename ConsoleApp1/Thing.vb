Imports DeveloperCore.DependencyInjection

Public Class Thing
    <Inject>
    Public Property Counter As Counter
    Private ReadOnly _controller As ITest

    Public Sub New(controller As ITest)
        _controller = controller
    End Sub

    Public Sub Hi()
        _controller.Hi()
    End Sub
End Class