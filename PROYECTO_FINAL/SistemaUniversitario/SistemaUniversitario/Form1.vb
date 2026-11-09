Public Class Form1
    Dim contadorPuntos As Integer = 0
    Private Sub FlowLayoutPanel1_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' Avanza la barra progresivamente
        If Guna2ProgressBar1.Value < 100 Then
            Guna2ProgressBar1.Value += 1
        Else
            ' Detiene el temporizador para no consumir recursos
            Timer1.Stop()

            ' Abre el siguiente formulario y oculta/cierra el Splash
            Dim siguienteForm As New FormLogin() ' Reemplaza Form2 por el nombre de tu formulario principal
            siguienteForm.Show()

            Me.Hide() ' Oculta el Splash Screen actual
        End If



    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        contadorPuntos += 1
        Select Case contadorPuntos Mod 4
            Case 0
                Guna2HtmlLabel1.Text = "Cargando módulos"
            Case 1
                Guna2HtmlLabel1.Text = "Cargando módulos."
            Case 2
                Guna2HtmlLabel1.Text = "Cargando módulos.."
            Case 3
                Guna2HtmlLabel1.Text = "Cargando módulos..."
        End Select

    End Sub
End Class
