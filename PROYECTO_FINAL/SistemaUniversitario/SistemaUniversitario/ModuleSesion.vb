Module ModuleSesion

    Public IdUsuarioActual As Integer = 0
    Public NombreUsuarioActual As String = String.Empty
    Public RolUsuarioActual As String = String.Empty

    Public Sub LimpiarSesion()

        IdUsuarioActual = 0
        NombreUsuarioActual = String.Empty
        RolUsuarioActual = String.Empty

    End Sub

End Module
