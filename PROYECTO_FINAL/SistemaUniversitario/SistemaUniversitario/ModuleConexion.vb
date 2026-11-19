Imports System.Data.OleDb
Imports System.IO

Module ModuleConexion

    Public ReadOnly RutaBaseDatos As String =
        "C:\PROYECTO_FINAL\BDUniversidad.accdb"

    Public ReadOnly CadenaConexion As String =
        "Provider=Microsoft.ACE.OLEDB.12.0;" &
        "Data Source=" & RutaBaseDatos & ";" &
        "Persist Security Info=False;"

    Public Function ExisteBaseDatos() As Boolean

        Return File.Exists(RutaBaseDatos)

    End Function

End Module
