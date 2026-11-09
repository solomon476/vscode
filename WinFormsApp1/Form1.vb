Imports System.Drawing
Imports System.Windows.Forms

Public Class Form1
    Inherits Form

    Public Property UserRole As String = "Admin"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Visible = False
        Me.ShowInTaskbar = False

        Dim login As New SignUpForm()
        If login.ShowDialog() = DialogResult.OK Then
            Me.Visible = True
            Me.ShowInTaskbar = True
            Me.WindowState = FormWindowState.Maximized
            Me.BackColor = Color.FromArgb(15, 23, 42)
            Me.Text = "Pamoja Rail Services - Control Center (" & UserRole & " Mode)"
            Me.Controls.Clear()
            BuildDashboardUI()
        Else
            Application.Exit()
        End If
    End Sub

    ' Programmatically build clean dashboard buttons with standard method event handlers
    Private Sub BuildDashboardUI()
        Dim startX As Integer = 100
        Dim startY As Integer = 150

        Dim btnBookings As New Button() With {.Text = "Manage Bookings", .Location = New Point(startX, startY), .Size = New Size(220, 60), .BackColor = Color.FromArgb(30, 41, 59), .ForeColor = Color.White, .Font = New Font("Segoe UI", 11, FontStyle.Bold)}
        AddHandler btnBookings.Click, AddressOf OpenBooking

        Dim btnPassengers As New Button() With {.Text = "Passenger Records", .Location = New Point(startX + 250, startY), .Size = New Size(220, 60), .BackColor = Color.FromArgb(30, 41, 59), .ForeColor = Color.White, .Font = New Font("Segoe UI", 11, FontStyle.Bold)}
        AddHandler btnPassengers.Click, AddressOf OpenPassenger

        Dim btnSchedules As New Button() With {.Text = "Train Schedules", .Location = New Point(startX + 500, startY), .Size = New Size(220, 60), .BackColor = Color.FromArgb(30, 41, 59), .ForeColor = Color.White, .Font = New Font("Segoe UI", 11, FontStyle.Bold)}
        AddHandler btnSchedules.Click, AddressOf OpenTrain

        Me.Controls.Add(btnBookings)
        Me.Controls.Add(btnPassengers)
        Me.Controls.Add(btnSchedules)

        ' Show Reports only if Admin
        If UserRole = "Admin" Then
            Dim btnReports As New Button() With {.Text = "System Reports", .Location = New Point(startX + 750, startY), .Size = New Size(220, 60), .BackColor = Color.FromArgb(217, 119, 6), .ForeColor = Color.White, .Font = New Font("Segoe UI", 11, FontStyle.Bold)}
            AddHandler btnReports.Click, AddressOf OpenReport
            Me.Controls.Add(btnReports)
        End If

        ' Logout Button
        Dim btnLogout As New Button() With {.Text = "Logout", .Location = New Point(startX, startY + 100), .Size = New Size(150, 40), .BackColor = Color.FromArgb(185, 28, 28), .ForeColor = Color.White, .Font = New Font("Segoe UI", 10, FontStyle.Bold)}
        AddHandler btnLogout.Click, AddressOf DoLogout
        Me.Controls.Add(btnLogout)
    End Sub

    ' Standard event handler methods (Zero lambda errors)
    Private Sub OpenBooking(sender As Object, e As EventArgs)
        Dim f As New BookingForm()
        f.ShowDialog()
    End Sub

    Private Sub OpenPassenger(sender As Object, e As EventArgs)
        Dim f As New PassengerForm()
        f.ShowDialog()
    End Sub

    Private Sub OpenTrain(sender As Object, e As EventArgs)
        Dim f As New TrainForm()
        f.ShowDialog()
    End Sub

    Private Sub OpenReport(sender As Object, e As EventArgs)
        Dim f As New ReportForm()
        f.ShowDialog()
    End Sub

    Private Sub DoLogout(sender As Object, e As EventArgs)
        Me.Hide()
        Dim f As New Form1()
        f.Show()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        Using bgBrush As New SolidBrush(Color.FromArgb(30, 41, 59))
            g.FillRectangle(bgBrush, 0, 0, Me.Width, 80)
        End Using

        Using f As New Font("Segoe UI", 18, FontStyle.Bold)
            g.DrawString("Pamoja Rail Services - Control Center", f, New SolidBrush(Color.White), 30, 22)
        End Using

        Dim modeColor As Color = If(UserRole = "Admin", Color.FromArgb(234, 179, 8), Color.LimeGreen)
        Using f As New Font("Segoe UI", 11, FontStyle.Bold)
            g.DrawString("MODE: " & UserRole.ToUpper(), f, New SolidBrush(modeColor), Me.Width - 250, 28)
        End Using
    End Sub

End Class