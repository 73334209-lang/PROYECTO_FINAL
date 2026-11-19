Imports System.Data.OleDb


Public Class FormMantenimientoPersonas
    Private idUsuarioSeleccionado As Integer = 0

    Private Sub ConfigurarCombos()

        cmbRol.DropDownStyle =
        ComboBoxStyle.DropDownList



    End Sub

    Private Sub FormMantenimientoPersonas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try

            ConfigurarUsuarios()
            CargarRoles()
            CargarEstadosUsuario()
            PrepararNuevoUsuario()
            CargarUsuarios()

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo cargar la sección de usuarios." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try


    End Sub


    Private Sub ConfigurarUsuarios()

        txtContraseña.UseSystemPasswordChar = True

        cmbRol.DropDownStyle =
            ComboBoxStyle.DropDownList

        cmbEstadoUsuario.DropDownStyle =
            ComboBoxStyle.DropDownList

        dgvUsuarios.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvUsuarios.MultiSelect = False
        dgvUsuarios.ReadOnly = True
        dgvUsuarios.AllowUserToAddRows = False
        dgvUsuarios.AllowUserToDeleteRows = False
        dgvUsuarios.AutoGenerateColumns = True
        dgvUsuarios.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

    End Sub
    Private Sub CargarRoles()

        cmbRol.Items.Clear()

        cmbRol.Items.Add("Administrador")
        cmbRol.Items.Add("Secretaría")
        cmbRol.Items.Add("Docente")
        cmbRol.Items.Add("Estudiante")

        cmbRol.SelectedIndex = -1

    End Sub

    Private Sub CargarEstadosUsuario()

        cmbEstadoUsuario.Items.Clear()

        cmbEstadoUsuario.Items.Add("Activo")
        cmbEstadoUsuario.Items.Add("Inactivo")

        cmbEstadoUsuario.SelectedIndex = 0

    End Sub

#End Region

#Region "Cargar usuarios"

    Private Sub CargarUsuarios()

        Try

            If Not ModuleConexion.ExisteBaseDatos() Then

                MessageBox.Show(
                    "No se encontró la base de datos en:" &
                    Environment.NewLine &
                    Environment.NewLine &
                    ModuleConexion.RutaBaseDatos,
                    "Base de datos no encontrada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )

                Return

            End If

            Dim tabla As New DataTable()

            Dim consulta As String =
                "SELECT IdUsuario, NombreUsuario, Rol, " &
                "Estado, FechaRegistro " &
                "FROM Usuarios " &
                "ORDER BY IdUsuario DESC"

            Using conexion As New OleDbConnection(
                ModuleConexion.CadenaConexion
            )

                Using adaptador As New OleDbDataAdapter(
                    consulta,
                    conexion
                )

                    adaptador.Fill(tabla)

                End Using

            End Using

            dgvUsuarios.DataSource = tabla

            FormatearTablaUsuarios()

            lblTotalUsuarios.Text =
                "Total de usuarios: " &
                tabla.Rows.Count.ToString()

        Catch ex As OleDbException

            MessageBox.Show(
                "Error al cargar los usuarios desde Access:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Error de Access",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        Catch ex As Exception

            MessageBox.Show(
                "Error al cargar los usuarios:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    Private Sub FormatearTablaUsuarios()

        If dgvUsuarios.Columns.Contains("IdUsuario") Then
            dgvUsuarios.Columns("IdUsuario").Visible = False
        End If

        If dgvUsuarios.Columns.Contains("NombreUsuario") Then
            dgvUsuarios.Columns("NombreUsuario").HeaderText =
                "Nombre de usuario"
        End If

        If dgvUsuarios.Columns.Contains("Rol") Then
            dgvUsuarios.Columns("Rol").HeaderText =
                "Rol"
        End If

        If dgvUsuarios.Columns.Contains("Estado") Then
            dgvUsuarios.Columns("Estado").HeaderText =
                "Estado"
        End If

        If dgvUsuarios.Columns.Contains("FechaRegistro") Then
            dgvUsuarios.Columns("FechaRegistro").HeaderText =
                "Fecha de registro"

            dgvUsuarios.Columns(
                "FechaRegistro"
            ).DefaultCellStyle.Format = "dd/MM/yyyy"
        End If

    End Sub

#End Region

#Region "Preparar y limpiar"

    Private Sub PrepararNuevoUsuario()

        idUsuarioSeleccionado = 0

        txtNombreusuario.Clear()
        txtContraseña.Clear()

        cmbRol.SelectedIndex = -1
        cmbEstadoUsuario.SelectedIndex = 0

        dtpFechaRegistro.Value = DateTime.Now

        txtNombreusuario.Focus()

    End Sub

    Private Sub LimpiarCamposUsuario()

        idUsuarioSeleccionado = 0

        txtNombreusuario.Clear()
        txtContraseña.Clear()

        cmbRol.SelectedIndex = -1
        cmbEstadoUsuario.SelectedIndex = 0

        dtpFechaRegistro.Value = DateTime.Now

        dgvUsuarios.ClearSelection()

        txtNombreusuario.Focus()

    End Sub

    Private Sub TabPage3_Click(sender As Object, e As EventArgs) Handles TabPage3.Click

    End Sub

    Private Sub Label13_Click(sender As Object, e As EventArgs) Handles Label13.Click, Label26.Click

    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub
End Class