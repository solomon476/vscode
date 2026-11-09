Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class BookingForm
    Inherits Form

    Private txtPassengerName As TextBox
    Private txtRoute As TextBox
    Private cmbClass As ComboBox
    Private dtpTravelDate As DateTimePicker
    Private dgvBookings As DataGridView
    Private btnSaveBooking As Button
    Private btnClear As Button
    Private btnClose As Button

    Private connectionString As String = "server=localhost;database=pamoja_rail;uid=root;pwd=;"

    Public Sub New()
        InitializeComponent()
        Me.Text = "Pamoja Rail Services - Booking Management"
        Me.Size = New Size(1000, 650)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.DoubleBuffered = True
        Me.BackColor = Color.FromArgb(248, 250, 252)
    End Sub

    Private Sub BookingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Controls.Clear()

        ' Title Header
        Dim lblTitle As New Label() With {
            .Text = "Rail Ticket Booking & Reservation Portal",
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .ForeColor = Color.FromArgb(230, 0, 0),
            .Location = New Point(35, 20),
            .AutoSize = True,
            .BackColor = Color.Transparent
        }
        Me.Controls.Add(lblTitle)

        ' Left Card for Inputs
        Dim pnlForm As New Panel() With {
            .Location = New Point(35, 75),
            .Size = New Size(300, 520),
            .BackColor = Color.White
        }

        Dim topBar As New Panel() With {.Dock = DockStyle.Top, .Height = 4, .BackColor = Color.FromArgb(230, 0, 0)}
        pnlForm.Controls.Add(topBar)

        Dim lblFormTitle As New Label() With {
            .Text = "New Ticket Reservation",
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .ForeColor = Color.FromArgb(230, 0, 0),
            .Location = New Point(20, 16),
            .AutoSize = True
        }
        pnlForm.Controls.Add(lblFormTitle)

        ' Passenger Name
        Dim lblName As New Label() With {.Text = "PASSENGER FULL NAME", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 55), .AutoSize = True}
        txtPassengerName = New TextBox() With {.Location = New Point(20, 78), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10)}
        pnlForm.Controls.AddRange(New Control() {lblName, txtPassengerName})

        ' Route
        Dim lblRouteTarget As New Label() With {.Text = "ROUTE / DESTINATION", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 120), .AutoSize = True}
        txtRoute = New TextBox() With {.Location = New Point(20, 143), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10), .Text = "Nairobi - Mombasa"}
        pnlForm.Controls.AddRange(New Control() {lblRouteTarget, txtRoute})

        ' Class
        Dim lblClass As New Label() With {.Text = "TICKET CLASS", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 185), .AutoSize = True}
        cmbClass = New ComboBox() With {.Location = New Point(20, 208), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10), .DropDownStyle = ComboBoxStyle.DropDownList}
        cmbClass.Items.AddRange(New String() {"Economy", "First Class", "VIP Sleeper"})
        cmbClass.SelectedIndex = 0
        pnlForm.Controls.AddRange(New Control() {lblClass, cmbClass})

        ' Date
        Dim lblDate As New Label() With {.Text = "TRAVEL DATE", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 250), .AutoSize = True}
        dtpTravelDate = New DateTimePicker() With {.Location = New Point(20, 273), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10), .Format = DateTimePickerFormat.Short}
        pnlForm.Controls.AddRange(New Control() {lblDate, dtpTravelDate})

        ' Save Button
        btnSaveBooking = New Button() With {
            .Text = "Confirm Booking",
            .Location = New Point(20, 335),
            .Size = New Size(125, 42),
            .BackColor = Color.FromArgb(230, 0, 0),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnSaveBooking.FlatAppearance.BorderSize = 0
        AddHandler btnSaveBooking.Click, AddressOf SaveBookingRecord
        pnlForm.Controls.Add(btnSaveBooking)

        ' Clear Button
        btnClear = New Button() With {
            .Text = "Clear",
            .Location = New Point(155, 335),
            .Size = New Size(125, 42),
            .BackColor = Color.FromArgb(241, 245, 249),
            .ForeColor = Color.FromArgb(100, 116, 139),
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnClear.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225)
        AddHandler btnClear.Click, Sub() ClearFields()
        pnlForm.Controls.Add(btnClear)

        ' Return Button
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
        dgvBookings = New DataGridView() With {
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
        dgvBookings.EnableHeadersVisualStyles = False
        dgvBookings.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 0, 0)
        dgvBookings.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvBookings.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        dgvBookings.ColumnHeadersHeight = 36
        Me.Controls.Add(dgvBookings)

        LoadBookings()
    End Sub

    Private Sub SaveBookingRecord(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtPassengerName.Text) OrElse String.IsNullOrWhiteSpace(txtRoute.Text) Then
            MessageBox.Show("Please fill out passenger name and route.", "Required Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim pName = txtPassengerName.Text.Trim()
        Dim route = txtRoute.Text.Trim()
        Dim tClass = cmbClass.SelectedItem.ToString()
        Dim tDate = dtpTravelDate.Value.ToString("yyyy-MM-dd")
        Dim fare = If(tClass = "Economy", 1500, If(tClass = "First Class", 3000, 6000))
        Dim currentUser = If(DataService.Instance.CurrentUser IsNot Nothing, DataService.Instance.CurrentUser.Username, "guest")

        ' Save to DataService
        Dim refCode = "PRS-" & (9400 + DataService.Instance.Bookings.Count + 1)
        DataService.Instance.Bookings.Add(New BookingRecord With {
            .Id = "b-" & (DataService.Instance.Bookings.Count + 1),
            .TicketRef = refCode,
            .TrainNumber = "Express 101",
            .Route = route,
            .PassengerName = pName,
            .TravelClass = tClass,
            .TravelDate = tDate,
            .Fare = fare,
            .MemberUsername = currentUser,
            .Status = "Confirmed"
        })

        ' Also attempt MySQL save if server is running
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim query As String = "INSERT INTO bookings (passenger_name, route, ticket_class, travel_date) VALUES (@name, @route, @tclass, @tdate)"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@name", pName)
                    cmd.Parameters.AddWithValue("@route", route)
                    cmd.Parameters.AddWithValue("@tclass", tClass)
                    cmd.Parameters.AddWithValue("@tdate", tDate)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ' MySQL optional fallback
        End Try

        MessageBox.Show("Ticket booked successfully! Ref: " & refCode, "Reservation Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ClearFields()
        LoadBookings()
    End Sub

    Private Sub LoadBookings()
        Dim isMember = (DataService.Instance.CurrentUser IsNot Nothing AndAlso DataService.Instance.CurrentUser.Role = "Member")
        Dim filter = If(isMember, DataService.Instance.CurrentUser.Username, Nothing)
        dgvBookings.DataSource = DataService.Instance.GetBookingsDataTable(filter)
    End Sub

    Private Sub ClearFields()
        txtPassengerName.Clear()
        txtRoute.Text = "Nairobi - Mombasa"
        cmbClass.SelectedIndex = 0
        dtpTravelDate.Value = DateTime.Now
    End Sub
End Class