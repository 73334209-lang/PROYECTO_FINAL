Imports System.ComponentModel
Public Class FrmReportes
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property TipoReporte As String
    Private Sub FrmReportes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case TipoReporte
            Case "Estudiantes"
                ' Cargar datos, ajustar ComboBox o cambiar títulos para estudiantes
                Me.Text = "Reporte General de Estudiantes"
                cmbTipoReporte.SelectedItem = "Estudiantes"

            Case "Matrículas"
                Me.Text = "Reporte de Matrículas"
                cmbTipoReporte.SelectedItem = "Matrículas"

            Case "Pagos"
                Me.Text = "Reporte de Pagos"
                cmbTipoReporte.SelectedItem = "Pagos"
        End Select

    End Sub
End Class