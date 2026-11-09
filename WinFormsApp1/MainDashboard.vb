Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.IO

Partial Public Class MainDashboard
    Inherits Form

    Public Property CurrentRole As String
    Public Property CurrentUsername As String

    Private btnManageBookings As Button
    Private btnPassengerRecords As Button
    Private btnTrainSchedules As Button
    Private btnSystemReports As Button
    Private btnUserManagement As Button
    Private btnUserLogout As Button

    Public Sub New()
        InitializeComponent()
        CurrentRole = "Admin"
        CurrentUsername = "Administrator"
        Me.Text = "Pamoja Rail Services - Control Center (Admin Mode)"
        Me.Size = New Size(1050, 700)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.DoubleBuffered = True
        Me.BackColor = Color.FromArgb(248, 250, 252)
    End Sub

    Public Sub New(role As String)
        Me.New()
        CurrentRole = role
        Dim currentUser = DataService.Instance.CurrentUser
        If currentUser IsNot Nothing Then
            CurrentUsername = currentUser.FullName
        Else
            CurrentUsername = If(role = "Admin", "Administrator", "Member")
        End If
        Me.Text = "Pamoja Rail Services - Control Center (" & CurrentRole & " Mode)"
    End Sub

    Private Sub MainDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Controls.Clear()
        BuildSidebarNavigation()
        BuildDashboardMetrics()
    End Sub

    Private Sub BuildSidebarNavigation()
        ' Sidebar panel
        Dim pnlSidebar As New Panel() With {
            .Dock = DockStyle.Left,
            .Width = 250,
            .BackColor = Color.FromArgb(230, 0, 0)
        }

        Dim lblLogo As New Label() With {
            .Text = "PAMOJA RAIL SERVICES",
            .Font = New Font("Segoe UI", 11, FontStyle.Bold),
            .ForeColor = Color.FromArgb(254, 240, 138),
            .Location = New Point(18, 20),
            .AutoSize = True
        }
        pnlSidebar.Controls.Add(lblLogo)

        Dim lblPortalMode As New Label() With {
            .Text = If(CurrentRole = "Admin", "Admin Control Center", "Passenger Portal"),
            .Font = New Font("Segoe UI", 9, FontStyle.Regular),
            .ForeColor = Color.White,
            .Location = New Point(18, 44),
            .AutoSize = True
        }
        pnlSidebar.Controls.Add(lblPortalMode)

        Dim startY As Integer = 100
        Dim btnSpacing As Integer = 56

        If CurrentRole = "Admin" Then
            btnManageBookings = CreateNavButton("Manage Bookings", 18, startY)
            AddHandler btnManageBookings.Click, AddressOf BtnBookings_Click
            pnlSidebar.Controls.Add(btnManageBookings)

            btnPassengerRecords = CreateNavButton("Passenger Records", 18, startY + btnSpacing)
            AddHandler btnPassengerRecords.Click, AddressOf BtnPassengers_Click
            pnlSidebar.Controls.Add(btnPassengerRecords)

            btnTrainSchedules = CreateNavButton("Train Fleet & Routes", 18, startY + btnSpacing * 2)
            AddHandler btnTrainSchedules.Click, AddressOf BtnSchedules_Click
            pnlSidebar.Controls.Add(btnTrainSchedules)

            btnSystemReports = CreateNavButton("System Reports", 18, startY + btnSpacing * 3)
            AddHandler btnSystemReports.Click, AddressOf BtnReports_Click
            pnlSidebar.Controls.Add(btnSystemReports)

            btnUserManagement = CreateNavButton("User Management", 18, startY + btnSpacing * 4)
            AddHandler btnUserManagement.Click, AddressOf BtnUserManagement_Click
            pnlSidebar.Controls.Add(btnUserManagement)
        Else
            btnManageBookings = CreateNavButton("Book a Ticket", 18, startY)
            AddHandler btnManageBookings.Click, AddressOf BtnBookings_Click
            pnlSidebar.Controls.Add(btnManageBookings)

            btnPassengerRecords = CreateNavButton("My Bookings", 18, startY + btnSpacing)
            AddHandler btnPassengerRecords.Click, AddressOf BtnPassengers_Click
            pnlSidebar.Controls.Add(btnPassengerRecords)

            btnTrainSchedules = CreateNavButton("Train Timetable", 18, startY + btnSpacing * 2)
            AddHandler btnTrainSchedules.Click, AddressOf BtnSchedules_Click
            pnlSidebar.Controls.Add(btnTrainSchedules)
        End If

        ' Logout Button at bottom
        btnUserLogout = New Button() With {
            .Text = "Log Out",
            .Location = New Point(18, 580),
            .Size = New Size(214, 44),
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .ForeColor = Color.White,
            .BackColor = Color.FromArgb(185, 0, 0),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnUserLogout.FlatAppearance.BorderSize = 0
        AddHandler btnUserLogout.Click, AddressOf BtnLogout_Click
        pnlSidebar.Controls.Add(btnUserLogout)

        Me.Controls.Add(pnlSidebar)
    End Sub

    Private Function CreateNavButton(text As String, x As Integer, y As Integer) As Button
        Dim btn As New Button() With {
            .Text = text,
            .Location = New Point(x, y),
            .Size = New Size(214, 46),
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .ForeColor = Color.White,
            .BackColor = Color.FromArgb(200, 0, 0),
            .FlatStyle = FlatStyle.Flat,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Padding = New Padding(12, 0, 0, 0),
            .Cursor = Cursors.Hand
        }
        btn.FlatAppearance.BorderSize = 0
        btn.FlatAppearance.MouseOverBackColor = Color.White
        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 240, 138)
        AddHandler btn.MouseEnter, Sub() btn.ForeColor = Color.FromArgb(230, 0, 0)
        AddHandler btn.MouseLeave, Sub() btn.ForeColor = Color.White
        Return btn
    End Function

    Private Sub BuildDashboardMetrics()
        ' Top Header Bar
        Dim pnlHeader As New Panel() With {
            .Dock = DockStyle.Top,
            .Height = 75,
            .BackColor = Color.White,
            .Padding = New Padding(270, 15, 20, 15)
        }

        Dim lblTitle As New Label() With {
            .Text = If(CurrentRole = "Admin", "Operations Overview Dashboard", "Welcome back, " & CurrentUsername & "!"),
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .ForeColor = Color.FromArgb(230, 0, 0),
            .Location = New Point(275, 15),
            .AutoSize = True
        }
        Me.Controls.Add(lblTitle)

        Dim lblSub As New Label() With {
            .Text = If(CurrentRole = "Admin", "Real-time railway network metrics, fleet and booking status", "Your personal Pamoja Rail travel overview"),
            .Font = New Font("Segoe UI", 9.5F, FontStyle.Regular),
            .ForeColor = Color.FromArgb(100, 116, 139),
            .Location = New Point(275, 46),
            .AutoSize = True
        }
        Me.Controls.Add(lblSub)

        ' Metric KPI Cards
        Dim startCardX As Integer = 275
        Dim cardY As Integer = 100
        Dim cardW As Integer = 220
        Dim cardH As Integer = 110
        Dim gap As Integer = 20

        If CurrentRole = "Admin" Then
            Dim totalBookings = DataService.Instance.Bookings.Count
            Dim totalMembers = DataService.Instance.Users.FindAll(Function(u) u.Role = "Member").Count
            Dim totalTrains = DataService.Instance.Trains.Count
            Dim onTimeTrains = DataService.Instance.Trains.FindAll(Function(t) t.Status.Contains("On Time")).Count

            AddMetricCard("TOTAL BOOKINGS", totalBookings.ToString(), Color.FromArgb(230, 0, 0), startCardX, cardY, cardW, cardH)
            AddMetricCard("REGISTERED MEMBERS", totalMembers.ToString(), Color.FromArgb(230, 0, 0), startCardX + cardW + gap, cardY, cardW, cardH)
            AddMetricCard("ACTIVE FLEET", totalTrains.ToString(), Color.FromArgb(245, 158, 11), startCardX + (cardW + gap) * 2, cardY, cardW, cardH)
            AddMetricCard("ON-TIME TRAINS", onTimeTrains.ToString(), Color.FromArgb(22, 163, 74), startCardX, cardY + cardH + gap, cardW, cardH)
            AddMetricCard("DELAYED / ALERT", (totalTrains - onTimeTrains).ToString(), Color.FromArgb(220, 38, 38), startCardX + cardW + gap, cardY + cardH + gap, cardW, cardH)
        Else
            Dim currentUname = If(DataService.Instance.CurrentUser IsNot Nothing, DataService.Instance.CurrentUser.Username, "")
            Dim myBookings = DataService.Instance.Bookings.FindAll(Function(b) b.MemberUsername.Equals(currentUname, StringComparison.OrdinalIgnoreCase))
            Dim totalSpent As Integer = 0
            For Each b In myBookings
                totalSpent += b.Fare
            Next
            Dim totalTrains = DataService.Instance.Trains.FindAll(Function(t) t.Status.Contains("On Time")).Count

            AddMetricCard("MY BOOKINGS", myBookings.Count.ToString(), Color.FromArgb(230, 0, 0), startCardX, cardY, cardW, cardH)
            AddMetricCard("AVAILABLE ROUTES", totalTrains.ToString(), Color.FromArgb(245, 158, 11), startCardX + cardW + gap, cardY, cardW, cardH)
            AddMetricCard("TOTAL SPENT (KES)", totalSpent.ToString("N0"), Color.FromArgb(22, 163, 74), startCardX + (cardW + gap) * 2, cardY, cardW, cardH)
        End If
    End Sub

    Private Sub AddMetricCard(title As String, value As String, accentColor As Color, x As Integer, y As Integer, w As Integer, h As Integer)
        Dim pnlCard As New Panel() With {
            .Location = New Point(x, y),
            .Size = New Size(w, h),
            .BackColor = Color.White
        }

        Dim topBorder As New Panel() With {
            .Dock = DockStyle.Top,
            .Height = 4,
            .BackColor = accentColor
        }
        pnlCard.Controls.Add(topBorder)

        Dim lblVal As New Label() With {
            .Text = value,
            .Font = New Font("Segoe UI", 22, FontStyle.Bold),
            .ForeColor = Color.FromArgb(230, 0, 0),
            .Location = New Point(18, 18),
            .AutoSize = True
        }
        pnlCard.Controls.Add(lblVal)

        Dim lblLbl As New Label() With {
            .Text = title,
            .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(100, 116, 139),
            .Location = New Point(18, 68),
            .AutoSize = True
        }
        pnlCard.Controls.Add(lblLbl)

        Me.Controls.Add(pnlCard)
    End Sub

    Private Sub BtnBookings_Click(sender As Object, e As EventArgs)
        Dim f As New BookingForm()
        f.ShowDialog()
        RefreshMetrics()
    End Sub

    Private Sub BtnPassengers_Click(sender As Object, e As EventArgs)
        Dim f As New PassengerForm()
        f.ShowDialog()
        RefreshMetrics()
    End Sub

    Private Sub BtnSchedules_Click(sender As Object, e As EventArgs)
        Dim f As New TrainForm()
        f.ShowDialog()
        RefreshMetrics()
    End Sub

    Private Sub BtnReports_Click(sender As Object, e As EventArgs)
        If CurrentRole <> "Admin" Then
            MessageBox.Show("Access Denied: System Analytics and Reports require Administrator privileges.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim f As New ReportForm()
        f.ShowDialog()
    End Sub

    Private Sub BtnUserManagement_Click(sender As Object, e As EventArgs)
        If CurrentRole <> "Admin" Then
            MessageBox.Show("Access Denied: User Management requires Administrator privileges.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim f As New UserManagementForm()
        f.ShowDialog()
        RefreshMetrics()
    End Sub

    Private Sub RefreshMetrics()
        Me.Controls.Clear()
        BuildSidebarNavigation()
        BuildDashboardMetrics()
    End Sub

    Private Sub BtnLogout_Click(sender As Object, e As EventArgs)
        DataService.Instance.CurrentUser = Nothing
        Me.Hide()
        Dim login As New SignUpForm()
        login.ShowDialog()
        Me.Close()
    End Sub
End Class