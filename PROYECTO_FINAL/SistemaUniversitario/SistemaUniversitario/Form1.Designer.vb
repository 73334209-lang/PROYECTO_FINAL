<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Label1 = New Label()
        Guna2ProgressBar1 = New Guna.UI2.WinForms.Guna2ProgressBar()
        Timer1 = New Timer(components)
        Panel1 = New Panel()
        Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Timer2 = New Timer(components)
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.None
        Label1.AutoSize = True
        Label1.BackColor = Color.Transparent
        Label1.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.ForeColor = Color.MidnightBlue
        Label1.Location = New Point(259, 23)
        Label1.Name = "Label1"
        Label1.Size = New Size(379, 92)
        Label1.TabIndex = 0
        Label1.Text = "SISTEMA DE GESTIÓN " & vbCrLf & "        ACADÉMICA"
        ' 
        ' Guna2ProgressBar1
        ' 
        Guna2ProgressBar1.Anchor = AnchorStyles.None
        Guna2ProgressBar1.BackColor = Color.Transparent
        Guna2ProgressBar1.BorderRadius = 11
        Guna2ProgressBar1.CustomizableEdges = CustomizableEdges1
        Guna2ProgressBar1.FillColor = Color.LightSkyBlue
        Guna2ProgressBar1.Location = New Point(74, 317)
        Guna2ProgressBar1.Name = "Guna2ProgressBar1"
        Guna2ProgressBar1.ProgressColor = Color.FromArgb(CByte(10), CByte(45), CByte(140))
        Guna2ProgressBar1.ProgressColor2 = Color.FromArgb(CByte(25), CByte(90), CByte(210))
        Guna2ProgressBar1.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        Guna2ProgressBar1.Size = New Size(779, 25)
        Guna2ProgressBar1.TabIndex = 2
        Guna2ProgressBar1.Text = "Guna2ProgressBar1"
        Guna2ProgressBar1.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault
        ' 
        ' Timer1
        ' 
        Timer1.Enabled = True
        Timer1.Interval = 50
        ' 
        ' Panel1
        ' 
        Panel1.Anchor = AnchorStyles.None
        Panel1.AutoScroll = True
        Panel1.BackColor = Color.Transparent
        Panel1.Controls.Add(Guna2HtmlLabel1)
        Panel1.Controls.Add(Guna2ProgressBar1)
        Panel1.Controls.Add(Label1)
        Panel1.Location = New Point(182, 374)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(922, 373)
        Panel1.TabIndex = 3
        ' 
        ' Guna2HtmlLabel1
        ' 
        Guna2HtmlLabel1.BackColor = Color.Transparent
        Guna2HtmlLabel1.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2HtmlLabel1.ForeColor = SystemColors.Highlight
        Guna2HtmlLabel1.Location = New Point(360, 259)
        Guna2HtmlLabel1.Name = "Guna2HtmlLabel1"
        Guna2HtmlLabel1.Size = New Size(183, 30)
        Guna2HtmlLabel1.TabIndex = 4
        Guna2HtmlLabel1.Text = "Cargando módulos..."
        ' 
        ' Timer2
        ' 
        Timer2.Enabled = True
        Timer2.Interval = 300
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(120F, 120F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), Image)
        BackgroundImageLayout = ImageLayout.Stretch
        ClientSize = New Size(1273, 747)
        Controls.Add(Panel1)
        DoubleBuffered = True
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Form1"
        WindowState = FormWindowState.Maximized
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Guna2ProgressBar1 As Guna.UI2.WinForms.Guna2ProgressBar
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Timer2 As Timer

End Class
