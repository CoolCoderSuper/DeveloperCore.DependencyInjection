Imports DeveloperCore.DependencyInjection

Public Class Thing
    <Inject>
    Public Property Controller As ITest

    Public Sub Hi()
        Controller.Hi()
    End Sub
End Class