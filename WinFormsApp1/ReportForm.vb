Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class ReportForm
    Inherits Form

    Private lblTotalRevenue As Label
    Private lblTotalPassengers As Label
    Private lblTotalBookings As Label
    Private lblTotalTrains As Label
    Private dgvReportSummary As DataGridView
    Private cmbReportType As ComboBox
    Private btnGenerate As Button
    Private btnClose As Button

    Private connectionString As String = "server=localhost;database=pamoja_rail;uid=root;pwd=;"

    Public Sub New()
        Me.Text = "Pamoja Rail Services - System Analytics & Reports"
        Me.Size = New Size(1000, 650)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.DoubleBuffered = True
        Me.BackColor = Color.FromArgb(248, 250, 252)

        BuildUI()
    End Sub

    Private Sub BuildUI()
        Me.Controls.Clear()

        ' Title Header
        Dim lblTitle As New Label() With {
            .Text = "System Analytics & Operational Reports",
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .ForeColor = Color.FromArgb(230, 0, 0),
            .Location = New Point(35, 20),
            .AutoSize = True,
            .BackColor = Color.Transparent
        }
        Me.Controls.Add(lblTitle)

        ' Left Control Card
        Dim pnlForm As New Panel() With {
            .Location = New Point(35, 75),
            .Size = New Size(300, 520),
            .BackColor = Color.White
        }

        Dim topBar As New Panel() With {.Dock = DockStyle.Top, .Height = 4, .BackColor = Color.FromArgb(230, 0, 0)}
        pnlForm.Controls.Add(topBar)

        Dim lblFormTitle As New Label() With {
            .Text = "Report Generator",
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .ForeColor = Color.FromArgb(230, 0, 0),
            .Location = New Point(20, 16),
            .AutoSize = True
        }
        pnlForm.Controls.Add(lblFormTitle)

        ' Select Report View
        Dim lblSelectType As New Label() With {.Text = "SELECT REPORT VIEW", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 60), .AutoSize = True}
        cmbReportType = New ComboBox() With {.Location = New Point(20, 83), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10), .DropDownStyle = ComboBoxStyle.DropDownList}
        cmbReportType.Items.AddRange(New String() {"Executive Financial Ledger", "Passenger Manifest Summary", "Train Fleet Audit"})
        cmbReportType.SelectedIndex = 0
        pnlForm.Controls.AddRange(New Control() {lblSelectType, cmbReportType})

        ' Generate Button
        btnGenerate = New Button() With {
            .Text = "Generate Report",
            .Location = New Point(20, 135),
            .Size = New Size(260, 44),
            .BackColor = Color.FromArgb(230, 0, 0),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnGenerate.FlatAppearance.BorderSize = 0
        AddHandler btnGenerate.Click, AddressOf GenerateReport
        pnlForm.Controls.Add(btnGenerate)

        ' Summary Metrics Group
        Dim lblMetrics As New Label() With {.Text = "KEY PERFORMANCE METRICS", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 210), .AutoSize = True}
        pnlForm.Controls.Add(lblMetrics)

        Dim rev = 0
        For Each b In DataService.Instance.Bookings
            rev += b.Fare
        Next

        lblTotalRevenue = New Label() With {.Text = "Total Revenue: KES " & rev.ToString("N0"), .Font = New Font("Segoe UI", 10, FontStyle.Bold), .ForeColor = Color.FromArgb(230, 0, 0), .Location = New Point(20, 238), .AutoSize = True}
        lblTotalBookings = New Label() With {.Text = "Confirmed Bookings: " & DataService.Instance.Bookings.Count, .Font = New Font("Segoe UI", 9.5F, FontStyle.Regular), .ForeColor = Color.FromArgb(15, 23, 42), .Location = New Point(20, 270), .AutoSize = True}
        lblTotalPassengers = New Label() With {.Text = "Registered Members: " & DataService.Instance.Users.FindAll(Function(u) u.Role = "Member").Count, .Font = New Font("Segoe UI", 9.5F, FontStyle.Regular), .ForeColor = Color.FromArgb(15, 23, 42), .Location = New Point(20, 300), .AutoSize = True}
        lblTotalTrains = New Label() With {.Text = "Active Trains: " & DataService.Instance.Trains.Count, .Font = New Font("Segoe UI", 9.5F, FontStyle.Regular), .ForeColor = Color.FromArgb(15, 23, 42), .Location = New Point(20, 330), .AutoSize = True}
        pnlForm.Controls.AddRange(New Control() {lblTotalRevenue, lblTotalBookings, lblTotalPassengers, lblTotalTrains})

        ' Close Button
        btnClose = New Button() With {
            .Text = "Return to Dashboard",
            .Location = New Point(20, 455),
            .Size = New Size(260, 42),
            .BackColor = Color.FromArgb(241, 245, 249),
            .ForeColor = Color.FromArgb(100, 116, 139),
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225)
        AddHandler btnClose.Click, Sub() Me.Close()
        pnlForm.Controls.Add(btnClose)

        Me.Controls.Add(pnlForm)

        ' DataGridView Table Card
        dgvReportSummary = New DataGridView() With {
            .Location = New Point(355, 75),
            .Size = New Size(605, 520),
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .ForeColor = Color.FromArgb(15, 23, 42),
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .RowHeadersVisible = False,
            .Font = New Font("Segoe UI", 9.5F)
        }
        dgvReportSummary.EnableHeadersVisualStyles = False
        dgvReportSummary.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 0, 0)
        dgvReportSummary.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvReportSummary.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        dgvReportSummary.ColumnHeadersHeight = 36
        Me.Controls.Add(dgvReportSummary)

        GenerateReport(Nothing, Nothing)
    End Sub

    Private Sub GenerateReport(sender As Object, e As EventArgs)
        Select Case cmbReportType.SelectedIndex
            Case 0
                dgvReportSummary.DataSource = DataService.Instance.GetBookingsDataTable()
            Case 1
                dgvReportSummary.DataSource = DataService.Instance.GetPassengersDataTable()
            Case 2
                dgvReportSummary.DataSource = DataService.Instance.GetTrainsDataTable()
            Case Else
                dgvReportSummary.DataSource = DataService.Instance.GetBookingsDataTable()
        End Select
    End Sub
End Class