Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class TrainForm
    Inherits Form

    Private txtTrainNumber As TextBox
    Private txtRouteName As TextBox
    Private txtCapacity As TextBox
    Private cmbStatus As ComboBox
    Private dgvTrains As DataGridView
    Private btnSaveTrain As Button
    Private btnClear As Button
    Private btnClose As Button

    Private connectionString As String = "server=localhost;database=pamoja_rail;uid=root;pwd=;"

    Public Sub New()
        InitializeComponent()
        Me.Text = "Pamoja Rail Services - Train Management"
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
            .Text = "Train Fleet & Timetable Operations",
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .ForeColor = Color.FromArgb(230, 0, 0),
            .Location = New Point(35, 20),
            .AutoSize = True,
            .BackColor = Color.Transparent
        }
        Me.Controls.Add(lblTitle)

        ' Left Form Panel
        Dim pnlForm As New Panel() With {
            .Location = New Point(35, 75),
            .Size = New Size(300, 520),
            .BackColor = Color.White
        }

        Dim topBar As New Panel() With {.Dock = DockStyle.Top, .Height = 4, .BackColor = Color.FromArgb(230, 0, 0)}
        pnlForm.Controls.Add(topBar)

        Dim lblFormTitle As New Label() With {
            .Text = "Add / Update Train Fleet",
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .ForeColor = Color.FromArgb(230, 0, 0),
            .Location = New Point(20, 16),
            .AutoSize = True
        }
        pnlForm.Controls.Add(lblFormTitle)

        ' Train Number
        Dim lblNumber As New Label() With {.Text = "TRAIN NUMBER / NAME", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 60), .AutoSize = True}
        txtTrainNumber = New TextBox() With {.Location = New Point(20, 83), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10), .Text = "Express 102"}
        pnlForm.Controls.AddRange(New Control() {lblNumber, txtTrainNumber})

        ' Route
        Dim lblRoute As New Label() With {.Text = "OPERATIONAL ROUTE", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 130), .AutoSize = True}
        txtRouteName = New TextBox() With {.Location = New Point(20, 153), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10), .Text = "Nairobi - Mombasa"}
        pnlForm.Controls.AddRange(New Control() {lblRoute, txtRouteName})

        ' Capacity
        Dim lblCap As New Label() With {.Text = "SEAT CAPACITY", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 200), .AutoSize = True}
        txtCapacity = New TextBox() With {.Location = New Point(20, 223), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10), .Text = "560"}
        pnlForm.Controls.AddRange(New Control() {lblCap, txtCapacity})

        ' Status
        Dim lblStat As New Label() With {.Text = "OPERATIONAL STATUS", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 270), .AutoSize = True}
        cmbStatus = New ComboBox() With {.Location = New Point(20, 293), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10), .DropDownStyle = ComboBoxStyle.DropDownList}
        cmbStatus.Items.AddRange(New String() {"On Time / Active", "Delayed", "Maintenance", "Cancelled"})
        cmbStatus.SelectedIndex = 0
        pnlForm.Controls.AddRange(New Control() {lblStat, cmbStatus})

        ' Save Button
        btnSaveTrain = New Button() With {
            .Text = "Save Train",
            .Location = New Point(20, 355),
            .Size = New Size(125, 42),
            .BackColor = Color.FromArgb(230, 0, 0),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnSaveTrain.FlatAppearance.BorderSize = 0
        AddHandler btnSaveTrain.Click, AddressOf SaveTrainRecord
        pnlForm.Controls.Add(btnSaveTrain)

        ' Clear Button
        btnClear = New Button() With {
            .Text = "Clear",
            .Location = New Point(155, 355),
            .Size = New Size(125, 42),
            .BackColor = Color.FromArgb(241, 245, 249),
            .ForeColor = Color.FromArgb(100, 116, 139),
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnClear.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225)
        AddHandler btnClear.Click, Sub()
            txtTrainNumber.Clear()
            txtRouteName.Clear()
            txtCapacity.Text = "500"
            cmbStatus.SelectedIndex = 0
        End Sub
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
        dgvTrains = New DataGridView() With {
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
        dgvTrains.EnableHeadersVisualStyles = False
        dgvTrains.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 0, 0)
        dgvTrains.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvTrains.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        dgvTrains.ColumnHeadersHeight = 36
        Me.Controls.Add(dgvTrains)

        LoadTrains()
    End Sub

    Private Sub SaveTrainRecord(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtTrainNumber.Text) OrElse String.IsNullOrWhiteSpace(txtRouteName.Text) Then
            MessageBox.Show("Please enter train number and route.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim cap As Integer = 500
        Integer.TryParse(txtCapacity.Text, cap)

        DataService.Instance.Trains.Add(New TrainRecord With {
            .Id = "t-" & (DataService.Instance.Trains.Count + 1),
            .TrainNumber = txtTrainNumber.Text.Trim(),
            .Route = txtRouteName.Text.Trim(),
            .DepartureStation = "Nairobi Terminus",
            .ArrivalStation = "Terminus",
            .DepartureTime = "09:00",
            .Capacity = cap,
            .EconomyTariff = 1500,
            .FirstClassTariff = 3000,
            .VIPTariff = 6000,
            .Status = cmbStatus.SelectedItem.ToString()
        })

        MessageBox.Show("Train fleet record saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        LoadTrains()
    End Sub

    Private Sub LoadTrains()
        dgvTrains.DataSource = DataService.Instance.GetTrainsDataTable()
    End Sub
End Class