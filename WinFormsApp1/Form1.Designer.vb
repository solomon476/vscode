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
        txtUsername = New TextBox()
        txtPassword = New TextBox()
        btnLogin = New TextBox()
        SuspendLayout()
        ' 
        ' txtUsername
        ' 
        txtUsername.Location = New Point(369, 90)
        txtUsername.Name = "txtUsername"
        txtUsername.Size = New Size(150, 31)
        txtUsername.TabIndex = 1
        txtUsername.Text = "admin"
        ' 
        ' txtPassword
        ' 
        txtPassword.Location = New Point(822, 91)
        txtPassword.Name = "txtPassword"
        txtPassword.PasswordChar = "*"c
        txtPassword.Size = New Size(150, 31)
        txtPassword.TabIndex = 3
        txtPassword.Text = "admin123"
        ' 
        ' btnLogin
        ' 
        btnLogin.Location = New Point(580, 248)
        btnLogin.Name = "btnLogin"
        btnLogin.Size = New Size(150, 31)
        btnLogin.TabIndex = 4
        btnLogin.Text = "Login"
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1344, 450)
        Controls.Add(btnLogin)
        Controls.Add(txtPassword)
        Controls.Add(txtUsername)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents txtUsername As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents btnLogin As TextBox

End Class
