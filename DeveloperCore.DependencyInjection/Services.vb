Imports System.Collections.Concurrent
Imports System.Reflection

Public Class Services
    Private Shared ReadOnly _services As New ConcurrentDictionary(Of Type, (Type As ServiceType, ServiceType As Type))
    Private Shared ReadOnly _singletons As New ConcurrentDictionary(Of Type, Object)

    Public Shared Sub AddSingleton(Of T)()
        _services.TryAdd(GetType(T), (ServiceType.Singleton, GetType(T)))
    End Sub

    Public Shared Sub AddTransient(Of T)()
        _services.TryAdd(GetType(T), (ServiceType.Transient, GetType(T)))
    End Sub
    
    Public Shared Sub AddScoped(Of T)()
        _services.TryAdd(GetType(T), (ServiceType.Scoped, GetType(T)))
    End Sub

    Public Shared Sub AddSingleton(Of TService, TImplementation As TService)()
        _services.TryAdd(GetType(TService), (ServiceType.Singleton, GetType(TImplementation)))
    End Sub

    Public Shared Sub AddTransient(Of TService, TImplementation As TService)()
        _services.TryAdd(GetType(TService), (ServiceType.Transient, GetType(TImplementation)))
    End Sub
    
    Public Shared Sub AddScoped(Of TService, TImplementation As TService)()
        _services.TryAdd(GetType(TService), (ServiceType.Scoped, GetType(TImplementation)))
    End Sub

    Friend Shared Function GetService(type As Type, scoped As Dictionary(Of Type, Object)) As Object
        Dim value As Object = Nothing
        If _services.ContainsKey(type) Then
            Dim service = _services(type)
            If service.Type = ServiceType.Transient Then
                value = CreateInstance(service.ServiceType, scoped)
            ElseIf service.Type = ServiceType.Scoped Then
                If scoped.ContainsKey(type) Then
                    value = scoped(type)
                Else
                    value = CreateInstance(service.ServiceType, scoped)
                    scoped.Add(type, value)
                End If
            ElseIf service.Type = ServiceType.Singleton Then
                If _singletons.ContainsKey(type) Then
                    value = _singletons(type)
                Else
                    value = CreateInstance(service.ServiceType, scoped)
                    _singletons.TryAdd(type, value)
                End If
            End If
        End If
        Return value
    End Function

    Friend Shared Function CreateInstance(service As Type, scoped As Dictionary(Of Type, Object)) As Object
        Dim constructor As ConstructorInfo = service.GetConstructors().First()
        Dim parameters As ParameterInfo() = constructor.GetParameters()
        Dim parameterValues As List(Of Object) = parameters.Select(Function(x) GetService(x.ParameterType, scoped)).ToList()
        Return Activator.CreateInstance(service, parameterValues.ToArray())
    End Function
    
    Public Shared Function CreateInstance(Of T) As T
        Return CreateInstance(GetType(T))
    End Function
    
    Public Shared Function CreateInstance(service As Type) As Object
        Return CreateInstance(service, New Dictionary(Of Type, Object)())
    End Function
End Class