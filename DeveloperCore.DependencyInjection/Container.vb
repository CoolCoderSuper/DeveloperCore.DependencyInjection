Imports System.Collections.Concurrent
Imports System.Reflection

Public Class Container
    Public Shared Property [Shared] As New Container
    
    Private ReadOnly _services As New ConcurrentDictionary(Of Type, (Type As ServiceType, ServiceType As Type))
    Private ReadOnly _singletons As New ConcurrentDictionary(Of Type, Object)

    Public Sub AddSingleton(Of T)()
        _services.TryAdd(GetType(T), (ServiceType.Singleton, GetType(T)))
    End Sub

    Public Sub AddTransient(Of T)()
        _services.TryAdd(GetType(T), (ServiceType.Transient, GetType(T)))
    End Sub
    
    Public Sub AddScoped(Of T)()
        _services.TryAdd(GetType(T), (ServiceType.Scoped, GetType(T)))
    End Sub

    Public Sub AddSingleton(Of TService, TImplementation As TService)()
        _services.TryAdd(GetType(TService), (ServiceType.Singleton, GetType(TImplementation)))
    End Sub

    Public Sub AddTransient(Of TService, TImplementation As TService)()
        _services.TryAdd(GetType(TService), (ServiceType.Transient, GetType(TImplementation)))
    End Sub
    
    Public Sub AddScoped(Of TService, TImplementation As TService)()
        _services.TryAdd(GetType(TService), (ServiceType.Scoped, GetType(TImplementation)))
    End Sub

    Friend Function GetService(type As Type, scoped As Dictionary(Of Type, Object)) As Object
        Dim value As Object = Nothing
        If _services.ContainsKey(type) Then
            Dim service = _services(type)
            If service.Type = ServiceType.Transient Then
                value = Create(service.ServiceType, scoped)
            ElseIf service.Type = ServiceType.Scoped Then
                If scoped.ContainsKey(type) Then
                    value = scoped(type)
                Else
                    value = Create(service.ServiceType, scoped)
                    scoped.Add(type, value)
                End If
            ElseIf service.Type = ServiceType.Singleton Then
                If _singletons.ContainsKey(type) Then
                    value = _singletons(type)
                Else
                    value = Create(service.ServiceType, scoped)
                    _singletons.TryAdd(type, value)
                End If
            End If
        End If
        Return value
    End Function

    Friend Function Create(service As Type, scoped As Dictionary(Of Type, Object)) As Object
        Dim constructor As ConstructorInfo = service.GetConstructors().First()
        Dim parameters As ParameterInfo() = constructor.GetParameters()
        Dim parameterValues As List(Of Object) = parameters.Select(Function(x) GetService(x.ParameterType, scoped)).ToList()
        Dim instance As Object = Activator.CreateInstance(service, parameterValues.ToArray())
        Inject(instance)
        Return instance
    End Function
    
    Public Function Create(Of T) As T
        Return Create(GetType(T))
    End Function
    
    Public Function Create(service As Type) As Object
        Return Create(service, New Dictionary(Of Type, Object)())
    End Function
    
    Public Sub Inject(obj As Object)
        Dim t As Type = obj.GetType()
        For Each prop In t.GetProperties()
            Dim attr = prop.GetCustomAttributes(GetType(InjectAttribute), False)
            If attr.Length > 0 Then
                Dim scoped As New Dictionary(Of Type, Object)
                Dim propType As Type = prop.PropertyType
                Dim propValue As Object = GetService(propType, scoped)
                prop.SetValue(obj, propValue)
            End If
        Next
    End Sub
End Class