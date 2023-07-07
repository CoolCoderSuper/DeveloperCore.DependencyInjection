Public Class Counter
    Private _count As Integer

    Public ReadOnly Property Count As Integer
        Get
            Return _count
        End Get
    End Property
    
    Public Sub Increment()
        _count += 1
    End Sub
    
    Public Sub Decrement()
        _count -= 1
    End Sub
End Class