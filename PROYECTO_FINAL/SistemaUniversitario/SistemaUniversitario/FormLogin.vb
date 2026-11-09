Imports System.Data.OleDb
Public Class FormLogin

    Private Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click

        If String.IsNullOrWhiteSpace(Guna2TextBox1.Text) OrElse
            String.IsNullOrWhiteSpace(Guna2TextBox2.Text) Then

            MessageBox.Show(
                "Por favor, ingrese usuario y contraseña.",
                "Campos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Exit Sub
        End If

        Dim query As String =
            "SELECT IdUsuario, NombreUsuario, Rol " &
            "FROM Usuarios " &
            "WHERE NombreUsuario = ? " &
            "AND [Contraseña] = ? " &
            "AND Estado = True"

        Using conexion As New OleDbConnection(
            ModuleConexion.CadenaConexion)

            Try
                conexion.Open()

                Using cmd As New OleDbCommand(query, conexion)

                    'OleDb utiliza los parámetros según su posición.
                    cmd.Parameters.AddWithValue(
                        "@NombreUsuario",
                        Guna2TextBox1.Text.Trim()
                    )

                    cmd.Parameters.AddWithValue(
                        "@Contraseña",
                        Guna2TextBox2.Text
                    )

                    Using reader As OleDbDataReader =
                        cmd.ExecuteReader()

                        If reader.Read() Then

                            ModuleConexion.IdUsuarioActual =
                                Convert.ToInt32(
                                    reader("IdUsuario")
                                )

                            ModuleConexion.UsuarioActual =
                                reader("NombreUsuario").ToString()

                            ModuleConexion.RolActual =
                                reader("Rol").ToString()

                            Dim mainForm As New FormPrincipal()
                            mainForm.Show()

                            Me.Hide()

                        Else

                            MessageBox.Show(
                                "Usuario o contraseña incorrectos, " &
                                "o el usuario está inactivo.",
                                "Error de autenticación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            )

                            Guna2TextBox1.Clear()
                            Guna2TextBox2.Clear()
                            Guna2TextBox1.Focus()

                        End If

                    End Using
                End Using

            Catch ex As OleDbException

                MessageBox.Show(
                    "Error de Access: " & ex.Message,
                    "Error de consulta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )

            Catch ex As Exception

                MessageBox.Show(
                    "Error general: " & ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )

            End Try
        End Using

    End Sub




    Private Sub FormLogin_Click(sender As Object, e As EventArgs) Handles Me.Click

    End Sub

    Private Sub FormLogin_Closed(sender As Object, e As EventArgs) Handles Me.Closed

    End Sub

    Private Sub FormLogin_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()

    End Sub

    Private Sub Guna2TextBox2_IconRightClick(sender As Object, e As EventArgs) Handles Guna2TextBox2.IconRightClick
        ' Al dar clic: si está oculta se muestra, si está visible se oculta
        If Guna2TextBox2.UseSystemPasswordChar Then
            Guna2TextBox2.UseSystemPasswordChar = False
        Else
            Guna2TextBox2.UseSystemPasswordChar = True
        End If
    End Sub

    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles Me.Load
        Guna2TextBox2.UseSystemPasswordChar = True
    End Sub
End Class