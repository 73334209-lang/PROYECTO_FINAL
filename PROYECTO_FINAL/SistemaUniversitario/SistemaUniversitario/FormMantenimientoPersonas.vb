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


#Region "Configuración inicial"

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

    Private Sub btnNuevoUsuario_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnNuevoUsuario.Click

        PrepararNuevoUsuario()

    End Sub

    Private Sub btnLimpiarUsuario_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnLimpiarUsuario.Click

        LimpiarCamposUsuario()

    End Sub

#End Region

#Region "Validación"

    Private Function ValidarUsuario(
        esModificacion As Boolean
    ) As Boolean

        If String.IsNullOrWhiteSpace(
            txtNombreusuario.Text
        ) Then

            MessageBox.Show(
                "Ingrese el nombre de usuario.",
                "Validación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtNombreusuario.Focus()
            Return False

        End If

        If Not esModificacion AndAlso
           String.IsNullOrWhiteSpace(
               txtContraseña.Text
           ) Then

            MessageBox.Show(
                "Ingrese la contraseña.",
                "Validación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtContraseña.Focus()
            Return False

        End If

        If cmbRol.SelectedIndex = -1 Then

            MessageBox.Show(
                "Seleccione el rol del usuario.",
                "Validación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cmbRol.Focus()
            Return False

        End If

        If cmbEstadoUsuario.SelectedIndex = -1 Then

            MessageBox.Show(
                "Seleccione el estado del usuario.",
                "Validación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cmbEstadoUsuario.Focus()
            Return False

        End If

        Return True

    End Function

#End Region

#Region "Guardar usuario"

    Private Sub btnGuardarUsuario_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnGuardarUsuario.Click

        If Not ValidarUsuario(False) Then
            Return
        End If

        Try

            If ExisteNombreUsuario(
                txtNombreusuario.Text.Trim(),
                0
            ) Then

                MessageBox.Show(
                    "El nombre de usuario ya existe.",
                    "Registro duplicado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                txtNombreusuario.Focus()
                Return

            End If

            Dim estado As Boolean =
                cmbEstadoUsuario.Text = "Activo"

            Dim consulta As String =
                "INSERT INTO Usuarios " &
                "(NombreUsuario, [Contraseña], Rol, Estado, FechaRegistro) " &
                "VALUES (?, ?, ?, ?, ?)"

            Using conexion As New OleDbConnection(
                ModuleConexion.CadenaConexion
            )

                Using comando As New OleDbCommand(
                    consulta,
                    conexion
                )

                    comando.Parameters.Add(
                        "@NombreUsuario",
                        OleDbType.VarWChar
                    ).Value =
                        txtNombreusuario.Text.Trim()

                    comando.Parameters.Add(
                        "@Contraseña",
                        OleDbType.VarWChar
                    ).Value =
                        txtContraseña.Text

                    comando.Parameters.Add(
                        "@Rol",
                        OleDbType.VarWChar
                    ).Value =
                        cmbRol.Text

                    comando.Parameters.Add(
                        "@Estado",
                        OleDbType.Boolean
                    ).Value =
                        estado

                    comando.Parameters.Add(
                        "@FechaRegistro",
                        OleDbType.Date
                    ).Value =
                        dtpFechaRegistro.Value

                    conexion.Open()
                    comando.ExecuteNonQuery()

                End Using

            End Using

            MessageBox.Show(
                "Usuario registrado correctamente.",
                "Proceso completado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            CargarUsuarios()
            PrepararNuevoUsuario()

        Catch ex As OleDbException

            MessageBox.Show(
                "No se pudo guardar el usuario:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Error de Access",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        Catch ex As Exception

            MessageBox.Show(
                "Ocurrió un error al guardar el usuario:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

#End Region

#Region "Seleccionar usuario"

    Private Sub dgvUsuarios_CellClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    ) Handles dgvUsuarios.CellClick

        If e.RowIndex < 0 Then
            Return
        End If

        Try

            Dim fila As DataGridViewRow =
                dgvUsuarios.Rows(e.RowIndex)

            idUsuarioSeleccionado =
                Convert.ToInt32(
                    fila.Cells("IdUsuario").Value
                )

            txtNombreusuario.Text =
                fila.Cells("NombreUsuario").Value.ToString()

            cmbRol.Text =
                fila.Cells("Rol").Value.ToString()

            Dim estado As Boolean =
                Convert.ToBoolean(
                    fila.Cells("Estado").Value
                )

            If estado Then
                cmbEstadoUsuario.Text = "Activo"
            Else
                cmbEstadoUsuario.Text = "Inactivo"
            End If

            If Not IsDBNull(
                fila.Cells("FechaRegistro").Value
            ) Then

                dtpFechaRegistro.Value =
                    Convert.ToDateTime(
                        fila.Cells("FechaRegistro").Value
                    )

            End If

            'La contraseña no se carga por seguridad.
            txtContraseña.Clear()

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo seleccionar el usuario:" &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

#End Region

#Region "Modificar usuario"

    Private Sub btnModificarUsuario_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnModificarUsuario.Click

        If idUsuarioSeleccionado = 0 Then

            MessageBox.Show(
                "Seleccione un usuario de la lista.",
                "Modificar usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If

        If Not ValidarUsuario(True) Then
            Return
        End If

        Try

            If ExisteNombreUsuario(
                txtNombreusuario.Text.Trim(),
                idUsuarioSeleccionado
            ) Then

                MessageBox.Show(
                    "El nombre de usuario ya está siendo utilizado.",
                    "Registro duplicado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                txtNombreusuario.Focus()
                Return

            End If

            Dim estado As Boolean =
                cmbEstadoUsuario.Text = "Activo"

            Dim consulta As String

            If String.IsNullOrWhiteSpace(
                txtContraseña.Text
            ) Then

                consulta =
                    "UPDATE Usuarios SET " &
                    "NombreUsuario = ?, Rol = ?, Estado = ?, " &
                    "FechaRegistro = ? " &
                    "WHERE IdUsuario = ?"

            Else

                consulta =
                    "UPDATE Usuarios SET " &
                    "NombreUsuario = ?, [Contraseña] = ?, " &
                    "Rol = ?, Estado = ?, FechaRegistro = ? " &
                    "WHERE IdUsuario = ?"

            End If

            Using conexion As New OleDbConnection(
                ModuleConexion.CadenaConexion
            )

                Using comando As New OleDbCommand(
                    consulta,
                    conexion
                )

                    comando.Parameters.Add(
                        "@NombreUsuario",
                        OleDbType.VarWChar
                    ).Value =
                        txtNombreusuario.Text.Trim()

                    If Not String.IsNullOrWhiteSpace(
                        txtContraseña.Text
                    ) Then

                        comando.Parameters.Add(
                            "@Contraseña",
                            OleDbType.VarWChar
                        ).Value =
                            txtContraseña.Text

                    End If

                    comando.Parameters.Add(
                        "@Rol",
                        OleDbType.VarWChar
                    ).Value =
                        cmbRol.Text

                    comando.Parameters.Add(
                        "@Estado",
                        OleDbType.Boolean
                    ).Value =
                        estado

                    comando.Parameters.Add(
                        "@FechaRegistro",
                        OleDbType.Date
                    ).Value =
                        dtpFechaRegistro.Value

                    comando.Parameters.Add(
                        "@IdUsuario",
                        OleDbType.Integer
                    ).Value =
                        idUsuarioSeleccionado

                    conexion.Open()
                    comando.ExecuteNonQuery()

                End Using

            End Using

            MessageBox.Show(
                "Usuario modificado correctamente.",
                "Proceso completado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            CargarUsuarios()
            PrepararNuevoUsuario()

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo modificar el usuario:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

#End Region

#Region "Eliminar usuario"

    Private Sub btnEliminarUsuario_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnEliminarUsuario.Click

        If idUsuarioSeleccionado = 0 Then

            MessageBox.Show(
                "Seleccione un usuario de la lista.",
                "Eliminar usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If

        If idUsuarioSeleccionado =
           ModuleSesion.IdUsuarioActual Then

            MessageBox.Show(
                "No puedes eliminar el usuario con el que estás conectado.",
                "Operación no permitida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If

        Dim respuesta As DialogResult =
            MessageBox.Show(
                "¿Desea eliminar el usuario seleccionado?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )

        If respuesta <> DialogResult.Yes Then
            Return
        End If

        Try

            Dim consulta As String =
                "DELETE FROM Usuarios " &
                "WHERE IdUsuario = ?"

            Using conexion As New OleDbConnection(
                ModuleConexion.CadenaConexion
            )

                Using comando As New OleDbCommand(
                    consulta,
                    conexion
                )

                    comando.Parameters.Add(
                        "@IdUsuario",
                        OleDbType.Integer
                    ).Value =
                        idUsuarioSeleccionado

                    conexion.Open()
                    comando.ExecuteNonQuery()

                End Using

            End Using

            MessageBox.Show(
                "Usuario eliminado correctamente.",
                "Proceso completado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            CargarUsuarios()
            PrepararNuevoUsuario()

        Catch ex As OleDbException

            MessageBox.Show(
                "No se puede eliminar el usuario." &
                Environment.NewLine &
                "Es posible que tenga información relacionada.",
                "Error de eliminación",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo eliminar el usuario:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

#End Region

#Region "Buscar usuarios"

    Private Sub btnBuscarUsuario_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnBuscarUsuario.Click

        BuscarUsuarios()

    End Sub

    Private Sub txtBuscarUsuario_KeyDown(
        sender As Object,
        e As KeyEventArgs
    ) Handles txtBuscarUsuario.KeyDown

        If e.KeyCode = Keys.Enter Then

            e.SuppressKeyPress = True
            BuscarUsuarios()

        End If

    End Sub

    Private Sub BuscarUsuarios()

        Try

            Dim texto As String =
                txtBuscarUsuario.Text.Trim()

            Dim tabla As New DataTable()

            Dim consulta As String =
                "SELECT IdUsuario, NombreUsuario, Rol, " &
                "Estado, FechaRegistro " &
                "FROM Usuarios " &
                "WHERE NombreUsuario LIKE ? " &
                "OR Rol LIKE ? " &
                "ORDER BY IdUsuario DESC"

            Using conexion As New OleDbConnection(
                ModuleConexion.CadenaConexion
            )

                Using comando As New OleDbCommand(
                    consulta,
                    conexion
                )

                    Dim criterio As String =
                        "*" & texto & "*"

                    comando.Parameters.Add(
                        "@NombreUsuario",
                        OleDbType.VarWChar
                    ).Value = criterio

                    comando.Parameters.Add(
                        "@Rol",
                        OleDbType.VarWChar
                    ).Value = criterio

                    Using adaptador As New OleDbDataAdapter(
                        comando
                    )

                        adaptador.Fill(tabla)

                    End Using

                End Using

            End Using

            dgvUsuarios.DataSource = tabla

            FormatearTablaUsuarios()

            lblTotalUsuarios.Text =
                "Total de usuarios: " &
                tabla.Rows.Count.ToString()

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo realizar la búsqueda:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Error de búsqueda",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    Private Sub txtBuscarUsuario_TextChanged(
        sender As Object,
        e As EventArgs
    ) Handles txtBuscarUsuario.TextChanged

        If String.IsNullOrWhiteSpace(
            txtBuscarUsuario.Text
        ) Then

            CargarUsuarios()

        End If

    End Sub

#End Region

#Region "Funciones auxiliares"

    Private Function ExisteNombreUsuario(
        nombreUsuario As String,
        idExcluir As Integer
    ) As Boolean

        Dim resultado As Integer = 0

        Dim consulta As String =
            "SELECT COUNT(*) FROM Usuarios " &
            "WHERE NombreUsuario = ? " &
            "AND IdUsuario <> ?"

        Using conexion As New OleDbConnection(
            ModuleConexion.CadenaConexion
        )

            Using comando As New OleDbCommand(
                consulta,
                conexion
            )

                comando.Parameters.Add(
                    "@NombreUsuario",
                    OleDbType.VarWChar
                ).Value = nombreUsuario

                comando.Parameters.Add(
                    "@IdUsuario",
                    OleDbType.Integer
                ).Value = idExcluir

                conexion.Open()

                resultado =
                    Convert.ToInt32(
                        comando.ExecuteScalar()
                    )

            End Using

        End Using

        Return resultado > 0

    End Function

#End Region

#Region "Cerrar formulario"

    Private Sub btnCerrarPersonas_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnCerrarPersonas.Click

        Me.Close()

    End Sub

#End Region
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