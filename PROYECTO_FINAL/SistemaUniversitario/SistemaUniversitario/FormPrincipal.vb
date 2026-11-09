Public Class FormPrincipal
    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles PanelContenido.Paint

    End Sub

    Private Sub InicioToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InicioToolStripMenuItem.Click
        PanelContenido.Visible = True
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
        lblUsuarioConectado.Text = "Bienvenido, " & ModuleConexion.RolActual

        ' Aplica la restricción de menús según el rol
        ConfigurarPermisos()

        LblFechaActual.Text =
       DateTime.Now.ToString(
           "ddd. dd 'de' MMMM 'de' yyyy"
       )
    End Sub
    Private Sub ConfigurarPermisos()
        Select Case ModuleConexion.RolActual
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
        End Select
    End Sub
End Class