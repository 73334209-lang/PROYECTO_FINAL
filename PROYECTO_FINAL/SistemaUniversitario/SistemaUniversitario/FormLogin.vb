Imports System.Data.OleDb

Public Class FormLogin

    Private Sub FormLogin_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        Guna2TextBox1.Clear()
        Guna2TextBox2.Clear()

        Guna2TextBox2.UseSystemPasswordChar = True

        Guna2TextBox1.Focus()

    End Sub

    Private Sub btnIngresar_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnIngresar.Click

        IniciarSesion()

    End Sub

    Private Sub IniciarSesion()

        Dim nombreUsuario As String =
            Guna2TextBox1.Text.Trim()

        Dim contraseña As String =
            Guna2TextBox2.Text.Trim()

        If String.IsNullOrWhiteSpace(nombreUsuario) OrElse
           String.IsNullOrWhiteSpace(contraseña) Then

            MessageBox.Show(
                "Por favor, ingrese usuario y contraseña.",
                "Campos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            If String.IsNullOrWhiteSpace(nombreUsuario) Then
                Guna2TextBox1.Focus()
            Else
                Guna2TextBox2.Focus()
            End If

            Return

        End If

        Dim consulta As String =
            "SELECT IdUsuario, NombreUsuario, Rol " &
            "FROM Usuarios " &
            "WHERE NombreUsuario = ? " &
            "AND [Contraseña] = ? " &
            "AND Estado = True"

        Try

            Using conexion As New OleDbConnection(
                ModuleConexion.CadenaConexion
            )

                Using comando As New OleDbCommand(
                    consulta,
                    conexion
                )

                    'OleDb utiliza los parámetros por posición.
                    comando.Parameters.Add(
                        "@usuario",
                        OleDbType.VarWChar
                    ).Value = nombreUsuario

                    comando.Parameters.Add(
                        "@contraseña",
                        OleDbType.VarWChar
                    ).Value = contraseña

                    conexion.Open()

                    Using lector As OleDbDataReader =
                        comando.ExecuteReader()

                        If lector.Read() Then

                            'Guardar la sesión en ModuleSesion.
                            ModuleSesion.IdUsuarioActual =
                                Convert.ToInt32(
                                    lector("IdUsuario")
                                )

                            ModuleSesion.NombreUsuarioActual =
                                lector("NombreUsuario").ToString()

                            ModuleSesion.RolUsuarioActual =
                                lector("Rol").ToString()

                            Dim formularioPrincipal As New FormPrincipal()


                            Me.Hide()

                            formularioPrincipal.Show()

                        Else

                            MessageBox.Show(
                                "Usuario o contraseña incorrectos, " &
                                "o el usuario está inactivo.",
                                "Error de autenticación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            )

                            Guna2TextBox2.Clear()
                            Guna2TextBox2.Focus()

                        End If

                    End Using

                End Using

            End Using

        Catch ex As OleDbException

            MessageBox.Show(
                "No se pudo acceder a la base de datos:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Error de Access",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        Catch ex As Exception

            MessageBox.Show(
                "Ocurrió un error inesperado:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    Private Sub Guna2TextBox2_IconRightClick(
        sender As Object,
        e As EventArgs
    ) Handles Guna2TextBox2.IconRightClick

        If Guna2TextBox2.UseSystemPasswordChar Then

            Guna2TextBox2.UseSystemPasswordChar = False

        Else

            Guna2TextBox2.UseSystemPasswordChar = True

        End If

    End Sub

    Private Sub FormLogin_FormClosed(
        sender As Object,
        e As FormClosedEventArgs
    ) Handles Me.FormClosed

        Application.Exit()

    End Sub

End Class
