Imports System.Data.OleDb

Public Class FormMantenimientoPersonas
    Private idUsuarioSeleccionado As Integer = 0
    Private idEstudianteSeleccionado As Integer = 0
    Private idDocenteSeleccionado As Integer = 0

    Private Sub FormMantenimientoPersonas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 1. Usuarios
        Try
            ConfigurarUsuarios()
            CargarRoles()
            CargarEstadosUsuario()
            PrepararNuevoUsuario()
            CargarUsuarios()
        Catch ex As Exception
            MessageBox.Show(
                "No se pudo cargar la sección de usuarios." &
                Environment.NewLine & Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )
        End Try

        ' 2. Estudiantes
        CargarCarreras()
        ConfigurarEstudiantes()
        CargarEstudiantes()
        PrepararNuevoEstudiante()

        ' 3. Docentes
        ConfigurarDocentes()
        CargarDocentes()
        PrepararNuevoDocente()
    End Sub

#Region "1. Usuarios"

    Private Sub ConfigurarUsuarios()
        txtContraseña.UseSystemPasswordChar = True

        cmbRol.DropDownStyle = ComboBoxStyle.DropDownList
        cmbEstadoUsuario.DropDownStyle = ComboBoxStyle.DropDownList

        dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvUsuarios.MultiSelect = False
        dgvUsuarios.ReadOnly = True
        dgvUsuarios.AllowUserToAddRows = False
        dgvUsuarios.AllowUserToDeleteRows = False
        dgvUsuarios.AutoGenerateColumns = True
        dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
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

    Private Sub CargarUsuarios()
        Try
            If Not ModuleConexion.ExisteBaseDatos() Then
                MessageBox.Show(
                    "No se encontró la base de datos en:" &
                    Environment.NewLine & Environment.NewLine &
                    ModuleConexion.RutaBaseDatos,
                    "Base de datos no encontrada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )
                Return
            End If

            Dim tabla As New DataTable()

            Dim consulta As String =
                "SELECT IdUsuario, NombreUsuario, Rol, Estado, FechaRegistro " &
                "FROM Usuarios " &
                "ORDER BY IdUsuario DESC"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using adaptador As New OleDbDataAdapter(consulta, conexion)
                    adaptador.Fill(tabla)
                End Using
            End Using

            dgvUsuarios.DataSource = tabla
            FormatearTablaUsuarios()

            lblTotalUsuarios.Text = "Total de usuarios: " & tabla.Rows.Count.ToString()

        Catch ex As OleDbException
            MessageBox.Show(
                "Error al cargar los usuarios desde Access:" &
                Environment.NewLine & Environment.NewLine &
                ex.Message,
                "Error de Access",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )
        Catch ex As Exception
            MessageBox.Show(
                "Error al cargar los usuarios:" &
                Environment.NewLine & Environment.NewLine &
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
            dgvUsuarios.Columns("NombreUsuario").HeaderText = "Nombre de usuario"
        End If
        If dgvUsuarios.Columns.Contains("Rol") Then
            dgvUsuarios.Columns("Rol").HeaderText = "Rol"
        End If
        If dgvUsuarios.Columns.Contains("Estado") Then
            dgvUsuarios.Columns("Estado").HeaderText = "Estado"
        End If
        If dgvUsuarios.Columns.Contains("FechaRegistro") Then
            dgvUsuarios.Columns("FechaRegistro").HeaderText = "Fecha de registro"
            dgvUsuarios.Columns("FechaRegistro").DefaultCellStyle.Format = "dd/MM/yyyy"
        End If
    End Sub

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
        PrepararNuevoUsuario()
        dgvUsuarios.ClearSelection()
    End Sub

    Private Sub btnNuevoUsuario_Click(sender As Object, e As EventArgs) Handles btnNuevoUsuario.Click
        PrepararNuevoUsuario()
    End Sub

    Private Sub btnLimpiarUsuario_Click(sender As Object, e As EventArgs) Handles btnLimpiarUsuario.Click
        LimpiarCamposUsuario()
    End Sub

    Private Function ValidarUsuario(esModificacion As Boolean) As Boolean
        If String.IsNullOrWhiteSpace(txtNombreusuario.Text) Then
            MessageBox.Show("Ingrese el nombre de usuario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNombreusuario.Focus()
            Return False
        End If

        If Not esModificacion AndAlso String.IsNullOrWhiteSpace(txtContraseña.Text) Then
            MessageBox.Show("Ingrese la contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtContraseña.Focus()
            Return False
        End If

        If cmbRol.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione el rol del usuario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbRol.Focus()
            Return False
        End If

        If cmbEstadoUsuario.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione el estado del usuario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbEstadoUsuario.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub btnGuardarUsuario_Click(sender As Object, e As EventArgs) Handles btnGuardarUsuario.Click
        If Not ValidarUsuario(False) Then Return

        Try
            If ExisteNombreUsuario(txtNombreusuario.Text.Trim(), 0) Then
                MessageBox.Show("El nombre de usuario ya existe.", "Registro duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtNombreusuario.Focus()
                Return
            End If

            Dim estado As Boolean = (cmbEstadoUsuario.Text = "Activo")

            Dim consulta As String =
                "INSERT INTO Usuarios " &
                "(NombreUsuario, [Contraseña], Rol, Estado, FechaRegistro) " &
                "VALUES (?, ?, ?, ?, ?)"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using comando As New OleDbCommand(consulta, conexion)
                    comando.Parameters.Add("@NombreUsuario", OleDbType.VarWChar).Value = txtNombreusuario.Text.Trim()
                    comando.Parameters.Add("@Contraseña", OleDbType.VarWChar).Value = txtContraseña.Text
                    comando.Parameters.Add("@Rol", OleDbType.VarWChar).Value = cmbRol.Text
                    comando.Parameters.Add("@Estado", OleDbType.Boolean).Value = estado
                    comando.Parameters.Add("@FechaRegistro", OleDbType.Date).Value = dtpFechaRegistro.Value.Date

                    conexion.Open()
                    comando.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Usuario registrado correctamente.", "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarUsuarios()
            PrepararNuevoUsuario()

        Catch ex As OleDbException
            MessageBox.Show("No se pudo guardar el usuario:" & Environment.NewLine & Environment.NewLine & ex.Message, "Error de Access", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar el usuario:" & Environment.NewLine & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvUsuarios_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUsuarios.CellClick
        If e.RowIndex < 0 Then Return

        Try
            Dim fila As DataGridViewRow = dgvUsuarios.Rows(e.RowIndex)

            idUsuarioSeleccionado = Convert.ToInt32(fila.Cells("IdUsuario").Value)
            txtNombreusuario.Text = fila.Cells("NombreUsuario").Value.ToString()
            cmbRol.Text = fila.Cells("Rol").Value.ToString()

            Dim estado As Boolean = Convert.ToBoolean(fila.Cells("Estado").Value)
            cmbEstadoUsuario.Text = If(estado, "Activo", "Inactivo")

            If Not IsDBNull(fila.Cells("FechaRegistro").Value) Then
                dtpFechaRegistro.Value = Convert.ToDateTime(fila.Cells("FechaRegistro").Value)
            End If

            ' La contraseña no se carga por seguridad.
            txtContraseña.Clear()

        Catch ex As Exception
            MessageBox.Show("No se pudo seleccionar el usuario:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnModificarUsuario_Click(sender As Object, e As EventArgs) Handles btnModificarUsuario.Click
        If idUsuarioSeleccionado = 0 Then
            MessageBox.Show("Seleccione un usuario de la lista.", "Modificar usuario", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not ValidarUsuario(True) Then Return

        Try
            If ExisteNombreUsuario(txtNombreusuario.Text.Trim(), idUsuarioSeleccionado) Then
                MessageBox.Show("El nombre de usuario ya está siendo utilizado.", "Registro duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtNombreusuario.Focus()
                Return
            End If

            Dim estado As Boolean = (cmbEstadoUsuario.Text = "Activo")
            Dim consulta As String

            If String.IsNullOrWhiteSpace(txtContraseña.Text) Then
                consulta =
                    "UPDATE Usuarios SET " &
                    "NombreUsuario = ?, Rol = ?, Estado = ?, FechaRegistro = ? " &
                    "WHERE IdUsuario = ?"
            Else
                consulta =
                    "UPDATE Usuarios SET " &
                    "NombreUsuario = ?, [Contraseña] = ?, Rol = ?, Estado = ?, FechaRegistro = ? " &
                    "WHERE IdUsuario = ?"
            End If

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using comando As New OleDbCommand(consulta, conexion)
                    comando.Parameters.Add("@NombreUsuario", OleDbType.VarWChar).Value = txtNombreusuario.Text.Trim()

                    If Not String.IsNullOrWhiteSpace(txtContraseña.Text) Then
                        comando.Parameters.Add("@Contraseña", OleDbType.VarWChar).Value = txtContraseña.Text
                    End If

                    comando.Parameters.Add("@Rol", OleDbType.VarWChar).Value = cmbRol.Text
                    comando.Parameters.Add("@Estado", OleDbType.Boolean).Value = estado
                    comando.Parameters.Add("@FechaRegistro", OleDbType.Date).Value = dtpFechaRegistro.Value.Date
                    comando.Parameters.Add("@IdUsuario", OleDbType.Integer).Value = idUsuarioSeleccionado

                    conexion.Open()
                    comando.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Usuario modificado correctamente.", "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarUsuarios()
            PrepararNuevoUsuario()

        Catch ex As Exception
            MessageBox.Show("No se pudo modificar el usuario:" & Environment.NewLine & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnEliminarUsuario_Click(sender As Object, e As EventArgs) Handles btnEliminarUsuario.Click
        If idUsuarioSeleccionado = 0 Then
            MessageBox.Show("Seleccione un usuario de la lista.", "Eliminar usuario", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If idUsuarioSeleccionado = ModuleSesion.IdUsuarioActual Then
            MessageBox.Show("No puedes eliminar el usuario con el que estás conectado.", "Operación no permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("¿Desea eliminar el usuario seleccionado?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            Dim consulta As String = "DELETE FROM Usuarios WHERE IdUsuario = ?"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using comando As New OleDbCommand(consulta, conexion)
                    comando.Parameters.Add("@IdUsuario", OleDbType.Integer).Value = idUsuarioSeleccionado
                    conexion.Open()
                    comando.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Usuario eliminado correctamente.", "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarUsuarios()
            PrepararNuevoUsuario()

        Catch ex As OleDbException
            MessageBox.Show("No se puede eliminar el usuario." & Environment.NewLine & "Es posible que tenga información relacionada.", "Error de eliminación", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("No se pudo eliminar el usuario:" & Environment.NewLine & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnBuscarUsuario_Click(sender As Object, e As EventArgs) Handles btnBuscarUsuario.Click
        BuscarUsuarios()
    End Sub

    Private Sub txtBuscarUsuario_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBuscarUsuario.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            BuscarUsuarios()
        End If
    End Sub

    Private Sub txtBuscarUsuario_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarUsuario.TextChanged
        If String.IsNullOrWhiteSpace(txtBuscarUsuario.Text) Then
            CargarUsuarios()
        End If
    End Sub

    Private Sub BuscarUsuarios()
        Try
            Dim texto As String = txtBuscarUsuario.Text.Trim()
            Dim tabla As New DataTable()

            Dim consulta As String =
                "SELECT IdUsuario, NombreUsuario, Rol, Estado, FechaRegistro " &
                "FROM Usuarios " &
                "WHERE NombreUsuario LIKE ? OR Rol LIKE ? " &
                "ORDER BY IdUsuario DESC"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using comando As New OleDbCommand(consulta, conexion)
                    Dim criterio As String = "*" & texto & "*"
                    comando.Parameters.Add("@NombreUsuario", OleDbType.VarWChar).Value = criterio
                    comando.Parameters.Add("@Rol", OleDbType.VarWChar).Value = criterio

                    Using adaptador As New OleDbDataAdapter(comando)
                        adaptador.Fill(tabla)
                    End Using
                End Using
            End Using

            dgvUsuarios.DataSource = tabla
            FormatearTablaUsuarios()
            lblTotalUsuarios.Text = "Total de usuarios: " & tabla.Rows.Count.ToString()

        Catch ex As Exception
            MessageBox.Show("No se pudo realizar la búsqueda:" & Environment.NewLine & Environment.NewLine & ex.Message, "Error de búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function ExisteNombreUsuario(nombreUsuario As String, idExcluir As Integer) As Boolean
        Dim resultado As Integer = 0
        Dim consulta As String = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = ? AND IdUsuario <> ?"

        Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
            Using comando As New OleDbCommand(consulta, conexion)
                comando.Parameters.Add("@NombreUsuario", OleDbType.VarWChar).Value = nombreUsuario
                comando.Parameters.Add("@IdUsuario", OleDbType.Integer).Value = idExcluir
                conexion.Open()
                resultado = Convert.ToInt32(comando.ExecuteScalar())
            End Using
        End Using

        Return resultado > 0
    End Function

#End Region

#Region "2. Estudiantes"

    Private Sub ConfigurarEstudiantes()
        cmbCarreraEstudiante.DropDownStyle = ComboBoxStyle.DropDownList
        cmbEstadoEstudiante.DropDownStyle = ComboBoxStyle.DropDownList

        dgvEstudiantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvEstudiantes.MultiSelect = False
        dgvEstudiantes.ReadOnly = True
        dgvEstudiantes.AllowUserToAddRows = False
        dgvEstudiantes.AllowUserToDeleteRows = False
        dgvEstudiantes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        cmbEstadoEstudiante.Items.Clear()
        cmbEstadoEstudiante.Items.Add("Activo")
        cmbEstadoEstudiante.Items.Add("Inactivo")
    End Sub

    Private Sub CargarCarreras()
        Try
            Dim tabla As New DataTable()
            Dim consulta As String = "SELECT IdCarrera, NombreCarrera FROM Carreras ORDER BY NombreCarrera"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using adaptador As New OleDbDataAdapter(consulta, conexion)
                    adaptador.Fill(tabla)
                End Using
            End Using

            cmbCarreraEstudiante.DataSource = tabla
            cmbCarreraEstudiante.DisplayMember = "NombreCarrera"
            cmbCarreraEstudiante.ValueMember = "IdCarrera"
            cmbCarreraEstudiante.SelectedIndex = -1

        Catch ex As Exception
            MessageBox.Show("No se pudieron cargar las carreras:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CargarEstudiantes()
        Try
            Dim tabla As New DataTable()

            Dim consulta As String =
                "SELECT E.IdEstudiante, " &
                "E.CodigoEstudiante, E.DNI, " &
                "(E.ApellidoPaterno & ' ' & E.ApellidoMaterno) AS Apellidos, " &
                "E.Nombres, C.NombreCarrera, E.Telefono, " &
                "E.Correo, E.Estado " &
                "FROM Estudiantes AS E " &
                "LEFT JOIN Carreras AS C " &
                "ON E.IdCarrera = C.IdCarrera " &
                "ORDER BY E.IdEstudiante DESC"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using adaptador As New OleDbDataAdapter(consulta, conexion)
                    adaptador.Fill(tabla)
                End Using
            End Using

            dgvEstudiantes.DataSource = tabla

            If dgvEstudiantes.Columns.Contains("IdEstudiante") Then
                dgvEstudiantes.Columns("IdEstudiante").Visible = False
            End If
            If dgvEstudiantes.Columns.Contains("CodigoEstudiante") Then
                dgvEstudiantes.Columns("CodigoEstudiante").HeaderText = "Código"
            End If
            If dgvEstudiantes.Columns.Contains("NombreCarrera") Then
                dgvEstudiantes.Columns("NombreCarrera").HeaderText = "Carrera"
            End If

            lblEstudiantes.Text = "Total de estudiantes: " & tabla.Rows.Count.ToString()

        Catch ex As Exception
            lblEstudiantes.Text = "Total de estudiantes: 0"
            MessageBox.Show("No se pudieron cargar los estudiantes:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub

    Private Sub PrepararNuevoEstudiante()
        idEstudianteSeleccionado = 0
        txtCodigoEstudiante.Clear()
        txtDniEstudiante.Clear()
        txtApellidosEstudiante.Clear()
        txtNombresEstudiante.Clear()
        txtTelefonoEstudiante.Clear()
        txtCorreoEstudiante.Clear()
        cmbCarreraEstudiante.SelectedIndex = -1
        cmbEstadoEstudiante.SelectedIndex = 0
        dgvEstudiantes.ClearSelection()
    End Sub

    Private Function ValidarEstudiante() As Boolean
        If String.IsNullOrWhiteSpace(txtCodigoEstudiante.Text) Then
            MessageBox.Show("Ingrese el código del estudiante.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCodigoEstudiante.Focus()
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtDniEstudiante.Text) Then
            MessageBox.Show("Ingrese el DNI del estudiante.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDniEstudiante.Focus()
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtApellidosEstudiante.Text) Then
            MessageBox.Show("Ingrese los apellidos del estudiante.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtApellidosEstudiante.Focus()
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtNombresEstudiante.Text) Then
            MessageBox.Show("Ingrese los nombres del estudiante.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNombresEstudiante.Focus()
            Return False
        End If
        If cmbCarreraEstudiante.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione una carrera.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbCarreraEstudiante.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub btnNuevoEstudiante_Click(sender As Object, e As EventArgs) Handles btnNuevoEstudiante.Click
        PrepararNuevoEstudiante()
    End Sub

    Private Sub btnLimpiarEstu_Click(sender As Object, e As EventArgs) Handles btnLimpiarEstu.Click
        PrepararNuevoEstudiante()
    End Sub

    Private Sub btnGuardarEstudiante_Click(sender As Object, e As EventArgs) Handles btnGuardarEstudiante.Click
        If Not ValidarEstudiante() Then Return

        Dim partes() As String = txtApellidosEstudiante.Text.Trim().Split(New Char() {" "c}, 2, StringSplitOptions.RemoveEmptyEntries)
        Dim apellidoPat As String = partes(0)
        Dim apellidoMat As String = If(partes.Length > 1, partes(1), "")

        Try
            Dim consulta As String =
                "INSERT INTO Estudiantes " &
                "(CodigoEstudiante, DNI, ApellidoPaterno, ApellidoMaterno, Nombres, " &
                "IdCarrera, Telefono, Correo, Estado) " &
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using comando As New OleDbCommand(consulta, conexion)
                    comando.Parameters.AddWithValue("@p1", txtCodigoEstudiante.Text.Trim())
                    comando.Parameters.AddWithValue("@p2", txtDniEstudiante.Text.Trim())
                    comando.Parameters.AddWithValue("@p3", apellidoPat)
                    comando.Parameters.AddWithValue("@p4", apellidoMat)
                    comando.Parameters.AddWithValue("@p5", txtNombresEstudiante.Text.Trim())
                    comando.Parameters.AddWithValue("@p6", Convert.ToInt32(cmbCarreraEstudiante.SelectedValue))
                    comando.Parameters.AddWithValue("@p7", txtTelefonoEstudiante.Text.Trim())
                    comando.Parameters.AddWithValue("@p8", txtCorreoEstudiante.Text.Trim())
                    comando.Parameters.AddWithValue("@p9", cmbEstadoEstudiante.Text)

                    conexion.Open()
                    comando.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Estudiante registrado correctamente.", "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarEstudiantes()
            PrepararNuevoEstudiante()

        Catch ex As Exception
            MessageBox.Show("No se pudo guardar el estudiante:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvEstudiantes_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEstudiantes.CellClick
        If e.RowIndex < 0 Then Return

        Try
            Dim fila As DataGridViewRow = dgvEstudiantes.Rows(e.RowIndex)

            idEstudianteSeleccionado = Convert.ToInt32(fila.Cells("IdEstudiante").Value)
            txtCodigoEstudiante.Text = fila.Cells("CodigoEstudiante").Value.ToString()
            txtDniEstudiante.Text = fila.Cells("DNI").Value.ToString()
            txtApellidosEstudiante.Text = fila.Cells("Apellidos").Value.ToString()
            txtNombresEstudiante.Text = fila.Cells("Nombres").Value.ToString()
            cmbCarreraEstudiante.Text = fila.Cells("NombreCarrera").Value.ToString()
            txtTelefonoEstudiante.Text = fila.Cells("Telefono").Value.ToString()
            txtCorreoEstudiante.Text = fila.Cells("Correo").Value.ToString()
            cmbEstadoEstudiante.Text = fila.Cells("Estado").Value.ToString()
        Catch ex As Exception
            MessageBox.Show("No se pudo seleccionar el estudiante:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnModificarEstudiante_Click(sender As Object, e As EventArgs) Handles btnModificarEstudiante.Click
        If idEstudianteSeleccionado = 0 Then
            MessageBox.Show("Seleccione un estudiante de la lista.", "Modificar estudiante", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not ValidarEstudiante() Then Return

        Dim partes() As String = txtApellidosEstudiante.Text.Trim().Split(New Char() {" "c}, 2, StringSplitOptions.RemoveEmptyEntries)
        Dim apellidoPat As String = partes(0)
        Dim apellidoMat As String = If(partes.Length > 1, partes(1), "")

        Try
            Dim consulta As String =
                "UPDATE Estudiantes SET " &
                "CodigoEstudiante = ?, DNI = ?, ApellidoPaterno = ?, ApellidoMaterno = ?, " &
                "Nombres = ?, IdCarrera = ?, Telefono = ?, Correo = ?, Estado = ? " &
                "WHERE IdEstudiante = ?"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using comando As New OleDbCommand(consulta, conexion)
                    comando.Parameters.AddWithValue("@p1", txtCodigoEstudiante.Text.Trim())
                    comando.Parameters.AddWithValue("@p2", txtDniEstudiante.Text.Trim())
                    comando.Parameters.AddWithValue("@p3", apellidoPat)
                    comando.Parameters.AddWithValue("@p4", apellidoMat)
                    comando.Parameters.AddWithValue("@p5", txtNombresEstudiante.Text.Trim())
                    comando.Parameters.AddWithValue("@p6", Convert.ToInt32(cmbCarreraEstudiante.SelectedValue))
                    comando.Parameters.AddWithValue("@p7", txtTelefonoEstudiante.Text.Trim())
                    comando.Parameters.AddWithValue("@p8", txtCorreoEstudiante.Text.Trim())
                    comando.Parameters.AddWithValue("@p9", cmbEstadoEstudiante.Text)
                    comando.Parameters.AddWithValue("@p10", idEstudianteSeleccionado)

                    conexion.Open()
                    comando.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Estudiante modificado correctamente.", "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarEstudiantes()
            PrepararNuevoEstudiante()

        Catch ex As Exception
            MessageBox.Show("No se pudo modificar el estudiante:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnEliminarEstudiante_Click(sender As Object, e As EventArgs) Handles btnEliminarEstudiante.Click
        If idEstudianteSeleccionado = 0 Then
            MessageBox.Show("Seleccione un estudiante de la lista.", "Eliminar estudiante", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("¿Desea eliminar el estudiante seleccionado?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            Dim consulta As String = "DELETE FROM Estudiantes WHERE IdEstudiante = ?"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using comando As New OleDbCommand(consulta, conexion)
                    comando.Parameters.AddWithValue("@p1", idEstudianteSeleccionado)
                    conexion.Open()
                    comando.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Estudiante eliminado correctamente.", "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarEstudiantes()
            PrepararNuevoEstudiante()

        Catch ex As OleDbException
            MessageBox.Show("No se puede eliminar el estudiante." & Environment.NewLine & "Es posible que tenga matrículas o notas registradas.", "Error de eliminación", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("No se pudo eliminar el estudiante:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

#End Region

#Region "3. Docentes"

    Private Sub ConfigurarDocentes()
        cmbEspecialidad.DropDownStyle = ComboBoxStyle.DropDownList
        cmbGradoAcademico.DropDownStyle = ComboBoxStyle.DropDownList
        cmbEstadoDocente.DropDownStyle = ComboBoxStyle.DropDownList

        dgvDocentes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvDocentes.MultiSelect = False
        dgvDocentes.ReadOnly = True
        dgvDocentes.AllowUserToAddRows = False
        dgvDocentes.AllowUserToDeleteRows = False
        dgvDocentes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        cmbEspecialidad.Items.Clear()
        cmbEspecialidad.Items.Add("Ingeniería de Sistemas")
        cmbEspecialidad.Items.Add("Matemática")
        cmbEspecialidad.Items.Add("Comunicación")
        cmbEspecialidad.Items.Add("Contabilidad")
        cmbEspecialidad.Items.Add("Administración")

        cmbGradoAcademico.Items.Clear()
        cmbGradoAcademico.Items.Add("Bachiller")
        cmbGradoAcademico.Items.Add("Licenciado")
        cmbGradoAcademico.Items.Add("Magíster")
        cmbGradoAcademico.Items.Add("Doctor")

        cmbEstadoDocente.Items.Clear()
        cmbEstadoDocente.Items.Add("Activo")
        cmbEstadoDocente.Items.Add("Inactivo")
    End Sub

    Private Sub CargarDocentes()
        Try
            Dim tabla As New DataTable()

            Dim consulta As String =
                "SELECT IdDocente, DNI, Apellidos, Nombres, " &
                "Especialidad, GradoAcademico, Telefono, " &
                "Correo, Estado " &
                "FROM Docentes " &
                "ORDER BY IdDocente DESC"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using adaptador As New OleDbDataAdapter(consulta, conexion)
                    adaptador.Fill(tabla)
                End Using
            End Using

            dgvDocentes.DataSource = tabla

            If dgvDocentes.Columns.Contains("IdDocente") Then
                dgvDocentes.Columns("IdDocente").Visible = False
            End If
            If dgvDocentes.Columns.Contains("GradoAcademico") Then
                dgvDocentes.Columns("GradoAcademico").HeaderText = "Grado académico"
            End If
            lblDocentes.Text = "Total de docentes: " & tabla.Rows.Count.ToString()

        Catch ex As Exception
            lblDocentes.Text = "Total de docentes: 0"
            MessageBox.Show("No se pudieron cargar los docentes:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrepararNuevoDocente()
        idDocenteSeleccionado = 0
        txtDniDocente.Clear()
        txtApellidosDocente.Clear()
        txtNombresDocente.Clear()
        txtTelefonoDocente.Clear()
        txtCorreoDocente.Clear()
        cmbEspecialidad.SelectedIndex = -1
        cmbGradoAcademico.SelectedIndex = -1
        cmbEstadoDocente.SelectedIndex = 0
        dgvDocentes.ClearSelection()
    End Sub

    Private Function ValidarDocente() As Boolean
        If String.IsNullOrWhiteSpace(txtDniDocente.Text) Then
            MessageBox.Show("Ingrese el DNI del docente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDniDocente.Focus()
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtApellidosDocente.Text) Then
            MessageBox.Show("Ingrese los apellidos del docente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtApellidosDocente.Focus()
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtNombresDocente.Text) Then
            MessageBox.Show("Ingrese los nombres del docente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNombresDocente.Focus()
            Return False
        End If
        If cmbEspecialidad.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione la especialidad.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbEspecialidad.Focus()
            Return False
        End If
        If cmbGradoAcademico.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione el grado académico.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbGradoAcademico.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub btnNuevoDocent_Click(sender As Object, e As EventArgs) Handles btnNuevoDocent.Click
        PrepararNuevoDocente()
    End Sub

    Private Sub btnLimpiarDocent_Click(sender As Object, e As EventArgs) Handles btnLimpiarDocent.Click
        PrepararNuevoDocente()
    End Sub

    Private Sub btnGuardarDocent_Click(sender As Object, e As EventArgs) Handles btnGuardarDocent.Click
        If Not ValidarDocente() Then Return

        Try
            Dim consulta As String =
                "INSERT INTO Docentes " &
                "(DNI, Apellidos, Nombres, Especialidad, " &
                "GradoAcademico, Telefono, Correo, Estado) " &
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?)"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using comando As New OleDbCommand(consulta, conexion)
                    comando.Parameters.AddWithValue("@p1", txtDniDocente.Text.Trim())
                    comando.Parameters.AddWithValue("@p2", txtApellidosDocente.Text.Trim())
                    comando.Parameters.AddWithValue("@p3", txtNombresDocente.Text.Trim())
                    comando.Parameters.AddWithValue("@p4", cmbEspecialidad.Text)
                    comando.Parameters.AddWithValue("@p5", cmbGradoAcademico.Text)
                    comando.Parameters.AddWithValue("@p6", txtTelefonoDocente.Text.Trim())
                    comando.Parameters.AddWithValue("@p7", txtCorreoDocente.Text.Trim())
                    comando.Parameters.AddWithValue("@p8", cmbEstadoDocente.Text)

                    conexion.Open()
                    comando.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Docente registrado correctamente.", "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarDocentes()
            PrepararNuevoDocente()

        Catch ex As Exception
            MessageBox.Show("No se pudo guardar el docente:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvDocentes_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDocentes.CellClick
        If e.RowIndex < 0 Then Return

        Try
            Dim fila As DataGridViewRow = dgvDocentes.Rows(e.RowIndex)

            idDocenteSeleccionado = Convert.ToInt32(fila.Cells("IdDocente").Value)
            txtDniDocente.Text = fila.Cells("DNI").Value.ToString()
            txtApellidosDocente.Text = fila.Cells("Apellidos").Value.ToString()
            txtNombresDocente.Text = fila.Cells("Nombres").Value.ToString()
            cmbEspecialidad.Text = fila.Cells("Especialidad").Value.ToString()
            cmbGradoAcademico.Text = fila.Cells("GradoAcademico").Value.ToString()
            txtTelefonoDocente.Text = fila.Cells("Telefono").Value.ToString()
            txtCorreoDocente.Text = fila.Cells("Correo").Value.ToString()
            cmbEstadoDocente.Text = fila.Cells("Estado").Value.ToString()
        Catch ex As Exception
            MessageBox.Show("No se pudo seleccionar el docente:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnModificarDocent_Click(sender As Object, e As EventArgs) Handles btnModificarDocent.Click
        If idDocenteSeleccionado = 0 Then
            MessageBox.Show("Seleccione un docente de la lista.", "Modificar docente", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not ValidarDocente() Then Return

        Try
            Dim consulta As String =
                "UPDATE Docentes SET " &
                "DNI = ?, Apellidos = ?, Nombres = ?, " &
                "Especialidad = ?, GradoAcademico = ?, " &
                "Telefono = ?, Correo = ?, Estado = ? " &
                "WHERE IdDocente = ?"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using comando As New OleDbCommand(consulta, conexion)
                    comando.Parameters.AddWithValue("@p1", txtDniDocente.Text.Trim())
                    comando.Parameters.AddWithValue("@p2", txtApellidosDocente.Text.Trim())
                    comando.Parameters.AddWithValue("@p3", txtNombresDocente.Text.Trim())
                    comando.Parameters.AddWithValue("@p4", cmbEspecialidad.Text)
                    comando.Parameters.AddWithValue("@p5", cmbGradoAcademico.Text)
                    comando.Parameters.AddWithValue("@p6", txtTelefonoDocente.Text.Trim())
                    comando.Parameters.AddWithValue("@p7", txtCorreoDocente.Text.Trim())
                    comando.Parameters.AddWithValue("@p8", cmbEstadoDocente.Text)
                    comando.Parameters.AddWithValue("@p9", idDocenteSeleccionado)

                    conexion.Open()
                    comando.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Docente modificado correctamente.", "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarDocentes()
            PrepararNuevoDocente()

        Catch ex As Exception
            MessageBox.Show("No se pudo modificar el docente:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnEliminarDocent_Click(sender As Object, e As EventArgs) Handles btnEliminarDocent.Click
        If idDocenteSeleccionado = 0 Then
            MessageBox.Show("Seleccione un docente de la lista.", "Eliminar docente", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("¿Desea eliminar el docente seleccionado?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            Dim consulta As String = "DELETE FROM Docentes WHERE IdDocente = ?"

            Using conexion As New OleDbConnection(ModuleConexion.CadenaConexion)
                Using comando As New OleDbCommand(consulta, conexion)
                    comando.Parameters.AddWithValue("@p1", idDocenteSeleccionado)
                    conexion.Open()
                    comando.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Docente eliminado correctamente.", "Proceso completado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarDocentes()
            PrepararNuevoDocente()

        Catch ex As OleDbException
            MessageBox.Show("No se puede eliminar el docente." & Environment.NewLine & "Es posible que tenga cursos u horarios asignados.", "Error de eliminación", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("No se pudo eliminar el docente:" & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

#End Region

#Region "Cerrar formulario"

    Private Sub btnCerrarPersonas_Click(sender As Object, e As EventArgs) Handles btnCerrarPersonas.Click
        Me.Close()
    End Sub

#End Region

End Class