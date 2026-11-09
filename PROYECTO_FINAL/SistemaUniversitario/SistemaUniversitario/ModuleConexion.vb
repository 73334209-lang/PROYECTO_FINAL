Imports System.Data.OleDb
Module ModuleConexion
    Public ReadOnly CadenaConexion As String =
        "Provider=Microsoft.ACE.OLEDB.12.0;" &
        "Data Source=C:\Users\Usuario\OneDrive\Desktop\PROYECTO_FINAL\BDUniversidad.accdb;" &
        "Persist Security Info=False;"

    Public UsuarioActual As String = String.Empty
    Public RolActual As String = String.Empty
    Public IdUsuarioActual As Integer = 0
End Module
