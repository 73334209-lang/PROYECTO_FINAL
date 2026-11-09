Module ModuleSesion
    Public IdUsuarioActual As Integer
    Public NombreUsuarioActual As String
    Public RolUsuarioActual As String

    Public Sub LimpiarSesion()
        IdUsuarioActual = 0
        NombreUsuarioActual = String.Empty
        RolUsuarioActual = String.Empty
    End Sub
End Module
