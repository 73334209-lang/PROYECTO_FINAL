Imports System.Data.OleDb
Imports System.Globalization

Public Class FormPrincipal
    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles pnlContenido.Paint

    End Sub

    Private Sub InicioToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InicioToolStripMenuItem.Click
        pnlContenido.Visible = True
    End Sub

    Private Sub MantenimientoDePersonasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MantenimientoDePersonasToolStripMenuItem.Click
        Dim formulario As New FormMantenimientoPersonas()
        formulario.ShowDialog()

    End Sub

    Private Sub MantenimientoAcadémicoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MantenimientoAcadémicoToolStripMenuItem.Click
        Dim formulario As New FrmMantenimientoAcademico()
        formulario.ShowDialog()
    End Sub

    Private Sub GestiónDeHorariosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GestiónDeHorariosToolStripMenuItem.Click
        Dim formulario As New FrmHorarios()
        formulario.ShowDialog()
    End Sub

    Private Sub RegistroDeMatrículaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegistroDeMatrículaToolStripMenuItem.Click
        Dim formulario As New FrmMatricula()
        formulario.ShowDialog()
    End Sub

    Private Sub RegistroDePagosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegistroDePagosToolStripMenuItem.Click
        Dim formulario As New FrmPagos()
        formulario.ShowDialog()
    End Sub

    Private Sub ConstanciaDeMatrículaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConstanciaDeMatrículaToolStripMenuItem.Click
        Dim formulario As New FrmConstanciaMatricula()
        formulario.ShowDialog()
    End Sub

    Private Sub RegistroDeAsistenciaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegistroDeAsistenciaToolStripMenuItem.Click
        Dim formulario As New FrmAsistencia()
        formulario.ShowDialog()

    End Sub

    Private Sub RegistroDeCalificacionesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegistroDeCalificacionesToolStripMenuItem.Click
        Dim formulario As New FrmNotas()
        formulario.ShowDialog()
    End Sub

    Private Sub ConsultasGeneralesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConsultasGeneralesToolStripMenuItem.Click
        Dim formulario As New FrmConsultas()
        formulario.ShowDialog()

    End Sub

    Private Sub EstudiantesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles EstudiantesToolStripMenuItem1.Click
        Dim formulario As New FrmReportes()
        formulario.TipoReporte = "Estudiantes"
        formulario.ShowDialog()

    End Sub

    Private Sub FormPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try

            CargarDatosUsuario()
            CargarFechaActual()
            CargarIndicadores()
            ConfigurarPermisos()

        Catch ex As Exception

            MessageBox.Show(
                "Error al cargar el formulario principal:" &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub
    Private Sub ConfigurarPermisos()

        Select Case ModuleSesion.RolUsuarioActual

            Case "Administrador"

                PersonasToolStripMenuItem.Enabled = True
                AcadémicoToolStripMenuItem.Enabled = True
                ProcesosToolStripMenuItem.Enabled = True
                EvaluacionToolStripMenuItem.Enabled = True
                ConsultasToolStripMenuItem.Enabled = True
                ReportesToolStripMenuItem.Enabled = True

            Case "Secretaría"

                PersonasToolStripMenuItem.Enabled = True
                AcadémicoToolStripMenuItem.Enabled = True
                ProcesosToolStripMenuItem.Enabled = True
                EvaluacionToolStripMenuItem.Enabled = False
                ConsultasToolStripMenuItem.Enabled = True
                ReportesToolStripMenuItem.Enabled = True

            Case "Docente"

                PersonasToolStripMenuItem.Enabled = False
                AcadémicoToolStripMenuItem.Enabled = False
                ProcesosToolStripMenuItem.Enabled = False
                EvaluacionToolStripMenuItem.Enabled = True
                ConsultasToolStripMenuItem.Enabled = True
                ReportesToolStripMenuItem.Enabled = True

            Case "Estudiante"

                PersonasToolStripMenuItem.Enabled = False
                AcadémicoToolStripMenuItem.Enabled = False
                ProcesosToolStripMenuItem.Enabled = False
                EvaluacionToolStripMenuItem.Enabled = False
                ConsultasToolStripMenuItem.Enabled = True
                ReportesToolStripMenuItem.Enabled = False

            Case Else

                PersonasToolStripMenuItem.Enabled = False
                AcadémicoToolStripMenuItem.Enabled = False
                ProcesosToolStripMenuItem.Enabled = False
                EvaluacionToolStripMenuItem.Enabled = False
                ConsultasToolStripMenuItem.Enabled = False
                ReportesToolStripMenuItem.Enabled = False

        End Select

    End Sub







    Private Sub CargarDatosUsuario()

        If String.IsNullOrWhiteSpace(
        ModuleSesion.RolUsuarioActual
    ) Then

            lblUsuarioActual.Text =
            "Bienvenido, Usuario"

        Else

            lblUsuarioActual.Text =
            "Bienvenido, " &
            ModuleSesion.RolUsuarioActual

        End If

    End Sub


    Private Sub CargarFechaActual()

        Dim cultura As New CultureInfo("es-PE")

        LblFechaActual.Text =
            DateTime.Now.ToString(
                "ddd. dd 'de' MMMM 'de' yyyy",
                cultura
            )

    End Sub

    Private Sub CargarIndicadores()

        Try

            lblValorEstudiantes.Text =
                ObtenerCantidad(
                    "SELECT COUNT(*) FROM Estudiantes " &
                    "WHERE Estado = 'Activo'"
                ).ToString("N0")

            lblValorMatriculas.Text =
                ObtenerCantidad(
                    "SELECT COUNT(*) FROM Matriculas " &
                    "WHERE Estado = 'Confirmada'"
                ).ToString("N0")

            lblValorPagos.Text =
                ObtenerCantidad(
                    "SELECT COUNT(*) FROM Pagos " &
                    "WHERE Monto < 480"
                ).ToString("N0")

            lblValorCursos.Text =
                ObtenerCantidad(
                    "SELECT COUNT(*) FROM Cursos " &
                    "WHERE Estado = 'Activo'"
                ).ToString("N0")

            lblValorAsistencia.Text =
                ObtenerPromedioAsistencia().ToString("0") & "%"

            lblValorPromedio.Text =
                ObtenerPromedioAcademico().ToString("0.0")

        Catch ex As Exception

            MessageBox.Show(
                "No se pudieron cargar los indicadores." &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    Private Function ObtenerCantidad(
        consulta As String
    ) As Integer

        Using conexion As New OleDbConnection(
            ModuleConexion.CadenaConexion
        )

            Using comando As New OleDbCommand(
                consulta,
                conexion
            )

                conexion.Open()

                Return Convert.ToInt32(
                    comando.ExecuteScalar()
                )

            End Using

        End Using

    End Function

    Private Function ObtenerPromedioAcademico() As Double

        Using conexion As New OleDbConnection(
            ModuleConexion.CadenaConexion
        )

            Dim consulta As String =
                "SELECT AVG(Nota) FROM Notas"

            Using comando As New OleDbCommand(
                consulta,
                conexion
            )

                conexion.Open()

                Dim resultado As Object =
                    comando.ExecuteScalar()

                If resultado Is DBNull.Value OrElse
                   resultado Is Nothing Then

                    Return 0

                End If

                Return Convert.ToDouble(resultado)

            End Using

        End Using

    End Function

    Private Function ObtenerPromedioAsistencia() As Double

        Using conexion As New OleDbConnection(
            ModuleConexion.CadenaConexion
        )

            Dim consulta As String =
                "SELECT EstadoAsistencia FROM Asistencias"

            Using comando As New OleDbCommand(
                consulta,
                conexion
            )

                conexion.Open()

                Using lector As OleDbDataReader =
                    comando.ExecuteReader()

                    Dim total As Integer = 0
                    Dim presentes As Integer = 0

                    While lector.Read()

                        total += 1

                        If lector("EstadoAsistencia").ToString() =
                           "Presente" Then

                            presentes += 1

                        End If

                    End While

                    If total = 0 Then
                        Return 0
                    End If

                    Return (presentes * 100.0) / total

                End Using

            End Using

        End Using

    End Function


    Private Sub CerrarSesiónToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CerrarSesiónToolStripMenuItem.Click

        Dim respuesta As DialogResult

        respuesta = MessageBox.Show(
            "¿Está seguro de que desea cerrar sesión?",
            "Cerrar sesión",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        )

        If respuesta = DialogResult.Yes Then

            Me.Hide()

            Dim frm As New FormLogin()

            If frm.ShowDialog() = DialogResult.OK Then
                Me.Show()
            Else
                Application.Exit()
            End If

        End If
    End Sub

    Private Sub Guna2TileButton1_Click(sender As Object, e As EventArgs) Handles btnMantenimientoper.Click
        Try

            Using formulario As New FormMantenimientoPersonas()

                formulario.ShowDialog()

            End Using

        Catch ex As Exception

            MessageBox.Show(
            "No se pudo abrir el mantenimiento de personas." &
            Environment.NewLine &
            ex.Message,
            "Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

        End Try

    End Sub

    Private Sub btnMantenimientoAca_Click(sender As Object, e As EventArgs) Handles btnMantenimientoAca.Click
        Try

            Using formulario As New FrmMantenimientoAcademico()

                formulario.ShowDialog()

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el mantenimiento académico." &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try
    End Sub

    Private Sub btnHorarios_Click(sender As Object, e As EventArgs) Handles btnHorarios.Click

        Try

            Using formulario As New FrmHorarios()

                formulario.ShowDialog()

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el formulario de horarios." &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try
    End Sub

    Private Sub btnMatricula_Click(sender As Object, e As EventArgs) Handles btnMatricula.Click

        Try

            Using formulario As New FrmMatricula()

                formulario.ShowDialog()

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el formulario de matrícula." &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try
    End Sub

    Private Sub btnPagos_Click(sender As Object, e As EventArgs) Handles btnPagos.Click
        Try

            Using formulario As New FrmPagos()

                formulario.ShowDialog()

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el formulario de pagos." &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try
    End Sub

    Private Sub btnAsistencia_Click(sender As Object, e As EventArgs) Handles btnAsistencia.Click
        Try

            Using formulario As New FrmAsistencia()

                formulario.ShowDialog()

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el formulario de asistencia." &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try
    End Sub

    Private Sub btnCalificaciones_Click(sender As Object, e As EventArgs) Handles btnCalificaciones.Click
        Try

            Using formulario As New FrmNotas()

                formulario.ShowDialog()

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "No se pudo abrir el formulario de calificaciones." &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try
    End Sub
End Class