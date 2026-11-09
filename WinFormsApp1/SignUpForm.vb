Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.IO

Public Class SignUpForm
    Inherits Form

    Public Property UserRole As String = "Admin"
    Public Property LoggedInUser As UserAccount

    Private txtUser As TextBox
    Private txtPass As TextBox
    Private cmbRole As ComboBox
    Private btnLogin As Button
    Private btnSignUp As Button
    Public Sub New()
        Me.Text = "Pamoja Rail Services - Secure Portal"
        Me.Size = New Size(920, 620)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.DoubleBuffered = True
        Me.BackColor = Color.White
    End Sub

    Private Sub SignUpForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BuildControls()
    End Sub

    Private Sub BuildControls()
        Me.Controls.Clear()

        Dim cardLeft As Integer = 285
        Dim cardTop As Integer = 65

        ' Role ComboBox
        cmbRole = New ComboBox() With {
            .Location = New Point(cardLeft + 35, cardTop + 140),
            .Size = New Size(280, 32),
            .Font = New Font("Segoe UI", 11, FontStyle.Bold),
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .BackColor = Color.White,
            .ForeColor = Color.FromArgb(15, 23, 42)
        }
        cmbRole.Items.Add("Administrator")
        cmbRole.Items.Add("Member Portal")
        cmbRole.SelectedIndex = 0
        AddHandler cmbRole.SelectedIndexChanged, Sub()
            UserRole = If(cmbRole.SelectedIndex = 0, "Admin", "Member")
            btnLogin.Text = "Sign In as " & If(UserRole = "Admin", "Administrator", "Member")
        End Sub

        ' Username Field
        txtUser = New TextBox() With {
            .Location = New Point(cardLeft + 35, cardTop + 205),
            .Size = New Size(280, 32),
            .Font = New Font("Segoe UI", 11),
            .BackColor = Color.White,
            .ForeColor = Color.FromArgb(15, 23, 42),
            .BorderStyle = BorderStyle.FixedSingle
        }

        ' Password Field
        txtPass = New TextBox() With {
            .Location = New Point(cardLeft + 35, cardTop + 270),
            .Size = New Size(280, 32),
            .Font = New Font("Segoe UI", 11),
            .PasswordChar = "*"c,
            .BackColor = Color.White,
            .ForeColor = Color.FromArgb(15, 23, 42),
            .BorderStyle = BorderStyle.FixedSingle
        }

        ' Inverted Login Button: Pure White with Pure Red text
        btnLogin = New Button() With {
            .Text = "Sign In as Administrator",
            .Location = New Point(cardLeft + 35, cardTop + 335),
            .Size = New Size(280, 48),
            .Font = New Font("Segoe UI", 11, FontStyle.Bold),
            .BackColor = Color.White,
            .ForeColor = Color.FromArgb(230, 0, 0),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnLogin.FlatAppearance.BorderColor = Color.FromArgb(245, 158, 11)
        btnLogin.FlatAppearance.BorderSize = 2
        AddHandler btnLogin.Click, AddressOf BtnLogin_Click

        ' Register link button
        btnSignUp = New Button() With {
            .Text = "New passenger? Register Member Account",
            .Location = New Point(cardLeft + 35, cardTop + 395),
            .Size = New Size(280, 32),
            .Font = New Font("Segoe UI", 9, FontStyle.Underline),
            .BackColor = Color.Transparent,
            .ForeColor = Color.FromArgb(254, 240, 138),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnSignUp.FlatAppearance.BorderSize = 0
        btnSignUp.FlatAppearance.MouseDownBackColor = Color.Transparent
        btnSignUp.FlatAppearance.MouseOverBackColor = Color.Transparent
        AddHandler btnSignUp.Click, AddressOf BtnSignUp_Click

        Me.Controls.Add(cmbRole)
        Me.Controls.Add(txtUser)
        Me.Controls.Add(txtPass)
        Me.Controls.Add(btnLogin)
        Me.Controls.Add(btnSignUp)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Background: Crisp luminous white overlay over rail photo
        Dim imgPath As String = Path.Combine(Application.StartupPath, "download.jpg")
        If File.Exists(imgPath) Then
            Using trainImg = Image.FromFile(imgPath)
                g.DrawImage(trainImg, Me.ClientRectangle)
            End Using
        End If

        ' Crisp White Glass Overlay
        Using whiteOverlay As New SolidBrush(Color.FromArgb(240, 255, 255, 255))
            g.FillRectangle(whiteOverlay, Me.ClientRectangle)
        End Using

        ' Brand Badge (Pure Red with Gold text)
        Dim badgeRect As New Rectangle(335, 18, 250, 36)
        Using badgeBrush As New SolidBrush(Color.FromArgb(230, 0, 0))
            Using path = GetRoundedRectPath(badgeRect, 18)
                g.FillPath(badgeBrush, path)
                Using p As New Pen(Color.FromArgb(245, 158, 11), 1.5F)
                    g.DrawPath(p, path)
                End Using
            End Using
        End Using

        Using fBadge As New Font("Segoe UI", 10, FontStyle.Bold)
            g.DrawString("PAMOJA RAIL SERVICES", fBadge, New SolidBrush(Color.White), 375, 26)
        End Using

        ' Central Card: Pure Vibrant Red with Gold Border
        Dim cardRect As New Rectangle(285, 65, 350, 480)
        Using cardBrush As New LinearGradientBrush(cardRect, Color.FromArgb(255, 0, 0), Color.FromArgb(204, 0, 0), 90.0F)
            Using path = GetRoundedRectPath(cardRect, 22)
                g.FillPath(cardBrush, path)
                Using p As New Pen(Color.FromArgb(245, 158, 11), 2.5F)
                    g.DrawPath(p, path)
                End Using
            End Using
        End Using

        ' Title and Subtitle inside Card
        Using fontTitle As New Font("Segoe UI", 18, FontStyle.Bold)
            g.DrawString("Portal Sign In", fontTitle, New SolidBrush(Color.White), 380, 85)
        End Using
        Using fontSub As New Font("Segoe UI", 10, FontStyle.Regular)
            g.DrawString("Select account type to proceed", fontSub, New SolidBrush(Color.FromArgb(241, 245, 249)), 370, 118)
        End Using

        ' Field Labels inside Card
        Using fontLbl As New Font("Segoe UI", 9, FontStyle.Bold)
            Dim lblBrush As New SolidBrush(Color.White)
            g.DrawString("SELECT ROLE", fontLbl, lblBrush, 320, 148)
            g.DrawString("USERNAME", fontLbl, lblBrush, 320, 213)
            g.DrawString("PASSWORD", fontLbl, lblBrush, 320, 278)
        End Using
    End Sub

    Private Function GetRoundedRectPath(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim d = radius * 2
        path.AddArc(rect.X, rect.Y, d, d, 180, 90)
        path.AddArc(rect.X + rect.Width - d, rect.Y, d, d, 270, 90)
        path.AddArc(rect.X + rect.Width - d, rect.Y + rect.Height - d, d, d, 0, 90)
        path.AddArc(rect.X, rect.Y + rect.Height - d, d, d, 90, 90)
        path.CloseFigure()
        Return path
    End Function

    Private Sub BtnLogin_Click(sender As Object, e As EventArgs)
        Dim u As String = txtUser.Text.Trim()
        Dim p As String = txtPass.Text.Trim()

        If String.IsNullOrEmpty(u) OrElse String.IsNullOrEmpty(p) Then
            MessageBox.Show("Please enter both username and password.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim targetRole = If(cmbRole.SelectedIndex = 0, "Admin", "Member")
        Dim userAccount As UserAccount = Nothing
        Dim errorMsg As String = Nothing

        If DataService.Instance.ValidateLogin(u, p, targetRole, userAccount, errorMsg) Then
            LoggedInUser = userAccount
            UserRole = userAccount.Role
            Dim dashboard As New MainDashboard(UserRole)
            Me.Hide()
            dashboard.ShowDialog()
            Me.Close()
        Else
            MessageBox.Show(errorMsg, "Sign In Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub BtnSignUp_Click(sender As Object, e As EventArgs)
        Dim name = InputBox("Enter your Full Name:", "Passenger Registration")
        If String.IsNullOrWhiteSpace(name) Then Return

        Dim username = InputBox("Choose a Username:", "Passenger Registration")
        If String.IsNullOrWhiteSpace(username) Then Return

        Dim password = InputBox("Choose a Password:", "Passenger Registration")
        If String.IsNullOrWhiteSpace(password) Then Return

        Dim err As String = Nothing
        If DataService.Instance.RegisterMember(name.Trim(), username.Trim(), password.Trim(), err) Then
            MessageBox.Show("Registration successful! You can now select 'Member Portal' and log in.", "Account Created", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cmbRole.SelectedIndex = 1
            txtUser.Text = username.Trim()
            txtPass.Text = ""
        Else
            MessageBox.Show(err, "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class